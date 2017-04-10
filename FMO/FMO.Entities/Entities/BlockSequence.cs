namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.BlockSequence")]
    public partial class BlockSequence
    {
        public int BlockSequence_Id { get; set; }

        public int Block_Id { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OrderIndex { get; set; }

        public int? OperationalObject_Id { get; set; }

        public int? OperationalObjectType_Id { get; set; }

        public int? DeliveryGroup_Id { get; set; }

        public Guid ID { get; set; }

        public Guid Block_GUID { get; set; }

        public Guid? OperationalObjectType_GUID { get; set; }

        public Guid? DeliveryGroup_GUID { get; set; }

        public virtual Block Block { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}