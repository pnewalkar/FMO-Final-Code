using System;
using System.Data.Entity.Spatial;

namespace RM.DataManagement.AccessLink.WebAPI.DTOs
{
    public class OsRoadLinkDTO
    {
        public Guid ID { get; set; }

        public DbGeometry CentreLineGeometry { get; set; }

        public bool? Ficticious { get; set; }

        public string RoadClassificaton { get; set; }

        public string RouteHierarchy { get; set; }

        public string FormOfWay { get; set; }

        public byte[] TrunkRoad { get; set; }

        public byte[] PrimaryRoute { get; set; }

        public string RoadClassificationNumber { get; set; }

        public string RoadName { get; set; }

        public string AlternateName { get; set; }

        public string Directionality { get; set; }

        public decimal LengthInMeters { get; set; }

        public string StartNodeTOID { get; set; }

        public string EndNodeTOID { get; set; }

        public byte StartGradeSeparation { get; set; }

        public byte EndGradeSeparation { get; set; }

        public string OperationalState { get; set; }

        public string Type { get; set; }

        public object Features { get; set; }
    }
}