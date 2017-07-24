namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.ScenarioDayOfTheWeek")]
    public partial class ScenarioDayOfTheWeek
    {
        public Guid ID { get; set; }

        public Guid ScenarioID { get; set; }

        public Guid DayOfTheWeekGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Scenario Scenario { get; set; }
    }
}
