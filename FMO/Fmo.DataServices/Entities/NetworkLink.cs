namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkLink")]
    public partial class NetworkLink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NetworkLink()
        {
            AccessLinks = new HashSet<AccessLink>();
            NetworkReferences = new HashSet<NetworkReference>();
            NetworkLinkReferences = new HashSet<NetworkLinkReference>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkLink_Id { get; set; }

        [Required]
        [StringLength(1)]
        public string NetworkLinkType { get; set; }

        [StringLength(20)]
        public string ExternalTOID { get; set; }

        [StringLength(1)]
        public string DataProvider { get; set; }

        public int? RoadName_Id { get; set; }

        public int? StreetName_Id { get; set; }

        public int StartNode_Id { get; set; }

        public int EndNode_Id { get; set; }

        [Required]
        public DbGeometry LinkGeometry { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LinkLength { get; set; }

        public int? LinkGradientType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        public virtual NetworkNode NetworkNode1 { get; set; }

        public virtual StreetName StreetName { get; set; }

        public virtual RoadName RoadName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkReference> NetworkReferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLinkReference> NetworkLinkReferences { get; set; }

        public virtual OSConnectingLink OSConnectingLink { get; set; }

        public virtual OSPathLink OSPathLink { get; set; }

        public virtual OSRoadLink OSRoadLink { get; set; }

        public virtual RMGLink RMGLink { get; set; }
    }
}
