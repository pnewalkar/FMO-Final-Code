using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.Route")]
    public partial class Route
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Route()
        {
            RouteActivities = new HashSet<RouteActivity>();
            RouteStatus = new HashSet<RouteStatus>();
            ScenarioRoutes = new HashSet<ScenarioRoute>();
        }

        public Guid ID { get; set; }

        public int? GeoRouteID { get; set; }

        [StringLength(30)]
        public string RouteName { get; set; }

        [StringLength(10)]
        public string RouteNumber { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SpanTimeMinute { get; set; }

        [StringLength(20)]
        public string RouteBarcode { get; set; }

        public Guid RouteMethodTypeGUID { get; set; }

        public double? TotalDistanceMeter { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteActivity> RouteActivities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteStatus> RouteStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScenarioRoute> ScenarioRoutes { get; set; }
    }
}