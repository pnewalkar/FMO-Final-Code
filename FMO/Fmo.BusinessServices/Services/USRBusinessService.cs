namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Reflection;
    using System.Threading.Tasks;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.Common.Constants;
    using Fmo.Common.Interface;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.DTO.FileProcessing;

    /// <summary>
    /// Implements interface for USR Business service
    /// </summary>
    public class USRBusinessService : IUSRBusinessService
    {
        #region Property Declarations

        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private IAddressRepository addressRepository = default(IAddressRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private INotificationRepository notificationRepository = default(INotificationRepository);
        private IPostCodeSectorRepository postcodeSectorRepository = default(IPostCodeSectorRepository);
        private IReferenceDataCategoryRepository referenceDataCategoryRepository = default(IReferenceDataCategoryRepository);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private bool enableLogging = false;

        #endregion Property Declarations

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="USRBusinessService"/> class.
        /// </summary>
        /// <param name="addressLocationRepository">The address location repository.</param>
        /// <param name="addressRepository">The address repository.</param>
        /// <param name="deliveryPointsRepository">The delivery points repository.</param>
        /// <param name="notificationRepository">The notification repository.</param>
        /// <param name="postcodeSectorRepository">The postcode sector repository.</param>
        /// <param name="referenceDataCategoryRepository">The reference data category repository.</param>
        /// <param name="configurationHelper">The configuration helper.</param>
        /// <param name="loggingHelper">The logging helper.</param>
        public USRBusinessService(
           IAddressLocationRepository addressLocationRepository,
           IAddressRepository addressRepository,
           IDeliveryPointsRepository deliveryPointsRepository,
           INotificationRepository notificationRepository,
           IPostCodeSectorRepository postcodeSectorRepository,
           IReferenceDataCategoryRepository referenceDataCategoryRepository,
           IConfigurationHelper configurationHelper,
           ILoggingHelper loggingHelper)
        {
            this.addressLocationRepository = addressLocationRepository;
            this.addressRepository = addressRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.notificationRepository = notificationRepository;
            this.postcodeSectorRepository = postcodeSectorRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.enableLogging = Convert.ToBoolean(configurationHelper.ReadAppSettingsConfigurationValues(Constants.EnableLogging));
        }

        #endregion Constructor

        #region Save USR Details to Database

        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="addressLocationUsrpostdtos"> List of Address Locations</param>
        /// <returns> Task </returns>
        public async Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos)
        {
            int fileUdprn;

            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted, Constants.COLON);

            foreach (AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO in addressLocationUsrpostdtos)
            {
                try
                {
                    // Get the udprn id for each USR record.
                    fileUdprn = (int)addressLocationUSRPOSTDTO.UDPRN;

                    if (!addressLocationRepository.AddressLocationExists(fileUdprn))
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
                        await addressLocationRepository.SaveNewAddressLocation(newAddressLocationDTO);

                        // Check if the delivery point exists
                        if (deliveryPointsRepository.DeliveryPointExists((int)fileUdprn))
                        {
                            DeliveryPointDTO deliveryPointDTO = deliveryPointsRepository.GetDeliveryPointByUDPRN((int)fileUdprn);

                            // Check if the existing delivery point has a location.
                            if (deliveryPointDTO.LocationXY == null)
                            {
                                // Get the Location provider Guid from the reference data table.
                                Guid locationProviderId = referenceDataCategoryRepository.GetReferenceDataId(Constants.NETWORKLINKDATAPROVIDER, Constants.EXTERNAL);

                                DeliveryPointDTO newDeliveryPoint = new DeliveryPointDTO
                                {
                                    Latitude = addressLocationUSRPOSTDTO.Latitude,
                                    Longitude = addressLocationUSRPOSTDTO.Longitude,
                                    LocationXY = spatialLocationXY,
                                    LocationProvider_GUID = locationProviderId,
                                    UDPRN = fileUdprn
                                };

                                // Update the location details for the delivery point
                                await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(newDeliveryPoint);

                                // Check if a notification exists for the UDPRN.
                                if (notificationRepository.CheckIfNotificationExists(fileUdprn, Constants.USRACTION))
                                {
                                    // Delete the notification if it exists.
                                    await notificationRepository.DeleteNotificationbyUDPRNAndAction(fileUdprn, Constants.USRACTION);
                                }
                            }
                            else
                            {
                                // Calculates the straight line distance between the existing delivery point and the new delivery point.
                                var straightLineDistance = deliveryPointsRepository.GetDeliveryPointDistance(deliveryPointDTO, spatialLocationXY);

                                // Check if the new point is within the tolerance limit
                                if (straightLineDistance < Constants.TOLERANCEDISTANCEINMETERS)
                                {
                                    Guid locationProviderId = referenceDataCategoryRepository.GetReferenceDataId(
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

                                    // Update the delivery point location directly in case it is within the tolerance limits.
                                    await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(newDeliveryPoint);

                                    // Check if the notification exists for the given UDPRN.
                                    if (notificationRepository.CheckIfNotificationExists(fileUdprn, Constants.USRACTION))
                                    {
                                        // Delete the notification if it exists.
                                        await notificationRepository.DeleteNotificationbyUDPRNAndAction(fileUdprn, Constants.USRACTION);
                                    }
                                }
                                else
                                {
                                    // Get the Postcode Sector by UDPRN
                                    PostCodeSectorDTO postCodeSectorDTO = postcodeSectorRepository.GetPostCodeSectorByUDPRN(fileUdprn);

                                    // Get the Notification Type from the Reference data table
                                    Guid notificationTypeId_GUID = referenceDataCategoryRepository.GetReferenceDataId(
                                                                                                                        Constants.USRCATEGORY,
                                                                                                                        Constants.USRREFERENCEDATANAME);

                                    PostalAddressDTO postalAddressDTO = addressRepository.GetPostalAddress(fileUdprn);

                                    NotificationDTO notificationDO = new NotificationDTO
                                    {
                                        ID = Guid.NewGuid(),
                                        NotificationType_GUID = notificationTypeId_GUID,
                                        NotificationDueDate = DateTime.UtcNow.AddHours(Constants.NOTIFICATIONDUE),
                                        NotificationSource = Constants.USRNOTIFICATIONSOURCE,
                                        Notification_Heading = Constants.USRACTION,
                                        Notification_Message = AddressFields(postalAddressDTO),
                                        PostcodeDistrict = (postCodeSectorDTO == null || postCodeSectorDTO.District == null) ? string.Empty : postCodeSectorDTO.District,
                                        PostcodeSector = (postCodeSectorDTO == null || postCodeSectorDTO.Sector == null) ? string.Empty : postCodeSectorDTO.Sector,
                                        NotificationActionLink = string.Format(Constants.USRNOTIFICATIONLINK, fileUdprn)
                                    };

                                    // Insert the new notification.
                                    await notificationRepository.AddNewNotification(notificationDO);
                                }
                            }
                        }
                    }

                    LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted, Constants.COLON);
                }
                catch (Exception ex)
                {
                    this.loggingHelper.LogError(ex);
                }
            }
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
            try
            {
                string sbLocationXY = string.Format(
                                                                            Constants.USRGEOMETRYPOINT,
                                                                            Convert.ToString(addressLocationUSRPOSTDTO.XCoordinate),
                                                                            Convert.ToString(addressLocationUSRPOSTDTO.YCoordinate));

                // Convert the location from string type to geometry type
                DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNGCOORDINATESYSTEM);
                return spatialLocationXY;
            }
            catch (Exception)
            {
                throw;
            }
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
            this.loggingHelper.LogInfo(methodName + separator + logMessage, this.enableLogging);
        }
    }
}