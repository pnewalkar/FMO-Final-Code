using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.ScenarioRoute")]
    public partial class ScenarioRoute
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ScenarioID { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid RouteID { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime RowCreateDateTime { get; set; }

        public virtual Route Route { get; set; }

        public virtual Scenario Scenario { get; set; }
    }
}