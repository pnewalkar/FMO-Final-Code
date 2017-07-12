namespace RM.Common.ActionManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.ReferenceData")]
    public partial class ReferenceData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReferenceData()
        {
            LocationReferenceDatas = new HashSet<LocationReferenceData>();
            PostalAddressIdentifiers = new HashSet<PostalAddressIdentifier>();
            ReferenceData1 = new HashSet<ReferenceData>();
        }

        public Guid ID { get; set; }

        [StringLength(1000)]
        public string ReferenceDataName { get; set; }

        [StringLength(1000)]
        public string ReferenceDataValue { get; set; }

        [StringLength(1000)]
        public string DataDescription { get; set; }

        [StringLength(1000)]
        public string DisplayText { get; set; }

        public Guid ReferenceDataCategoryID { get; set; }

        public Guid? ParentReferenceDataID { get; set; }

        public int? OrderingIndex { get; set; }

        public bool? Default { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationReferenceData> LocationReferenceDatas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddressIdentifier> PostalAddressIdentifiers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceData> ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }
    }
}
