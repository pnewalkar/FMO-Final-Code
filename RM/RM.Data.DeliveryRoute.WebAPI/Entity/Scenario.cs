namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.Scenario")]
    public partial class Scenario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Auto Generated")]
        public Scenario()
        {
            ScenarioRoutes = new HashSet<ScenarioRoute>();
        }

        public Guid ID { get; set; }

        [Required]
        [StringLength(50)]
        public string ScenarioName { get; set; }

        public Guid LocationID { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Location Location { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Auto Generated")]
        public virtual ICollection<ScenarioRoute> ScenarioRoutes { get; set; }
    }
}