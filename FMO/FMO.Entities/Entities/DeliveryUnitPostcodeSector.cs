namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.DeliveryUnitPostcodeSector")]
    public partial class DeliveryUnitPostcodeSector
    {
        public int DeliveryUnit_Id { get; set; }

        [Required]
        [StringLength(5)]
        public string PostcodeSector { get; set; }

        public int DeliveryUnitStatus_Id { get; set; }

        public Guid PostcodeSector_GUID { get; set; }

        public Guid DeliveryUnit_GUID { get; set; }

        public Guid DeliveryUnitStatus_GUID { get; set; }

        public Guid ID { get; set; }

        public virtual DeliveryUnitLocation DeliveryUnitLocation { get; set; }

        public virtual PostcodeSector PostcodeSector1 { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}