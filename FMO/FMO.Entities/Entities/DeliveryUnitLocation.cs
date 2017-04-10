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
            DeliveryUnitLocationPostcodes = new HashSet<DeliveryUnitLocationPostcode>();
            DeliveryUnitPostcodeSectors = new HashSet<DeliveryUnitPostcodeSector>();
            UserRoleUnits = new HashSet<UserRoleUnit>();
        }

        public int DeliveryUnit_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string UnitName { get; set; }

        public int UnitAddressUDPRN { get; set; }

        public DbGeometry UnitBoundryPolygon { get; set; }

        public Guid ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenario> Scenarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryUnitLocationPostcode> DeliveryUnitLocationPostcodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryUnitPostcodeSector> DeliveryUnitPostcodeSectors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRoleUnit> UserRoleUnits { get; set; }
    }
}