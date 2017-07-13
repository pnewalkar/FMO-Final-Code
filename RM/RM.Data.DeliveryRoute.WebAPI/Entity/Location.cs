using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.Location")]
    public partial class Location
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Location()
        {
            BlockSequences = new HashSet<BlockSequence>();
            RouteActivities = new HashSet<RouteActivity>();
            Scenarios = new HashSet<Scenario>();
        }

        public Guid ID { get; private set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlternateID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        [Required]
        public DbGeometry Shape { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BlockSequence> BlockSequences { get; private set; }

        public virtual NetworkNode NetworkNode { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteActivity> RouteActivities { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenario> Scenarios { get; private set; }
    }
}