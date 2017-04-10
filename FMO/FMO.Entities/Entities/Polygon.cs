namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Polygon")]
    public partial class Polygon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Polygon()
        {
            AreaHazards = new HashSet<AreaHazard>();
            DeliveryGroups = new HashSet<DeliveryGroup>();
            PolygonObjects = new HashSet<PolygonObject>();
        }

        public int Polygon_Id { get; set; }

        public int PolygonType_Id { get; set; }

        [Required]
        public DbGeometry PolygonGeometry { get; set; }

        [Required]
        public DbGeometry PolygonCentroid { get; set; }

        public Guid ID { get; set; }

        public Guid PolygonType_GUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AreaHazard> AreaHazards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryGroup> DeliveryGroups { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PolygonObject> PolygonObjects { get; set; }
    }
}