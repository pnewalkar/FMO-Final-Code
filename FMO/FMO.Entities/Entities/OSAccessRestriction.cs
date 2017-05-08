namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSAccessRestriction")]
    public partial class OSAccessRestriction
    {
        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

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
