namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSConnectingNode")]
    public partial class OSConnectingNode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkNode_Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry Location { get; set; }

        [Required]
        [StringLength(37)]
        public string RoadLinkTOID { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }
    }
}
