﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService;

namespace RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService
{
    public class ThirdPartyAddressLocationBusinessService : IThirdPartyAddressLocationBusinessService
    {
        private IAddressLocationDataService addressLocationDataService = default(IAddressLocationDataService);
        private IThirdPartyAddressLocationIntegrationService thirdPartyAddressLocationIntegrationService = default(IThirdPartyAddressLocationIntegrationService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public ThirdPartyAddressLocationBusinessService(
                                                        IAddressLocationDataService addressLocationDataService,
                                                        IThirdPartyAddressLocationIntegrationService thirdPartyAddressLocationIntegrationService,
                                                        ILoggingHelper loggingHelper)
        {
            this.addressLocationDataService = addressLocationDataService;
            this.thirdPartyAddressLocationIntegrationService = thirdPartyAddressLocationIntegrationService;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// This method is used to fetch data for Access Links.
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <returns>Address Location DTO</returns>
        public async Task<object> GetAddressLocationByUDPRNJson(int uDPRN)
        {
            AddressLocationDTO addressLocationDto = await this.addressLocationDataService.GetAddressLocationByUDPRN(uDPRN);
            return GetAddressLocationJsonData(addressLocationDto);
        }

        /// <summary>
        /// Get AddressLocation by UDPRN
        /// </summary>
        /// <param name="udprn">UDPRN id</param>
        /// <returns>AddressLocationDTO object</returns>
        public async Task<AddressLocationDTO> GetAddressLocationByUDPRN(int uDPRN)
        {
            return await addressLocationDataService.GetAddressLocationByUDPRN(uDPRN);
        }

        #region Save USR Details to Database

        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos">List of Address Locations</param>
        /// <returns>Task</returns>
        public async Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos)
        {
            int fileUdprn;

            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            foreach (AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO in addressLocationUsrpostdtos)
            {
                // Get the udprn id for each USR record.
                fileUdprn = (int)addressLocationUSRPOSTDTO.UDPRN;

                if (!await addressLocationDataService.AddressLocationExists(fileUdprn))
                {
                    DbGeometry spatialLocationXY = GetSpatialLocation(addressLocationUSRPOSTDTO);

                    AddressLocationDTO newAddressLocationDTO = new AddressLocationDTO()
                    {
                        ID = Guid.NewGuid(),
                        UDPRN = addressLocationUSRPOSTDTO.UDPRN,
                        LocationXY = spatialLocationXY,
                        Lattitude = addressLocationUSRPOSTDTO.Latitude,
                        Longitude = addressLocationUSRPOSTDTO.Longitude
                    };

                    // Save the address location data record to the database.
                    await addressLocationDataService.SaveNewAddressLocation(newAddressLocationDTO);

                    // Check if the delivery point exists
                    if (await thirdPartyAddressLocationIntegrationService.DeliveryPointExists((int)fileUdprn))
                    {
                        DeliveryPointDTO deliveryPointDTO =
                            await thirdPartyAddressLocationIntegrationService.GetDeliveryPointByUDPRNForThirdParty((int)fileUdprn);

                        // Check if the existing delivery point has a location.
                        if (deliveryPointDTO.LocationXY == null)
                        {
                            // Get the Location provider Guid from the reference data table.
                            Guid locationProviderId =
                                await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(Constants.NETWORKLINKDATAPROVIDER, Constants.EXTERNAL);

                            DeliveryPointDTO newDeliveryPoint = new DeliveryPointDTO
                            {
                                Latitude = addressLocationUSRPOSTDTO.Latitude,
                                Longitude = addressLocationUSRPOSTDTO.Longitude,
                                LocationXY = spatialLocationXY,
                                LocationProvider_GUID = locationProviderId,
                                UDPRN = fileUdprn
                            };

                            // Update the location details for the delivery point
                            await thirdPartyAddressLocationIntegrationService.UpdateDeliveryPointLocationOnUDPRN(newDeliveryPoint);

                            // Check if a notification exists for the UDPRN.
                            if (await thirdPartyAddressLocationIntegrationService.CheckIfNotificationExists(fileUdprn, Constants.USRACTION))
                            {
                                // Delete the notification if it exists.
                                await thirdPartyAddressLocationIntegrationService.DeleteNotificationbyUDPRNAndAction(fileUdprn, Constants.USRACTION);
                            }
                        }
                        else
                        {
                            // Calculates the straight line distance between the existing delivery
                            // point and the new delivery point.
                            var straightLineDistance = GetDeliveryPointDistance(deliveryPointDTO, spatialLocationXY);

                            // Check if the new point is within the tolerance limit
                            if (straightLineDistance < Constants.TOLERANCEDISTANCEINMETERS)
                            {
                                Guid locationProviderId = await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(
                                    Constants.NETWORKLINKDATAPROVIDER,
                                    Constants.EXTERNAL);

                                DeliveryPointDTO newDeliveryPoint = new DeliveryPointDTO
                                {
                                    UDPRN = fileUdprn,
                                    Latitude = addressLocationUSRPOSTDTO.Latitude,
                                    Longitude = addressLocationUSRPOSTDTO.Longitude,
                                    LocationXY = spatialLocationXY,
                                    LocationProvider_GUID = locationProviderId
                                };

                                // Update the delivery point location directly in case it is within
                                // the tolerance limits.
                                await thirdPartyAddressLocationIntegrationService.UpdateDeliveryPointLocationOnUDPRN(newDeliveryPoint);

                                // Check if the notification exists for the given UDPRN.
                                if (await thirdPartyAddressLocationIntegrationService.CheckIfNotificationExists(fileUdprn, Constants.USRACTION))
                                {
                                    // Delete the notification if it exists.
                                    await thirdPartyAddressLocationIntegrationService.DeleteNotificationbyUDPRNAndAction(fileUdprn, Constants.USRACTION);
                                }
                            }
                            else
                            {
                                // Get the Postcode Sector by UDPRN
                                PostCodeSectorDTO postCodeSectorDTO =
                                    await thirdPartyAddressLocationIntegrationService.GetPostCodeSectorByUDPRN(fileUdprn);

                                // Get the Notification Type from the Reference data table
                                Guid notificationTypeId_GUID = await thirdPartyAddressLocationIntegrationService.GetReferenceDataId(
                                    Constants.USRCATEGORY,
                                    Constants.USRREFERENCEDATANAME);

                                PostalAddressDTO postalAddressDTO = await thirdPartyAddressLocationIntegrationService.GetPostalAddress(fileUdprn);

                                NotificationDTO notificationDO = new NotificationDTO
                                {
                                    ID = Guid.NewGuid(),
                                    NotificationType_GUID = notificationTypeId_GUID,
                                    NotificationDueDate = DateTime.UtcNow.AddHours(Constants.NOTIFICATIONDUE),
                                    NotificationSource = Constants.USRNOTIFICATIONSOURCE,
                                    Notification_Heading = Constants.USRACTION,
                                    Notification_Message = AddressFields(postalAddressDTO),
                                    PostcodeDistrict = (postCodeSectorDTO == null || postCodeSectorDTO.District == null)
                                        ? string.Empty
                                        : postCodeSectorDTO.District,
                                    PostcodeSector = (postCodeSectorDTO == null || postCodeSectorDTO.Sector == null)
                                        ? string.Empty
                                        : postCodeSectorDTO.Sector,
                                    NotificationActionLink = string.Format(Constants.USRNOTIFICATIONLINK, fileUdprn)
                                };

                                // Insert the new notification.
                                await thirdPartyAddressLocationIntegrationService.AddNewNotification(notificationDO);
                            }
                        }
                    }
                }

                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
            }
        }

        /// <summary>
        /// This method is used to fetch GeoJson data for Address Location.
        /// </summary>
        /// <param name="addressLocationDto">Address Location DTO</param>
        /// <returns>lstDeliveryPointDTO</returns>
        private static object GetAddressLocationJsonData(AddressLocationDTO addressLocationDto)
        {
            var addressLocationGeoJson = new GeoJson
            {
                features = new List<Feature>()
            };

            if (addressLocationDto.LocationXY != null)
            {
                SqlGeometry addressLocationSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(addressLocationDto.LocationXY.AsBinary()), Constants.BNGCOORDINATESYSTEM);

                var feature = new Feature
                {
                    id = addressLocationDto.ID.ToString(),
                    properties = new Dictionary<string, JToken>
                    {
                        { Constants.UDPRN, addressLocationDto.UDPRN },
                        { Constants.Latitude, addressLocationDto.Lattitude },
                        { Constants.Longitude, addressLocationDto.Longitude },
                    },
                    geometry = new Geometry
                    {
                        coordinates = new double[] { addressLocationSqlGeometry.STX.Value, addressLocationSqlGeometry.STY.Value }
                    }
                };

                addressLocationGeoJson.features.Add(feature);
            }

            return addressLocationGeoJson;
        }

