namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.BlockSequence")]
    public partial class BlockSequence
    {
        [Column(TypeName = "numeric")]
        public decimal? OrderIndex { get; set; }

        public int? OperationalObject_Id { get; set; }

        public Guid ID { get; set; }

        public Guid Block_GUID { get; set; }

        public Guid? OperationalObjectType_GUID { get; set; }

        public Guid? DeliveryGroup_GUID { get; set; }

        public Guid? OperationalObject_GUID { get; set; }

        public virtual Block Block { get; set; }

        public virtual DeliveryGroup DeliveryGroup { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
