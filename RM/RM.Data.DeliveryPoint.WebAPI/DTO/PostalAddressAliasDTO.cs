using System;

namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    public class PostalAddressAliasDTO
    {
        public Guid ID { get; set; }

        public Guid DeliveryPoint_GUID { get; set; }

        public string DPAlias { get; set; }

        public bool Preferred { get; set; }
    }
}