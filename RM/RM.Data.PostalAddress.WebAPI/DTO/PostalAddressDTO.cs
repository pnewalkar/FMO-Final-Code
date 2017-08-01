using System;
using System.Collections.Generic;

namespace RM.DataManagement.PostalAddress.WebAPI.DTO
{
    public class PostalAddressDTO
    {
        /// <summary>
        /// This class represents data transfer object for PostalAddress entity
        /// </summary>
        public PostalAddressDTO()
        {
            this.DeliveryPoints = new List<DeliveryPointDTO>();
            this.PostalAddressAlias = new HashSet<PostalAddressAliasDTO>();
        }

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

        public Guid ID { get; set; }

        public Guid PostCodeGUID { get; set; }

        public Guid AddressType_GUID { get; set; }

        public Guid? AddressStatus_GUID { get; set; }

        public bool IsValidData { get; set; }

        public string InValidRemarks { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string AmendmentType { get; set; }

        public string AmendmentDesc { get; set; }

        public string FileName { get; set; }

        public ICollection<DeliveryPointDTO> DeliveryPoints { get; set; }

        public ICollection<PostalAddressAliasDTO> PostalAddressAlias { get; set; }
    }
}