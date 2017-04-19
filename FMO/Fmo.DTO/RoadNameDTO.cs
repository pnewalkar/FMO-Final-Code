namespace Fmo.DTO
{
    public class RoadNameDTO
    {
        /// <summary>
        /// This class represents data transfer object for RoadLink entity
        /// </summary>
        public int RoadName_Id { get; set; }

        public string TOID { get; set; }

        public string NationalRoadCode { get; set; }

        public string roadClassification { get; set; }

        public string DesignatedName { get; set; }

        public string type { get; set; }
        public object features { get; set; }
    }
}