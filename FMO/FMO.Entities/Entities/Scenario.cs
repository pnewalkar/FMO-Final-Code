namespace Fmo.Entities
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
            DeliveryRoutes = new HashSet<DeliveryRoute>();
        }

        public int Scenario_Id { get; set; }

        [StringLength(50)]
        public string ScenarioName { get; set; }

        public int? OperationalState_Id { get; set; }

        public Guid ID { get; set; }

        public Guid? OperationalState_GUID { get; set; }

        public int? Unit_Id { get; set; }

        public Guid? Unit_GUID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryRoute> DeliveryRoutes { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual UnitLocation UnitLocation { get; set; }
    }
}
