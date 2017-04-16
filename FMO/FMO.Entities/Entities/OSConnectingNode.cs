namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSConnectingNode")]
    public partial class OSConnectingNode
    {
        public int NetworkNode_Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry Location { get; set; }

        [StringLength(20)]
        public string RoadLinkTOID { get; set; }

        public Guid ID { get; set; }

        public Guid? RoadLink_GUID { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        public virtual OSRoadLink OSRoadLink { get; set; }
    }
}
