namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    using System;

    public class PostcodeHierarchyDataDTO
    {
        public Guid ID { get; private set; }

        public string Postcode { get; private set; }

        public string ParentPostcode { get; private set; }

        public Guid PostcodeTypeGUID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }
    }
}