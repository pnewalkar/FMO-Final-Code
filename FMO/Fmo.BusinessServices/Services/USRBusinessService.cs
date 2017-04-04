namespace Fmo.BusinessServices.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Spatial;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Constants;
    using DataServices.Repositories.Interfaces;
    using Fmo.BusinessServices.Interfaces;
    using Fmo.DTO;

    public class USRBusinessService : IUSRBusinessService
    {
        private IAddressLocationRepository addressLocationRepository = default(IAddressLocationRepository);
        private IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);


        public USRBusinessService(IAddressLocationRepository addressLocationRepository, IDeliveryPointsRepository deliveryPointsRepository)
        {
            this.addressLocationRepository = addressLocationRepository;
            this.deliveryPointsRepository = deliveryPointsRepository;
        }

        public bool SaveUSRDetails(AddressLocationUSRDTO addressLocationUSRDTO)
        {
            bool saveFlag = false;
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

                    DeliveryPointDTO deliveryPointDTO; //= deliveryPointsRepository.GetDeliveryPointByUDPRN((int)fileUdprn);

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

                    //if (deliveryPointDTO != null)
                    //{
                        
                    //}
                }
                return saveFlag;
            }

            catch (Exception ex)
            {
                return saveFlag;
            }
        }
    }
}
