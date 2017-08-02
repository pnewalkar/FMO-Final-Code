namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.ScenarioStatus")]
    public partial class ScenarioStatu
    {
        public Guid ID { get; set; }

        public Guid ScenarioID { get; set; }

        public Guid ScenarioStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Scenario Scenario { get; set; }
    }
}
