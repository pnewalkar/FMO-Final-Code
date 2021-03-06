using System;
using System.Data.Entity.Spatial;

namespace RM.DataManagement.NetworkManager.WebAPI.DataDTO
{
    public class StreetNameDataDTO
    {
        public Guid ID { get; set; }

        public string USRN { get; set; }

        public string NationalRoadCode { get; set; }

        public string DesignatedName { get; set; }

        public string LocalName { get; set; }

        public string Descriptor { get; set; }

        public string RoadClassification { get; set; }

        public string StreetType { get; set; }

        public DbGeometry Geometry { get; set; }

        public string StreetNameProvider { get; set; }

        public string OperationalState { get; set; }

        public string OperationalStateReason { get; set; }

        public DateTime? OperationalStateStartTime { get; set; }

        public DateTime? OperationalStateEndTime { get; set; }

        public string Locality { get; set; }

        public string Town { get; set; }

        public string AdministrativeArea { get; set; }
    }
}