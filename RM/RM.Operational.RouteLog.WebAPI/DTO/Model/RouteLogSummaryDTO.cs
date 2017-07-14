using System;
using System.Collections.Generic;

namespace RM.Operational.RouteLog.WebAPI.DTO.Model
{
    [Serializable]
    public class RouteLogSummaryDTO
    {
        public RouteDTO DeliveryRoute { get; set; }

        public List<RouteLogSequencedPointsDTO> RouteLogSequencedPoints { get; set; }
    }
}