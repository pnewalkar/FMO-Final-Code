namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.UnitLocation")]
    public partial class UnitLocation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitLocation()
        {
            Scenarios = new HashSet<Scenario>();
            UnitLocationPostcodes = new HashSet<UnitLocationPostcode>();
            UnitPostcodeSectors = new HashSet<UnitPostcodeSector>();
            UserRoleUnits = new HashSet<UserRoleUnit>();
        }

        [Required]
        [StringLength(50)]
        public string ExternalId { get; set; }

        [StringLength(50)]
        public string UnitName { get; set; }

        public int UnitAddressUDPRN { get; set; }

        public DbGeometry UnitBoundryPolygon { get; set; }

        public Guid ID { get; set; }

        public Guid? LocationType_GUID { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenario> Scenarios { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitLocationPostcode> UnitLocationPostcodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitPostcodeSector> UnitPostcodeSectors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRoleUnit> UserRoleUnits { get; set; }
    }
}
