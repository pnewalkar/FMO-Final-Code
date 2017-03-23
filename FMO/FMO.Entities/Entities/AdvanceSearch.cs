using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMO.Entities
{
    public partial class AdvanceSearch
    {

        public virtual ICollection<DeliveryRoute> DeliveryRoute { get; set; }

        public virtual ICollection<DeliveryPoint> DeliveryPoint { get; set; }

        public virtual ICollection<StreetName> StreetName { get; set; }

        public virtual ICollection<Postcode> PostCode { get; set; }

        public virtual ICollection<PostalAddress> PostalAddress { get; set; }

        public virtual ICollection<NetworkLink> NetworkLink { get; set; }

    }
}
