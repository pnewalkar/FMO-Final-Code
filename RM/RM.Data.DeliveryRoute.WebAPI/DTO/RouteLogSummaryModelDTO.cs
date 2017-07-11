using System;
using System.Collections.Generic;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DTO
{
    [Serializable]
    internal class RouteLogSummaryModelDTO
    {
        public RouteDTO DeliveryRoute { get; set; }

        public List<RouteLogSequencedPointsDTO> RouteLogSequencedPoints { get; set; }
    }
}