namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSConnectingNode_temp")]
    public partial class OSConnectingNode_temp
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkNode_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry Location { get; set; }

        [StringLength(20)]
        public string RoadLinkTOID { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid ID { get; set; }

        public Guid? RoadLink_GUID { get; set; }
    }
}