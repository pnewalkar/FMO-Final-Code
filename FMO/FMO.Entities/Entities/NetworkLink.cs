namespace Fmo.Entities
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
            DeliveryRouteNetworkLinks = new HashSet<DeliveryRouteNetworkLink>();
            NetworkReferences = new HashSet<NetworkReference>();
            NetworkLinkReferences = new HashSet<NetworkLinkReference>();
        }

        public Guid Id { get; set; }

        [StringLength(20)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry LinkGeometry { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LinkLength { get; set; }

        public int? LinkGradientType { get; set; }

        public Guid? NetworkLinkType_GUID { get; set; }

        public Guid? DataProvider_GUID { get; set; }

        public Guid? RoadName_GUID { get; set; }

        public Guid? StreetName_GUID { get; set; }

        public Guid? StartNode_GUID { get; set; }

        public Guid? EndNode_GUID { get; set; }

        [StringLength(255)]
        public string LinkName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLink> AccessLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRouteNetworkLink> DeliveryRouteNetworkLinks { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        public virtual NetworkNode NetworkNode1 { get; set; }

        public virtual StreetName StreetName { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual RoadName RoadName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkReference> NetworkReferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLinkReference> NetworkLinkReferences { get; set; }
    }
}
