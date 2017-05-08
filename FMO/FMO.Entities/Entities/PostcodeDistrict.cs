namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostcodeDistrict")]
    public partial class PostcodeDistrict
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostcodeDistrict()
        {
            PostcodeSectors = new HashSet<PostcodeSector>();
        }

        [Required]
        [StringLength(4)]
        public string District { get; set; }

        [StringLength(2)]
        public string Area { get; set; }

        public Guid ID { get; set; }

        public Guid AreaGUID { get; set; }

        public virtual PostcodeArea PostcodeArea { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostcodeSector> PostcodeSectors { get; set; }
    }
}
