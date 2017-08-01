using System;

namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    public class PostalAddressAliasDTO
    {
        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public Guid AliasTypeGUID { get; set; }

        public Guid? PostalAddressIdentifierID { get; set; }

        public string AliasName { get; set; }

        public decimal? PreferenceOrderIndex { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}