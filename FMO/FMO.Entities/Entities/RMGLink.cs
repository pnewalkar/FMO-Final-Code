namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.RMGLink")]
    public partial class RMGLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkLink_Id { get; set; }

        [StringLength(1)]
        public string LinkType { get; set; }

        [StringLength(20)]
        public string StartNodeReference { get; set; }

        [StringLength(2)]
        public string StartNodeType { get; set; }

        [StringLength(2)]
        public string EndNodeType { get; set; }

        [StringLength(20)]
        public string EndNodeReference { get; set; }

        public DbGeometry Geometry { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }
    }
}
