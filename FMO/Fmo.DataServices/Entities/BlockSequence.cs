namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.BlockSequence")]
    public partial class BlockSequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BlockSequence_Id { get; set; }

        public int Block_Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OrderIndex { get; set; }

        public int? OperationalObject_Id { get; set; }

        public int? OperationalObjectType_Id { get; set; }

        public int? DeliveryGroup_Id { get; set; }

        public virtual Block Block { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
