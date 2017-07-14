using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Operational.RouteLog.WebAPI.DTO.Model
{
    [Serializable]
    public class RouteLogSummaryDTO
    {
        public RouteDTO DeliveryRoute { get; set; }

        public List<RouteLogSequencedPointsDTO> RouteLogSequencedPoints { get; set; }
    }
}
