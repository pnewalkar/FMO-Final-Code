namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSTurnRestriction")]
    public partial class OSTurnRestriction
    {
        [Key]
        [StringLength(20)]
        public string TOID { get; set; }

        public int? NetworkReference_Id { get; set; }

        [StringLength(34)]
        public string Restriction { get; set; }

        [StringLength(50)]
        public string inclusion { get; set; }

        [StringLength(50)]
        public string Exclusion { get; set; }

        [StringLength(50)]
        public string TimeInterval { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}
