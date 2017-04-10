namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.ReferenceDataCategory")]
    public partial class ReferenceDataCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceDataCategory()
        {
            ReferenceDatas = new HashSet<ReferenceData>();
        }

        public int ReferenceDataCategory_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public bool Maintainable { get; set; }

        public int CategoryType { get; set; }

        public Guid ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceData> ReferenceDatas { get; set; }
    }
}