using System;
using System.Collections.Generic;

namespace RM.DataManagement.PostalAddress.WebAPI.DTO
{
    public class PostalAddressDBDTO
    {
        public PostalAddressDBDTO()
        {
            this.PostalAddressStatus = new List<PostalAddressStatusDBDTO>();
        }
        /// <summary>
        /// This class represents DB data transfer object for PostalAddress entity
        /// </summary>

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

        public virtual ICollection<DeliveryPointDBDTO> DeliveryPoints { get; set; }

        public virtual ICollection<PostalAddressStatusDBDTO> PostalAddressStatus { get; set; }

    }
}
