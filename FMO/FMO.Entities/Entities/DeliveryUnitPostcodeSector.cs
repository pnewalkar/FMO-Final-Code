namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryUnitPostcodeSector")]
    public partial class DeliveryUnitPostcodeSector
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeliveryUnit_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string DeliveryUnitStatus { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string PostcodeSector { get; set; }

        public virtual DeliveryUnitLocation DeliveryUnitLocation { get; set; }

        public virtual PostcodeSector PostcodeSector1 { get; set; }
    }
}
