namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Postcode")]
    public partial class Postcode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Postcode()
        {
            AMUChangeRequests = new HashSet<AMUChangeRequest>();
            PostalAddresses = new HashSet<PostalAddress>();
        }

        [Key]
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AMUChangeRequest> AMUChangeRequests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddress> PostalAddresses { get; set; }

        public virtual PostcodeSector PostcodeSector { get; set; }
    }
}
