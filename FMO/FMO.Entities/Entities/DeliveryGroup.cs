namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryGroup")]
    public partial class DeliveryGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryGroup()
        {
            BlockSequences = new HashSet<BlockSequence>();
            DeliveryPoints = new HashSet<DeliveryPoint>();
            DeliveryRouteActivities = new HashSet<DeliveryRouteActivity>();
            GroupHazards = new HashSet<GroupHazard>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeliveryGroup_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int GroupType_Id { get; set; }

        public byte? NumberOfFloors { get; set; }

        public double? InternalDistanceMeters { get; set; }

        public double? WorkloadOverrideTimeMinutes { get; set; }

        [StringLength(300)]
        public string WorkloadOverrideReason { get; set; }

        public bool? OverrideApproved { get; set; }

        public bool? ServicePoint { get; set; }

        public int? ServicePointType_Id { get; set; }

        public int Polygon_Id { get; set; }

        public Guid ID { get; set; }

        public Guid GroupType_GUID { get; set; }

        public Guid? ServicePointType_GUID { get; set; }

        public Guid Polygon_GUID { get; set; }

        public virtual AccessLink AccessLink { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockSequence> BlockSequences { get; set; }

        public virtual Polygon Polygon { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryPoint> DeliveryPoints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteActivity> DeliveryRouteActivities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupHazard> GroupHazards { get; set; }
    }
}