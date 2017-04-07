namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.OSAccessRestriction")]
    public partial class OSAccessRestriction
    {
        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public int? NetworkReference_Id { get; set; }

        [StringLength(21)]
        public string RestrictionValue { get; set; }

        [Column(TypeName = "xml")]
        public string InclusionVehicleQualifier { get; set; }

        [Column(TypeName = "xml")]
        public string ExclusionVehicleQualifier { get; set; }

        [Column(TypeName = "xml")]
        public string TimeInterval { get; set; }

        public Guid ID { get; set; }

        public Guid? NetworkReference_GUID { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}