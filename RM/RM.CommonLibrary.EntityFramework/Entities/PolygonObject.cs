namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PolygonObject")]
    public partial class PolygonObject
    {
        public bool ObjectExcluded { get; set; }

        [Column(TypeName = "numeric")]
        public decimal GroupOrderIndex { get; set; }

        public Guid ID { get; set; }

        public Guid OperationalObject_GUID { get; set; }

        public Guid OperationalObjectType_GUID { get; set; }

        public Guid Polygon_GUID { get; set; }

        public virtual Polygon Polygon { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
