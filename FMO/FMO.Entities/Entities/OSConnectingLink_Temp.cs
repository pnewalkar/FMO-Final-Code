namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSConnectingLink_Temp")]
    public partial class OSConnectingLink_Temp
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string TOID { get; set; }

        public bool? Fictitious { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string ConnectingNodeTOID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string PathNodeTOID { get; set; }

        [Required]
        public DbGeometry Geometry { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid ID { get; set; }

        [Key]
        [Column(Order = 4)]
        public Guid ConnectingNodeGUID { get; set; }

        public Guid? PathNodeGUID { get; set; }
    }
}