namespace Fmo.BusinessServices.Services
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

    public class USRBusinessService : IUSRBusinessService
    {
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        private INotificationRepository notificationRepository = default(INotificationRepository);


        public USRBusinessService(IAddressLocationRepository addressLocationRepository, IDeliveryPointsRepository deliveryPointsRepository, INotificationRepository notificationRepository)
        {
            this.addressLocationRepository = addressLocationRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task SaveUSRDetails(AddressLocationUSRDTO addressLocationUSRDTO)
        {
            //bool saveFlag = false;
            int fileUdprn;
            try
            {
                fileUdprn = (int)addressLocationUSRDTO.udprn;

                AddressLocationDTO addressLocationDTO = addressLocationRepository.GetAddressLocationByUDPRN(fileUdprn);

                if (addressLocationDTO != null)
                {
                }
                else
                {
                    DeliveryPointDTO deliveryPointDTO = deliveryPointsRepository.GetDeliveryPointByUDPRN((int)fileUdprn);

                    StringBuilder sbLocationXY = new StringBuilder();
                    sbLocationXY.Append("POINT");
                    sbLocationXY.Append("(");
                    sbLocationXY.Append(Convert.ToString(addressLocationUSRDTO.xCoordinate));
                    sbLocationXY.Append(",");
                    sbLocationXY.Append(Convert.ToString(addressLocationUSRDTO.yCoordinate));
                    sbLocationXY.Append(")");

                    DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), Constants.BNG_COORDINATE_SYSTEM);

                    AddressLocationDTO newAddressLocationDTO = new AddressLocationDTO()
                    {
                        UDPRN = addressLocationUSRDTO.udprn,
                        LocationXY = spatialLocationXY,
                        Latitude = addressLocationUSRDTO.latitude,
                        Longitude = addressLocationUSRDTO.longitude
                    };

                    addressLocationRepository.SaveNewAddressLocation(newAddressLocationDTO);

                    if (deliveryPointDTO != null)
                    {
                        if (deliveryPointDTO.LocationXY == null)
                        {
                            await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(fileUdprn, (decimal)addressLocationUSRDTO.latitude, (decimal)addressLocationUSRDTO.longitude, spatialLocationXY);
                            NotificationDTO notificationDTO = notificationRepository.GetNotificationByUDPRN(fileUdprn);
                            if (notificationDTO != null)
                            {
                               await notificationRepository.DeleteNotificationbyUDPRN(fileUdprn);
                            }
                        }
                        else
                        {
                           double straightLineDistance = (double)deliveryPointDTO.LocationXY.Distance(spatialLocationXY);
                            if (straightLineDistance < Constants.TOLERANCE_DISTANCE_IN_METERS)
                            {
                                await deliveryPointsRepository.UpdateDeliveryPointLocationOnUDPRN(fileUdprn, (decimal)addressLocationUSRDTO.latitude, (decimal)addressLocationUSRDTO.longitude, spatialLocationXY);
                                NotificationDTO notificationDTO = notificationRepository.GetNotificationByUDPRN(fileUdprn);
                                if (notificationDTO != null)
                                {
                                    await notificationRepository.DeleteNotificationbyUDPRN(fileUdprn);
                                }
                            }
                            else
                            {
                                NotificationDTO notificationDO = new NotificationDTO
                                {
                                    NotificationDueDate = DateTime.Now,
                                    NotificationSource = Constants.USR_NOTIFICATION_SOURCE,
                                    Notification_Heading = Constants.USR_ACTION
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
