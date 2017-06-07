using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class RouteLogSequencedPointsDTO
    {
        public RouteLogSequencedPointsDTO(string streetName, string description, int deliveryPointCount, int? multipleOccupancy)
        {
            StreetName = streetName;
            Description = description;
            DeliveryPointCount = deliveryPointCount;
            MultipleOccupancy = null;
            if (multipleOccupancy > 0)
            {
                MultipleOccupancy = multipleOccupancy;
            }

        }

        public RouteLogSequencedPointsDTO()
        { }

        public string Description { get; set; }

        public string StreetName { get; set; }

        public short? BuildingNumber { get; set; }

        public int DeliveryPointCount { get; set; }

        public int? MultipleOccupancy { get; set; }

        public string FormattedBuildingNumber { get; set; }

        public string BuildingName { get; set; }

        public string SubBuildingName { get; set; }
    }
}
