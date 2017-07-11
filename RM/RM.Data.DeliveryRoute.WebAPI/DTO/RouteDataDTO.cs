using System;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DTO
{
    internal class RouteDataDTO
    {
        public Guid ID { get; set; }

        public int? GeoRouteID { get; set; }

        public string RouteName { get; set; }

        public string RouteNumber { get; set; }

        public decimal? SpanTimeMinute { get; set; }

        public string RouteBarcode { get; set; }

        public Guid RouteMethodTypeGUID { get; set; }

        public double? TotalDistanceMeter { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}