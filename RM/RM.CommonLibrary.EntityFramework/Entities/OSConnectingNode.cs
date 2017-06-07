namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSConnectingNode")]
    public partial class OSConnectingNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OSConnectingNode()
        {
            OSConnectingLinks = new HashSet<OSConnectingLink>();
        }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        [Required]
        public DbGeometry Location { get; set; }

        [StringLength(20)]
        public string RoadLinkTOID { get; set; }

        public Guid ID { get; set; }

        public Guid? RoadLink_GUID { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OSConnectingLink> OSConnectingLinks { get; set; }

        public virtual OSRoadLink OSRoadLink { get; set; }
    }
}
