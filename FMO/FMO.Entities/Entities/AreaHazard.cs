namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AreaHazard")]
    public partial class AreaHazard
    {
        public int Hazard_Id { get; set; }

        public int Category_Id { get; set; }

        public int SubCategory_Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidUntilDate { get; set; }

        public int Polygon_Id { get; set; }

        public Guid ID { get; set; }

        public Guid Category_GUID { get; set; }

        public Guid SubCategory_GUID { get; set; }

        public Guid Polygon_GUID { get; set; }

        public virtual Polygon Polygon { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}
