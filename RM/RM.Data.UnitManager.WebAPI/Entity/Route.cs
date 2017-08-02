using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    [Table("FMO.Route")]
    public partial class Route
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public Route()
        {
            Postcodes = new HashSet<Postcode>();
            Postcodes1 = new HashSet<Postcode>();
            Route1 = new HashSet<Route>();
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

        public Guid? PairedRouteID { get; set; }

        public Guid? UnSequencedBlockID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<Postcode> Postcodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<Postcode> Postcodes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<Route> Route1 { get; set; }

        public virtual Route Route2 { get; set; }
    }
}