namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSPathNode")]
    public partial class OSPathNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkNode_Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public DbGeometry Location { get; set; }

        [StringLength(21)]
        public string formOfRoadNode { get; set; }

        [StringLength(19)]
        public string Classification { get; set; }

        [StringLength(32)]
        public string ReasonForChange { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }
    }
}
