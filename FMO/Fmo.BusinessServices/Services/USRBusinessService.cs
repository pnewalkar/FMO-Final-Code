namespace Fmo.BusinessServices.Services
{
    using Common.Constants;
    using Common.Interface;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DTO;
    using Fmo.DTO.FileProcessing;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Net.Mail;
    using System.Threading.Tasks;

    /// <summary>
    /// Implements interface for USR Business service
    /// </summary>
    public class USRBusinessService : IUSRBusinessService
    {
        #region Property Declarations

        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private INotificationRepository notificationRepository = default(INotificationRepository);
        private IPostCodeSectorRepository postCodeSectorRepository = default(IPostCodeSectorRepository);
        private IReferenceDataCategoryRepository referenceDataCategoryRepository = default(IReferenceDataCategoryRepository);
        private IEmailHelper emailHelper = default(IEmailHelper);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        #endregion

        #region Constructor

        public USRBusinessService(
           IAddressLocationRepository addressLocationRepository,
           IDeliveryPointsRepository deliveryPointsRepository,
           INotificationRepository notificationRepository,
           IPostCodeSectorRepository postCodeSectorRepository,
           IReferenceDataCategoryRepository referenceDataCategoryRepository,
           IEmailHelper emailHelper,
           IConfigurationHelper configurationHelper,
           ILoggingHelper loggingHelper)
        {
            this.addressLocationRepository = addressLocationRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.notificationRepository = notificationRepository;
            this.postCodeSectorRepository = postCodeSectorRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.emailHelper = emailHelper;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
        }

        #endregion

        #region Save USR Details to Database

        /// <summary>
        /// Method to save the list of USR data into the database.
        /// </summary>
        /// <param name="lstAddressLocationUSRPOSTDTO"> List of Address Locations</param>
        /// <returns> Task </returns>
        public async Task SaveUSRDetails(List<AddressLocationUSRPOSTDTO> lstAddressLocationUSRPOSTDTO)
        {
            int fileUdprn;
            try
            {
                loggingHelper.LogInfo("******SaveUSRDetails method execution started*****");
                foreach (AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO in lstAddressLocationUSRPOSTDTO)
                {
                    // Get the udprn id for each USR record.
                    fileUdprn = (int)addressLocationUSRPOSTDTO.udprn;

                    if (addressLocationRepository.AddressLocationExists(fileUdprn))
                    {
                        SendEmail(fileUdprn);
                    }
                    else
                    {
                        DbGeometry spatialLocationXY = GetSpatialLocation(addressLocationUSRPOSTDTO);

                        AddressLocationDTO newAddressLocationDTO = new AddressLocationDTO()
                        {
                            UDPRN = addressLocationUSRPOSTDTO.udprn,
                            LocationXY = spatialLocationXY,
                            Lattitude = addressLocationUSRPOSTDTO.latitude,
                            Longitude = addressLocationUSRPOSTDTO.longitude
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
                                    Latitude = addressLocationUSRPOSTDTO.latitude,
                                    Longitude = addressLocationUSRPOSTDTO.longitude,
                                    LocationXY = spatialLocationXY,
                                    LocationProvider_GUID = locationProviderId
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
                                        Latitude = addressLocationUSRPOSTDTO.latitude,
                                        Longitude = addressLocationUSRPOSTDTO.longitude,
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
                                    PostCodeSectorDTO postCodeSectorDTO = postCodeSectorRepository.GetPostCodeSectorByUDPRN(fileUdprn);

                                    // Get the Notification Type from the Reference data table
                                    Guid notificationTypeId_GUID = referenceDataCategoryRepository.GetReferenceDataId(
                                                                                                                        Constants.USRCATEGORY,
                                                                                                                        Constants.USRREFERENCEDATANAME);

                                    NotificationDTO notificationDO = new NotificationDTO
                                    {
                                        Notification_Id = fileUdprn,
                                        NotificationType_GUID = notificationTypeId_GUID,
                                        NotificationDueDate = DateTime.Now.AddHours(Constants.USRNOTIFICATIONDUE),
                                        NotificationSource = Constants.USRNOTIFICATIONSOURCE,
                                        Notification_Heading = Constants.USRACTION,
                                        Notification_Message = string.Format(
                                                                                Constants.USRBODY,
                                                                                addressLocationUSRPOSTDTO.latitude.ToString(),
                                                                                addressLocationUSRPOSTDTO.longitude.ToString(),
                                                                                addressLocationUSRPOSTDTO.xCoordinate.ToString(),
                                                                                addressLocationUSRPOSTDTO.yCoordinate.ToString()),
                                        PostcodeDistrict = (postCodeSectorDTO == null || postCodeSectorDTO.District == null) ? string.Empty : postCodeSectorDTO.District,
                                        PostcodeSector = (postCodeSectorDTO == null || postCodeSectorDTO.Sector == null) ? string.Empty : postCodeSectorDTO.Sector,
                                    };

                                    // Insert the new notification.
                                    await notificationRepository.AddNewNotification(notificationDO);
                                }
                            }
                        }
                    }
                }

                loggingHelper.LogInfo("******SaveUSRDetails method execution finished*****");
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
            }
        }

        #endregion

        #region Calculate Spatial Location

        /// <summary>
        /// Get the geometry equivalent of the X-Y co-ordinate of address location
        /// </summary>
        /// <param name="addressLocationUSRPOSTDTO">AddressLocationUSRPOSTDTO object</param>
        /// <returns>DbGeometry object</returns>
        private DbGeometry GetSpatialLocation(AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO)
        {
            try
            {
                loggingHelper.LogInfo("******GetSpatialLocation method execution started*****");
                string sbLocationXY = string.Format(
                                                                            Constants.USRGEOMETRYPOINT,
                                                                            Convert.ToString(addressLocationUSRPOSTDTO.xCoordinate),
                                                                            Convert.ToString(addressLocationUSRPOSTDTO.yCoordinate));

                // Convert the location from string type to geometry type
                DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNGCOORDINATESYSTEM);
                loggingHelper.LogInfo("******GetSpatialLocation method execution finished*****");
                return spatialLocationXY;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Send Email

        /// <summary>
        /// Send error e-mail to third party in case the UDPRN address location already exists
        /// </summary>
        /// <param name="fileUdprn">UDPRN id from file</param>
        private void SendEmail(int fileUdprn)
        {
            try
            {
                loggingHelper.LogInfo("******SendEmail method execution started*****");
                MailMessage message = new MailMessage()
                {
                    From = new MailAddress(configurationHelper.ReadAppSettingsConfigurationValues(Constants.USREMAILFROMEMAIL)),
                    Subject = configurationHelper.ReadAppSettingsConfigurationValues(Constants.USREMAILSUBJECT),
                    Body = string.Format(configurationHelper.ReadAppSettingsConfigurationValues(Constants.USREMAILBODY), fileUdprn.ToString())
                };

                message.To.Add(configurationHelper.ReadAppSettingsConfigurationValues(Constants.USREMAILTOEMAIL));

                // Send email if the address location udprn already exists in the database
                emailHelper.SendMessage(message);
                loggingHelper.LogInfo("******SendEmail method execution finished*****");
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}