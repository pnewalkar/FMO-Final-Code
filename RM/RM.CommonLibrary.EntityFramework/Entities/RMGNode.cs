namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.RMGNode")]
    public partial class RMGNode
    {
        public DbGeometry Location { get; set; }

        public long? FormofNode { get; set; }

        public bool? NodeOnLink { get; set; }

        [StringLength(20)]
        public string OSLinkReference { get; set; }

        [StringLength(1)]
        public string OSLinkType { get; set; }

        public Guid ID { get; set; }
    }
}
