namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.PostalAddress")]
    public partial class PostalAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostalAddress()
        {
            DeliveryPoints = new HashSet<DeliveryPoint>();
        }

        private Guid ID { get; set; }

        [Required]
        [StringLength(1)]
        private string PostcodeType { get; set; }

        [StringLength(60)]
        private string OrganisationName { get; set; }

        [StringLength(60)]
        private string DepartmentName { get; set; }

        [StringLength(50)]
        private string BuildingName { get; set; }

        private short? BuildingNumber { get; set; }

        [StringLength(50)]
        private string SubBuildingName { get; set; }

        [StringLength(80)]
        private string Thoroughfare { get; set; }

        [StringLength(80)]
        private string DependentThoroughfare { get; set; }

        [StringLength(35)]
        private string DependentLocality { get; set; }

        [StringLength(35)]
        private string DoubleDependentLocality { get; set; }

        [Required]
        [StringLength(30)]
        private string PostTown { get; set; }

        [Required]
        [StringLength(8)]
        private string Postcode { get; set; }

        [StringLength(2)]
        private string DeliveryPointSuffix { get; set; }

        [StringLength(1)]
        private string SmallUserOrganisationIndicator { get; set; }

        private int? UDPRN { get; set; }

        private bool? AMUApproved { get; set; }

        [StringLength(6)]
        private string POBoxNumber { get; set; }

        private Guid AddressType_GUID { get; set; }

        private DateTime RowCreateDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPoint> DeliveryPoints { get; set; }
    }
}