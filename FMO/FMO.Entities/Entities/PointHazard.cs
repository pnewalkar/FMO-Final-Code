namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PointHazard")]
    public partial class PointHazard
    {
        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        public Guid ID { get; set; }

        public Guid Category_GUID { get; set; }

        public Guid SubCategory_GUID { get; set; }

        public Guid OperationalObjectType_GUID { get; set; }

        public Guid OperationalObject_GUID { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }
    }
}
