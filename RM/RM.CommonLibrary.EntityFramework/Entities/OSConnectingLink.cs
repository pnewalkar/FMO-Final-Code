namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSConnectingLink")]
    public partial class OSConnectingLink
    {
        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public bool? Fictitious { get; set; }

        [Required]
        [StringLength(20)]
        public string ConnectingNodeTOID { get; set; }

        [Required]
        [StringLength(20)]
        public string PathNodeTOID { get; set; }

        [Required]
        public DbGeometry Geometry { get; set; }

        public Guid ID { get; set; }

        public Guid ConnectingNode_GUID { get; set; }

        public Guid? PathNode_GUID { get; set; }

        public virtual OSConnectingNode OSConnectingNode { get; set; }

        public virtual OSPathNode OSPathNode { get; set; }
    }
}
