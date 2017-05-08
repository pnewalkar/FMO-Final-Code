namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostcodeSector")]
    public partial class PostcodeSector
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostcodeSector()
        {
            Postcodes = new HashSet<Postcode>();
            UnitPostcodeSectors = new HashSet<UnitPostcodeSector>();
        }

        [Required]
        [StringLength(5)]
        public string Sector { get; set; }

        [StringLength(4)]
        public string District { get; set; }

        public Guid ID { get; set; }

        public Guid DistrictGUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Postcode> Postcodes { get; set; }

        public virtual PostcodeDistrict PostcodeDistrict { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitPostcodeSector> UnitPostcodeSectors { get; set; }
    }
}
