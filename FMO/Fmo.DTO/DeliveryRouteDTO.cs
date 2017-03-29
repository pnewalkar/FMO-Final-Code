using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class DeliveryRouteDTO
    {

        public int DeliveryRouteId { get; set; }

        public int? ExternalId { get; set; }

        public string RouteName { get; set; }

        public string RouteNumber { get; set; }

        public int OperationalStatusId { get; set; }

        public int RouteMethodTypeId { get; set; }

        public int? TravelOutTransportTypeId { get; set; }

        public int? TravelInTransportTypeId { get; set; }

        public decimal? TravelOutTimeMin { get; set; }

        public decimal? TravelInTimeMin { get; set; }

        public decimal? SpanTimeMin { get; set; }

        public int? DeliveryScenarioId { get; set; }

        public string DeliveryRouteBarcode { get; set; }

    }
}
