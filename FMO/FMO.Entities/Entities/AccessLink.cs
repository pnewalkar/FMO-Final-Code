namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AccessLink")]
    public partial class AccessLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccessLink_Id { get; set; }

        [Required]
        public DbGeometry OperationalObjectPoint { get; set; }

        [Required]
        public DbGeometry NetworkIntersectionPoint { get; set; }

        [Required]
        public DbGeometry AccessLinkLine { get; set; }

        public int AccessLinkType_Id { get; set; }

        public bool? Approved { get; set; }

        [Column(TypeName = "numeric")]
        public decimal ActualLengthMeter { get; set; }

        [Column(TypeName = "numeric")]
        public decimal WorkloadLengthMeter { get; set; }

        public int LinkStatus_Id { get; set; }

        public int? NetworkLink_Id { get; set; }

        public int LinkDirection_Id { get; set; }

        public int OperationalObjectId { get; set; }

        public int OperationalObjectType_Id { get; set; }

        public Guid ID { get; set; }

        public Guid? LinkStatus_GUID { get; set; }

        public Guid? AccessLinkType_GUID { get; set; }

        public Guid? LinkDirection_GUID { get; set; }

        public Guid? OperationalObject_GUID { get; set; }

        public Guid? OperationalObjectType_GUID { get; set; }

        public Guid NetworkLink_GUID { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }

        public virtual ReferenceData ReferenceData3 { get; set; }

        public virtual RMGDeliveryPoint RMGDeliveryPoint { get; set; }
    }
}