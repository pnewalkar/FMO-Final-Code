namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSTurnRestriction")]
    public partial class OSTurnRestriction
    {
        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        [StringLength(34)]
        public string Restriction { get; set; }

        [StringLength(50)]
        public string inclusion { get; set; }

        [StringLength(50)]
        public string Exclusion { get; set; }

        [StringLength(50)]
        public string TimeInterval { get; set; }

        public Guid ID { get; set; }

        public Guid? NetworkReference_GUID { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}
