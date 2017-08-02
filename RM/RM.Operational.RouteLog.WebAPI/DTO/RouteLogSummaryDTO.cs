using System;
using System.Collections.Generic;

namespace RM.Operational.RouteLog.WebAPI.DTO
{
    [Serializable]
    public class RouteLogSummaryDTO
    {
        public RouteDTO Route { get; set; }

        public List<RouteLogSequencedPointsDTO> RouteLogSequencedPoints { get; set; }
    }
}