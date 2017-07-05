using System;
using System.Collections.Generic;

namespace RM.CommonLibrary.EntityFramework.DTO.Model
{
    [Serializable]
    public class RouteLogSummaryModelDTO
    {
        public RouteDTO DeliveryRoute { get; set; }

        public List<RouteLogSequencedPointsDTO> RouteLogSequencedPoints { get; set; }
    }
}