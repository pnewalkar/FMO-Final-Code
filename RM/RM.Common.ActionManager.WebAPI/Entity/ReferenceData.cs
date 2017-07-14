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
        public string ReferenceDataName { get; private set; }

        [StringLength(1000)]
        public string ReferenceDataValue { get; set; }

        [StringLength(1000)]
        public string DataDescription { get; private set; }

        [StringLength(1000)]
        public string DisplayText { get; private set; }

        public Guid ReferenceDataCategoryID { get; private set; }

        public Guid? ParentReferenceDataID { get; private set; }

        public int? OrderingIndex { get; private set; }

        public bool? Default { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationReferenceData> LocationReferenceDatas { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PostalAddressIdentifier> PostalAddressIdentifiers { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReferenceData> ReferenceData1 { get; private set; }

        public virtual ReferenceData ReferenceData2 { get; private set; }
    }
}
