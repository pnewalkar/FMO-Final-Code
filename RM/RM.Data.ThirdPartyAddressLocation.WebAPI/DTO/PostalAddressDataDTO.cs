namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class PostalAddressDataDTO
    {

        public Guid ID { get; private set; }

        public string PostcodeType { get; private set; }

        public string OrganisationName { get; private set; }

        public string DepartmentName { get; private set; }
                
        public string BuildingName { get; private set; }

        public short? BuildingNumber { get; private set; }
                
        public string SubBuildingName { get; private set; }
                
        public string Thoroughfare { get; private set; }
                
        public string DependentThoroughfare { get; private set; }

        public string DependentLocality { get; private set; }

        public string DoubleDependentLocality { get; private set; }
                
        public string PostTown { get; private set; }
                
        public string Postcode { get; private set; }
        
        public string DeliveryPointSuffix { get; private set; }

        public string SmallUserOrganisationIndicator { get; private set; }

        public int? UDPRN { get; private set; }

        public bool? AMUApproved { get; private set; }
              
        public string POBoxNumber { get; private set; }

        public Guid AddressType_GUID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }
                
        public List<DeliveryPointDataDTO> DeliveryPoints { get; private set; }
    }
}
