using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class AdvanceSearchDTO
    {
        public ICollection<DeliveryRouteDTO> DeliveryRoute { get; set; }

        public ICollection<DeliveryPointDTO> DeliveryPoint { get; set; }

        public ICollection<StreetNameDTO> StreetName { get; set; }

        public ICollection<PostCodeDTO> PostCode { get; set; }

        public ICollection<PostalAddressDTO> PostalAddress { get; set; }

        public ICollection<NetworkLinkDTO> NetworkLink { get; set; }


        ////public int TotalCount{
        ////    get
        ////    {
        ////        return DeliveryRoute.Count+
        ////    }
        ////    }

    }
}
