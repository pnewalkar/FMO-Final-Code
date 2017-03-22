using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class AdvanceSearch
    {
        public ICollection<DeliveryRoute> DeliveryRoute { get; set; }

        public ICollection<DeliveryPoint> DeliveryPoint { get; set; }

        public ICollection<StreetName> StreetName { get; set; }

        public ICollection<PostCode> PostCode { get; set; }

        public ICollection<PostalAddress> PostalAddress { get; set; }

        public ICollection<NetworkLink> NetworkLink { get; set; }

    }
}
