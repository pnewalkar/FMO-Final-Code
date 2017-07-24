using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.RouteNetworkLink")]
    public partial class RouteNetworkLink
    {
        public Guid ID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? OrderIndex { get; set; }

        public Guid? RouteActivityID { get; set; }

        public Guid? NetworkLinkID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual RouteActivity RouteActivity { get; set; }
    }
}