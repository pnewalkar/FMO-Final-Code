namespace RM.Common.ReferenceData.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Reference Data Category Database Entity
    /// </summary>
    [Table("FMO.ReferenceDataCategory")]
    public partial class ReferenceDataCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "This is an entity model.")]
        public ReferenceDataCategory()
        {
            ReferenceDatas = new HashSet<ReferenceData>();
        }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public bool Maintainable { get; set; }

        public int CategoryType { get; set; }

        public Guid ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "This is an entity model.")]
        public virtual ICollection<ReferenceData> ReferenceDatas { get; set; }
    }
}