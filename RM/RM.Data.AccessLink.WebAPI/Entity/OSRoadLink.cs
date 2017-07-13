namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSRoadLink")]
    public partial class OSRoadLink
    {
        public Guid ID { get; private set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; private set; }

        [Required]
        public DbGeometry CentreLineGeometry { get; private set; }

        public bool? Ficticious { get; private set; }

        [StringLength(21)]
        public string RoadClassificaton { get; private set; }

        [StringLength(32)]
        public string RouteHierarchy { get; private set; }

        [StringLength(42)]
        public string FormOfWay { get; private set; }

        [MaxLength(1)]
        public byte[] TrunkRoad { get; private set; }

        [MaxLength(1)]
        public byte[] PrimaryRoute { get; private set; }

        [StringLength(10)]
        public string RoadClassificationNumber { get; private set; }

        [StringLength(255)]
        public string RoadName { get; private set; }

        [StringLength(255)]
        public string AlternateName { get; private set; }

        [StringLength(21)]
        public string Directionality { get; private set; }

        [Column(TypeName = "numeric")]
        public decimal LengthInMeters { get; private set; }

        [StringLength(20)]
        public string StartNodeTOID { get; private set; }

        [StringLength(20)]
        public string EndNodeTOID { get; private set; }

        public byte StartGradeSeparation { get; private set; }

        public byte EndGradeSeparation { get; private set; }

        [StringLength(19)]
        public string OperationalState { get; private set; }

        public Guid? StartNode_GUID { get; private set; }

        public Guid? EndNode_GUID { get; private set; }
    }
}
