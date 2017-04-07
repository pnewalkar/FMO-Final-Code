namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.StreetName")]
    public partial class StreetName
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StreetName()
        {
            NetworkLinks = new HashSet<NetworkLink>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StreetName_Id { get; set; }

        [StringLength(12)]
        public string USRN { get; set; }

        [StringLength(10)]
        public string NationalRoadCode { get; set; }

        [StringLength(120)]
        public string DesignatedName { get; set; }

        [StringLength(120)]
        public string LocalName { get; set; }

        [StringLength(120)]
        public string Descriptor { get; set; }

        [StringLength(21)]
        public string RoadClassification { get; set; }

        [StringLength(35)]
        public string StreetType { get; set; }

        public DbGeometry Geometry { get; set; }

        [Required]
        [StringLength(1)]
        public string StreetNameProvider { get; set; }

        [StringLength(35)]
        public string OperationalState { get; set; }

        [StringLength(120)]
        public string OperationalStateReason { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OperationalStateStartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? OperationalStateEndTime { get; set; }

        [StringLength(35)]
        public string Locality { get; set; }

        [StringLength(30)]
        public string Town { get; set; }

        [StringLength(30)]
        public string AdministrativeArea { get; set; }

        public Guid ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NetworkLink> NetworkLinks { get; set; }
    }
}