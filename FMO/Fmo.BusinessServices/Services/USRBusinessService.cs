﻿namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Constants;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DTO;
    using Fmo.DTO.FileProcessing;
    using Common.Interface;
    using System.Net.Mail;

    public class USRBusinessService : IUSRBusinessService
    {
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private INotificationRepository notificationRepository = default(INotificationRepository);
        private IPostCodeSectorRepository postCodeSectorRepository = default(IPostCodeSectorRepository);
        private IReferenceDataCategoryRepository referenceDataCategoryRepository = default(IReferenceDataCategoryRepository);
        private IEmailHelper emailHelper = default(IEmailHelper);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);

        public USRBusinessService(IAddressLocationRepository addressLocationRepository, IDeliveryPointsRepository deliveryPointsRepository, INotificationRepository notificationRepository, IPostCodeSectorRepository postCodeSectorRepository, IReferenceDataCategoryRepository referenceDataCategoryRepository, IEmailHelper emailHelper, IConfigurationHelper configurationHelper)
        {
            this.addressLocationRepository = addressLocationRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.notificationRepository = notificationRepository;
            this.postCodeSectorRepository = postCodeSectorRepository;
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
            this.emailHelper = emailHelper;
            this.configurationHelper = configurationHelper;
        }

        public async Task SaveUSRDetails(AddressLocationUSRPOSTDTO addressLocationUSRPOSTDTO)
        {
            int fileUdprn;
            try
            {
                fileUdprn = (int)addressLocationUSRPOSTDTO.udprn;

                if (addressLocationRepository.AddressLocationExists(fileUdprn))
                {
                    MailMessage message = new MailMessage()
                    {
                         From = new MailAddress(configurationHelper.ReadAppSettingsConfigurationValues("USRFromEmail")),
                         Subject = configurationHelper.ReadAppSettingsConfigurationValues("USRSubject"),
                         Body = string.Format(configurationHelper.ReadAppSettingsConfigurationValues("USRBody"), fileUdprn.ToString())
                    };

                    message.To.Add(configurationHelper.ReadAppSettingsConfigurationValues("USRToEmail"));

                    emailHelper.SendMessage(message);
                }
                else
                {
                    string sbLocationXY = string.Format(
                                                        Constants.USR_GEOMETRY_POINT,
                                                        Convert.ToString(addressLocationUSRPOSTDTO.xCoordinate),
                                                        Convert.ToString(addressLocationUSRPOSTDTO.yCoordinate));

                    DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNG_COORDINATE_SYSTEM);

                    AddressLocationDTO newAddressLocationDTO = new AddressLocationDTO()
                    {
                        UDPRN = addressLocationUSRPOSTDTO.udprn,
                        LocationXY = spatialLocationXY,
                        Lattitude = addressLocationUSRPOSTDTO.latitude,
                        Longitude = addressLocationUSRPOSTDTO.longitude
                    };

                    await addressLocationRepository.SaveNewAddressLocation(newAddressLocationDTO);

                    if (!deliveryPointsRepository.DeliveryPointExists((int)fileUdprn))
                    {

                        DeliveryPointDTO deliveryPointDTO = deliveryPointsRepository.GetDeliveryPointByUDPRN((int)fileUdprn);

                        if (deliveryPointDTO.LocationXY == null)
                        {
                            await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(fileUdprn, (decimal)addressLocationUSRPOSTDTO.latitude, (decimal)addressLocationUSRPOSTDTO.longitude, spatialLocationXY);
                            NotificationDTO notificationDTO = notificationRepository.GetNotificationByUDPRN(fileUdprn);
                            if (notificationDTO != null)
                            {
                               await notificationRepository.DeleteNotificationbyUDPRNAndAction(fileUdprn, Constants.USR_ACTION);
                            }
                        }
                        else
                        {
                           double straightLineDistance = (double)deliveryPointDTO.LocationXY.Distance(spatialLocationXY);
                            if (straightLineDistance < Constants.TOLERANCE_DISTANCE_IN_METERS)
                            {
                                await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(fileUdprn, (decimal)addressLocationUSRPOSTDTO.latitude, (decimal)addressLocationUSRPOSTDTO.longitude, spatialLocationXY);
                                NotificationDTO notificationDTO = notificationRepository.GetNotificationByUDPRN(fileUdprn);
                                if (notificationDTO != null)
                                {
                                    await notificationRepository.DeleteNotificationbyUDPRNAndAction(fileUdprn, Constants.USR_ACTION);
                                }
                            }
                            else
                            {
                                PostCodeSectorDTO postCodeSectorDTO = postCodeSectorRepository.GetPostCodeSectorByUDPRN(fileUdprn);
                                Guid notificationTypeId_GUID = referenceDataCategoryRepository.GetReferenceDataId(Constants.USR_CATEGORY, Constants.USR_REFERENCE_DATA_NAME);

                                NotificationDTO notificationDO = new NotificationDTO
                                {
                                    Notification_Id = fileUdprn,
                                    NotificationType_GUID = notificationTypeId_GUID,
                                    NotificationDueDate = DateTime.Now.AddHours(24),
                                    NotificationSource = Constants.USR_NOTIFICATION_SOURCE,
                                    Notification_Heading = Constants.USR_ACTION,
                                    Notification_Message = string.Format(Constants.USR_BODY, addressLocationUSRPOSTDTO.latitude.ToString(), addressLocationUSRPOSTDTO.longitude.ToString(), addressLocationUSRPOSTDTO.xCoordinate.ToString(), addressLocationUSRPOSTDTO.yCoordinate.ToString()),
                                    PostcodeDistrict = postCodeSectorDTO.District,
                                    PostcodeSector = postCodeSectorDTO.Sector,
                                };

                                await notificationRepository.AddNewNotification(notificationDO);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //return saveFlag;
            }
        }
    }
}
