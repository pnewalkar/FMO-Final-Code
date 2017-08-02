namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSRoadLink")]
    public partial class OSRoadLink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public OSRoadLink()
        {
            NetworkLinkReferences = new HashSet<NetworkLinkReference>();
            OSConnectingNodes = new HashSet<OSConnectingNode>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
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

        public Guid? StartNode_GUID { get; set; }

        public Guid? EndNode_GUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<NetworkLinkReference> NetworkLinkReferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<OSConnectingNode> OSConnectingNodes { get; set; }

        public virtual OSRoadNode OSRoadNode { get; set; }

        public virtual OSRoadNode OSRoadNode1 { get; set; }
    }
}