namespace RM.Operational.RouteLog.WebAPI.DTO
{
    public class RouteLogSequencedPointsDTO
    {
        public RouteLogSequencedPointsDTO(string streetName, string description, int deliveryPointCount, int? multipleOccupancy, string subbuildingname, string buildingname)
        {
            StreetName = streetName;
            Description = description;
            DeliveryPointCount = deliveryPointCount;
            MultipleOccupancy = null;
            SubBuildingName = subbuildingname;
            BuildingName = buildingname;
            if (multipleOccupancy > 0)
            {
                MultipleOccupancy = multipleOccupancy;
            }
        }

        public RouteLogSequencedPointsDTO()
        {
        }

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