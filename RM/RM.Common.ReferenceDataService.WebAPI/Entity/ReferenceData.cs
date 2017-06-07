namespace RM.Common.ReferenceData.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.ReferenceData")]
    public partial class ReferenceData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "This is an entity model.")]
        public ReferenceData()
        {
            ReferenceData1 = new HashSet<ReferenceData>();
        }

        [StringLength(100)]
        public string ReferenceDataName { get; set; }

        [StringLength(100)]
        public string ReferenceDataValue { get; set; }

        [StringLength(300)]
        public string DataDescription { get; set; }

        [StringLength(100)]
        public string DisplayText { get; set; }

        public Guid ID { get; set; }

        public Guid ReferenceDataCategory_GUID { get; set; }

        public Guid? DataParent_GUID { get; set; }

        public int? OrderingIndex { get; set; }

        public bool? Default { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "This is an entity model.")]
        public virtual ICollection<ReferenceData> ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }

        public virtual ReferenceDataCategory ReferenceDataCategory { get; set; }
    }
}