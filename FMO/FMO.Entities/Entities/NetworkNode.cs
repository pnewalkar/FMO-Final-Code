namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkNode")]
    public partial class NetworkNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NetworkNode()
        {
            NetworkLinks = new HashSet<NetworkLink>();
            NetworkLinks1 = new HashSet<NetworkLink>();
            NetworkReferences = new HashSet<NetworkReference>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NetworkNode_Id { get; set; }

        [StringLength(1)]
        public string NetworkNodeType { get; set; }

        [StringLength(1)]
        public string DataProvider { get; set; }

        [Required]
        public DbGeometry NodeGeometry { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLink> NetworkLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLink> NetworkLinks1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkReference> NetworkReferences { get; set; }

        public virtual OSConnectingNode OSConnectingNode { get; set; }

        public virtual OSPathNode OSPathNode { get; set; }

        public virtual OSRoadNode OSRoadNode { get; set; }

        public virtual RMGNode RMGNode { get; set; }
    }
}