        #endregion Save USR Details to Database

        #region Calculate Spatial Location

        /// <summary>
        /// Get the geometry equivalent of the X-Y co-ordinate of address location
        /// </summary>
        /// <param name="addressLocationUSRPOSTDTO">AddressLocationUSRPOSTDTO object</param>
        /// <returns>DbGeometry object</returns>
        private static DbGeometry GetSpatialLocation(AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO)
        {
            string sbLocationXY = string.Format(
                Constants.USRGEOMETRYPOINT,
                Convert.ToString(addressLocationUSRPOSTDTO.XCoordinate),
                Convert.ToString(addressLocationUSRPOSTDTO.YCoordinate));

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNGCOORDINATESYSTEM);
            return spatialLocationXY;
        }

        #endregion Calculate Spatial Location

        /// <summary>
        /// Concatenating address fileds require for notification
        /// </summary>
        /// <param name="objPostalAddress">USR create event PostalAddressDTO</param>
        /// <returns>returns concatenated value of address field</returns>
        private string AddressFields(PostalAddressDTO objPostalAddress)
        {
            return Constants.PAFTaskBodyPreText +
                        objPostalAddress.OrganisationName + Constants.Comma +
                        objPostalAddress.DepartmentName + Constants.Comma +
                        objPostalAddress.BuildingName + Constants.Comma +
                        objPostalAddress.BuildingNumber + Constants.Comma +
                        objPostalAddress.SubBuildingName + Constants.Comma +
                        objPostalAddress.Thoroughfare + Constants.Comma +
                        objPostalAddress.DependentThoroughfare + Constants.Comma +
                        objPostalAddress.DependentLocality + Constants.Comma +
                        objPostalAddress.DoubleDependentLocality + Constants.Comma +
                        objPostalAddress.PostTown + Constants.Comma +
                        objPostalAddress.Postcode;
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        /// <param name="separator">separator</param>
        private void LogMethodInfoBlock(string methodName, string logMessage, string separator)
        {
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
        }

        /// <summary>
        /// Calculte distance between two points
        /// </summary>
        /// <param name="deliveryPointDTO">deliveryPoint DTO</param>
        /// <param name="newPoint">newPoint as DbGeometry</param>
        /// <returns>distance as double</returns>
        private double? GetDeliveryPointDistance(DeliveryPointDTO deliveryPointDTO, DbGeometry newPoint)
        {
            double? distance = 0;
            if (deliveryPointDTO != null)
            {
                distance = deliveryPointDTO.LocationXY.Distance(newPoint);
            }

            return distance;
        }
    }
}