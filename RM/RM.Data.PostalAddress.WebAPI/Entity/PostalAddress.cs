namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.PostalAddress")]
    public partial class PostalAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public PostalAddress()
        {
            DeliveryPoints = new HashSet<DeliveryPoint>();
            PostalAddressStatus = new HashSet<PostalAddressStatus>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(1)]
        public string PostcodeType { get; set; }

        [StringLength(60)]
        public string OrganisationName { get; set; }

        [StringLength(60)]
        public string DepartmentName { get; set; }

        [StringLength(50)]
        public string BuildingName { get; set; }

        public short? BuildingNumber { get; set; }

        [StringLength(50)]
        public string SubBuildingName { get; set; }

        [StringLength(80)]
        public string Thoroughfare { get; set; }

        [StringLength(80)]
        public string DependentThoroughfare { get; set; }

        [StringLength(35)]
        public string DependentLocality { get; set; }

        [StringLength(35)]
        public string DoubleDependentLocality { get; set; }

        [Required]
        [StringLength(30)]
        public string PostTown { get; set; }

        [Required]
        [StringLength(8)]
        public string Postcode { get; set; }

        [StringLength(2)]
        public string DeliveryPointSuffix { get; set; }

        [StringLength(1)]
        public string SmallUserOrganisationIndicator { get; set; }

        public int? UDPRN { get; set; }

        public bool? AMUApproved { get; set; }

        [StringLength(6)]
        public string POBoxNumber { get; set; }

        public Guid AddressType_GUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<DeliveryPoint> DeliveryPoints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<PostalAddressStatus> PostalAddressStatus { get; set; }
    }
}