namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.RoadName")]
    public partial class RoadName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoadName()
        {
            NetworkLinks = new HashSet<NetworkLink>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoadName_Id { get; set; }

        [StringLength(20)]
        public string TOID { get; set; }

        [StringLength(10)]
        public string NationalRoadCode { get; set; }

        [StringLength(21)]
        public string roadClassification { get; set; }

        [StringLength(255)]
        public string DesignatedName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLink> NetworkLinks { get; set; }
    }
}
