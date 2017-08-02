namespace RM.DataManagement.DeliveryRoute.WebAPI.DataDTO
{
    public class RouteLogSequencedPointsDataDTO
    {
        public string StreetName { get; set; }

        public short? BuildingNumber { get; set; }

        public int? MultipleOccupancy { get; set; }

        public string BuildingName { get; set; }

        public string SubBuildingName { get; set; }
    }
}