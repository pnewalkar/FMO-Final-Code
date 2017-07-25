namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.NetworkLink")]
    public partial class NetworkLink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public NetworkLink()
        {
            NetworkReferences = new HashSet<NetworkReference>();
            NetworkLinkReferences = new HashSet<NetworkLinkReference>();
        }

        public Guid ID { get; set; }

        [StringLength(20)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry LinkGeometry { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LinkLength { get; set; }

        public int? LinkGradientType { get; set; }

        public Guid? NetworkLinkTypeGUID { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public Guid? RoadNameGUID { get; set; }

        public Guid? StreetNameGUID { get; set; }

        public Guid? StartNodeID { get; set; }

        public Guid? EndNodeID { get; set; }

        [StringLength(255)]
        public string LinkName { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        public virtual NetworkNode NetworkNode1 { get; set; }

        public virtual StreetName StreetName { get; set; }

        public virtual RoadName RoadName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<NetworkReference> NetworkReferences { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<NetworkLinkReference> NetworkLinkReferences { get; set; }
    }
}