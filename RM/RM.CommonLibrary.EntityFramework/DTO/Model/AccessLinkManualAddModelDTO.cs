namespace RM.CommonLibrary.EntityFramework.DTO.Model
{
    public class AccessLinkManualCreateModelDTO
    {
        public string BoundingBoxCoordinates { get; set; }

        public string OperationalObjectPoint { get; set; }

        public string NetworkIntersectionPoint { get; set; }

        public string AccessLinkLine { get; set; }

        public string NetworkLinkGUID { get; set; }

        public string OperationalObjectGUID { get; set; }

        public decimal Workloadlength { get; set; }
    }
}