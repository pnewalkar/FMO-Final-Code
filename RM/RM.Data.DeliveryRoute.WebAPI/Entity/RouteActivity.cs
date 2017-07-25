using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.RouteActivity")]
    public partial class RouteActivity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public RouteActivity()
        {
            RouteNetworkLinks = new HashSet<RouteNetworkLink>();
        }

        public Guid ID { get; set; }

        public Guid? ActivityTypeGUID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? RouteActivityOrderIndex { get; set; }

        public Guid? BlockID { get; set; }

        public Guid? LocationID { get; set; }

        public Guid RouteID { get; set; }

        public double? ActivityDurationMinute { get; set; }

        public Guid? ResourceGUID { get; set; }

        public double? DistanceToNextLocationMeter { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Block Block { get; set; }

        public virtual Location Location { get; set; }

        public virtual Route Route { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<RouteNetworkLink> RouteNetworkLinks { get; set; }
    }
}