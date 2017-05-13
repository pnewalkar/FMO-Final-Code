using System;

namespace Fmo.DTO.Model
{
    [Serializable]
    public class RouteLogSummaryModelDTO
    {
        public DeliveryRouteDTO DeliveryRoute { get; set; }

        public RouteLogSequencedPointsDTO RouteLogSequencedPoints { get; set; }
    }
}