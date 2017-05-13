using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class RouteLogSequencedPointsDTO
    {
        public string StreetName { get; set; }

        public short? BuildingNumer { get; set; }

        public int DeliveryPointCount { get; set; }

        public int? MultipleOccupancy { get; set; }

        public string FormattedBuildingNumber { get; set; }
    }
}
