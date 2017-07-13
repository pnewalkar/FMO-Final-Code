namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Scenario")]
    public partial class Scenario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Scenario()
        {
            ScenarioRoutes = new HashSet<ScenarioRoute>();
        }

        public Guid ID { get; private set; }

        [Required]
        [StringLength(50)]
        public string ScenarioName { get; private set; }

        public Guid LocationID { get; private set; }

        public DateTime? StartDateTime { get; private set; }

        public DateTime? EndDateTime { get; private set; }

        public DateTime? LastModifiedDateTime { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        public virtual Location Location { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScenarioRoute> ScenarioRoutes { get; private set; }
    }
}