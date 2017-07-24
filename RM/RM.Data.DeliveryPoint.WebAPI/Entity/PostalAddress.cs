namespace RM.Data.DeliveryPoint.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostalAddress")]
    public partial class PostalAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostalAddress()
        {
            DeliveryPoints = new HashSet<DeliveryPoint>();
        }

        public Guid ID { get; private set; }

        [Required]
        [StringLength(1)]
        public string PostcodeType { get; private set; }

        [StringLength(60)]
        public string OrganisationName { get; private set; }

        [StringLength(60)]
        public string DepartmentName { get; private set; }

        [StringLength(50)]
        public string BuildingName { get; private set; }

        public short? BuildingNumber { get; private set; }

        [StringLength(50)]
        public string SubBuildingName { get; private set; }

        [StringLength(80)]
        public string Thoroughfare { get; private set; }

        [StringLength(80)]
        public string DependentThoroughfare { get; private set; }

        [StringLength(35)]
        public string DependentLocality { get; private set; }

        [StringLength(35)]
        public string DoubleDependentLocality { get; private set; }

        [Required]
        [StringLength(30)]
        public string PostTown { get; private set; }

        [Required]
        [StringLength(8)]
        public string Postcode { get; private set; }

        [StringLength(2)]
        public string DeliveryPointSuffix { get; private set; }

        [StringLength(1)]
        public string SmallUserOrganisationIndicator { get; private set; }

        public int? UDPRN { get; private set; }

        public bool? AMUApproved { get; private set; }

        [StringLength(6)]
        public string POBoxNumber { get; private set; }

        public Guid AddressType_GUID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPoint> DeliveryPoints { get; private set; }
    }
}
