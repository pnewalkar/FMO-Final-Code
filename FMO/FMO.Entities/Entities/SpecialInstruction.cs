namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.SpecialInstruction")]
    public partial class SpecialInstruction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SpecialInstruction_Id { get; set; }

        public int? InstructionType_Id { get; set; }

        [StringLength(300)]
        public string InstructionText { get; set; }

        public int OperationalObject_Id { get; set; }

        public int OperationalObjectType_Id { get; set; }

        public short? DaysofTheWeek { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? EndDate { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}