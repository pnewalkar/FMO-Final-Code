namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.OSRoadNode")]
    public partial class OSRoadNode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OSRoadNode()
        {
            OSRoadLinks = new HashSet<OSRoadLink>();
            OSRoadLinks1 = new HashSet<OSRoadLink>();
        }

        public int NetworkNode_Id { get; set; }

        [Required]
        [StringLength(20)]
        public string TOID { get; set; }

        public DateTime? validFrom { get; set; }

        public DbGeometry Location { get; set; }

        [StringLength(21)]
        public string formOfRoadNode { get; set; }

        [StringLength(19)]
        public string Classification { get; set; }

        [StringLength(5)]
        public string access { get; set; }

        [StringLength(120)]
        public string junctionName { get; set; }

        [StringLength(30)]
        public string JunctionNumber { get; set; }

        [StringLength(32)]
        public string ReasonForChange { get; set; }

        public Guid ID { get; set; }

        public virtual NetworkNode NetworkNode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OSRoadLink> OSRoadLinks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OSRoadLink> OSRoadLinks1 { get; set; }
    }
}