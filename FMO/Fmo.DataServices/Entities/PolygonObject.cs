namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PolygonObject")]
    public partial class PolygonObject
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Polygon_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OperationalObject_Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OperationalObjectType_Id { get; set; }

        public bool ObjectExcluded { get; set; }

        [Column(TypeName = "numeric")]
        public decimal GroupOrderIndex { get; set; }

        public virtual Polygon Polygon { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
