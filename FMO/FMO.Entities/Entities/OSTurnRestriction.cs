namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.OSTurnRestriction")]
    public partial class OSTurnRestriction
    {
        [Required]
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

        public Guid ID { get; set; }

        public Guid? NetworkReference_GUID { get; set; }

        public virtual NetworkReference NetworkReference { get; set; }
    }
}