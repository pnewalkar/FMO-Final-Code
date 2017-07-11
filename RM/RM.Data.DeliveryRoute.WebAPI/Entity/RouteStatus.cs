using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.RouteStatus")]
    public partial class RouteStatus
    {
        public Guid ID { get; set; }

        public Guid RouteID { get; set; }

        public Guid RouteStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual Route Route { get; set; }
    }
}