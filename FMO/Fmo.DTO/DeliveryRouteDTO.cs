using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class DeliveryRouteDTO
    {

        public int DeliveryRoute_Id { get; set; }

        public int? ExternalId { get; set; }

        public string RouteName { get; set; }

        public string RouteNumber { get; set; }

        public int OperationalStatus_Id { get; set; }

        public int RouteMethodType_Id { get; set; }

        public int? TravelOutTransportType_Id { get; set; }

        public int? TravelInTransportType_Id { get; set; }

        public decimal? TravelOutTimeMin { get; set; }

        public decimal? TravelInTimeMin { get; set; }

        public decimal? SpanTimeMin { get; set; }

        public int? DeliveryScenario_Id { get; set; }

        public string DeliveryRouteBarcode { get; set; }

    }
}
