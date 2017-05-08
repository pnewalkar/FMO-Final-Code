namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.SpecialInstruction")]
    public partial class SpecialInstruction
    {
        [StringLength(300)]
        public string InstructionText { get; set; }

        public int OperationalObject_Id { get; set; }

        public short? DaysofTheWeek { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        public Guid ID { get; set; }

        public Guid OperationalObjectType_GUID { get; set; }

        public Guid? InstructionType_GUID { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}
