using System;
using System.Collections.Generic;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class PostalAddressDataDTO
    {
        public PostalAddressDataDTO()
        {
            DeliveryPoints = new HashSet<DeliveryPointDataDTO>();
        }

        private Guid ID { get; set; }

        private string PostcodeType { get; set; }

        private string OrganisationName { get; set; }

        private string DepartmentName { get; set; }

        private string BuildingName { get; set; }

        private short? BuildingNumber { get; set; }

        private string SubBuildingName { get; set; }

        private string Thoroughfare { get; set; }

        private string DependentThoroughfare { get; set; }

        private string DependentLocality { get; set; }

        private string DoubleDependentLocality { get; set; }

        private string PostTown { get; set; }

        private string Postcode { get; set; }

        private string DeliveryPointSuffix { get; set; }

        private string SmallUserOrganisationIndicator { get; set; }

        private int? UDPRN { get; set; }

        private bool? AMUApproved { get; set; }

        private string POBoxNumber { get; set; }

        private Guid AddressType_GUID { get; set; }

        private DateTime RowCreateDateTime { get; set; }

        public virtual ICollection<DeliveryPointDataDTO> DeliveryPoints { get; set; }
    }
}