namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSPathNode")]
    public partial class OSPathNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public OSPathNode()
        {
            OSConnectingLinks = new HashSet<OSConnectingLink>();
            OSPathLinks = new HashSet<OSPathLink>();
            OSPathLinks1 = new HashSet<OSPathLink>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public DbGeometry Location { get; set; }

        [StringLength(21)]
        public string FormOfRoadNode { get; set; }

        [StringLength(19)]
        public string Classification { get; set; }

        [StringLength(32)]
        public string ReasonForChange { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<OSConnectingLink> OSConnectingLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<OSPathLink> OSPathLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<OSPathLink> OSPathLinks1 { get; set; }
    }
}