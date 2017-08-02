namespace RM.DataManagement.NetworkManager.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.RoadName")]
    public partial class RoadName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public RoadName()
        {
            NetworkLinks = new HashSet<NetworkLink>();
        }

        public Guid ID { get; set; }

        [StringLength(20)]
        public string TOID { get; set; }

        [StringLength(10)]
        public string NationalRoadCode { get; set; }

        [StringLength(21)]
        public string RoadClassification { get; set; }

        [StringLength(255)]
        public string DesignatedName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<NetworkLink> NetworkLinks { get; set; }
    }
}