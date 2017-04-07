namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.Postcode")]
    public partial class Postcode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Postcode()
        {
            AMUChangeRequests = new HashSet<AMUChangeRequest>();
            DeliveryUnitLocationPostcodes = new HashSet<DeliveryUnitLocationPostcode>();
            PostalAddresses = new HashSet<PostalAddress>();
        }

        [Required]
        [StringLength(8)]
        public string PostcodeUnit { get; set; }

        [Required]
        [StringLength(4)]
        public string OutwardCode { get; set; }

        [Required]
        [StringLength(3)]
        public string InwardCode { get; set; }

        [Required]
        [StringLength(5)]
        public string Sector { get; set; }

        public Guid ID { get; set; }

        public Guid SectorGUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AMUChangeRequest> AMUChangeRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryUnitLocationPostcode> DeliveryUnitLocationPostcodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddress> PostalAddresses { get; set; }

        public virtual PostcodeSector PostcodeSector { get; set; }
    }
}