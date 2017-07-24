using System;
using System.Collections.Generic;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DTO
{
    [Serializable]
    public class RouteLogSummaryDTO
    {
        public RouteDTO Route { get; set; }

        public List<RouteLogSequencedPointsDTO> RouteLogSequencedPoints { get; set; }
    }
}