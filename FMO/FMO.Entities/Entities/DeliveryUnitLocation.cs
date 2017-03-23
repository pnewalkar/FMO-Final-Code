namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.DeliveryUnitLocation")]
    public partial class DeliveryUnitLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryUnitLocation()
        {
            Scenarios = new HashSet<Scenario>();
            DeliveryUnitPostcodeSectors = new HashSet<DeliveryUnitPostcodeSector>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeliveryUnit_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string UnitName { get; set; }

        public int UnitAddressUDPRN { get; set; }

        public DbGeometry UnitBoundryPolygon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenario> Scenarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryUnitPostcodeSector> DeliveryUnitPostcodeSectors { get; set; }
    }
}
