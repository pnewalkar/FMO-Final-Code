namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    using System;
    using System.Collections.Generic;

    public class PostalAddressDataDTO
    {
        public Guid ID { get; set; }

        public string PostcodeType { get; set; }

        public string OrganisationName { get; set; }

        public string DepartmentName { get; set; }

        public string BuildingName { get; set; }

        public short? BuildingNumber { get; set; }

        public string SubBuildingName { get; set; }

        public string Thoroughfare { get; set; }

        public string DependentThoroughfare { get; set; }

        public string DependentLocality { get; set; }

        public string DoubleDependentLocality { get; set; }

        public string PostTown { get; set; }

        public string Postcode { get; set; }

        public string DeliveryPointSuffix { get; set; }

        public string SmallUserOrganisationIndicator { get; set; }

        public int? UDPRN { get; set; }

        public bool? AMUApproved { get; set; }

        public string POBoxNumber { get; set; }

        public Guid AddressType_GUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public List<DeliveryPointDataDTO> DeliveryPoints { get; set; }
    }
}