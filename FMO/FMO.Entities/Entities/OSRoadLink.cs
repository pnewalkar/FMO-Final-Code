namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSRoadLink")]
    public partial class OSRoadLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkLink_Id { get; set; }

        [Required]
        [StringLength(37)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry CentreLineGeometry { get; set; }

        public bool? Ficticious { get; set; }

        [StringLength(21)]
        public string RoadClassificaton { get; set; }

        [StringLength(32)]
        public string RouteHierarchy { get; set; }

        [StringLength(42)]
        public string FormOfWay { get; set; }

        [MaxLength(1)]
        public byte[] TrunkRoad { get; set; }

        [MaxLength(1)]
        public byte[] PrimaryRoute { get; set; }

        [StringLength(10)]
        public string RoadClassificationNumber { get; set; }

        [StringLength(255)]
        public string RoadName { get; set; }

        [StringLength(255)]
        public string AlternateName { get; set; }

        [StringLength(21)]
        public string Directionality { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LengthInMeters { get; set; }

        [StringLength(20)]
        public string StartNodeTOID { get; set; }

        [StringLength(20)]
        public string EndNodeTOID { get; set; }

        public byte StartGradeSeparation { get; set; }

        public byte EndGradeSeparation { get; set; }

        [StringLength(19)]
        public string OperationalState { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }
    }
}
