using System;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.DataManagement.AccessLink.WebAPI.DTOs
{
    public class NetworkLinkDTO
    {
        public Guid Id { get; set; }

        public string TOID { get; set; }

        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry LinkGeometry { get; set; }

        public decimal LinkLength { get; set; }

        public int? LinkGradientType { get; set; }

        public Guid? NetworkLinkType_GUID { get; set; }

        public Guid? DataProvider_GUID { get; set; }

        public Guid? RoadName_GUID { get; set; }

        public Guid? StreetName_GUID { get; set; }

        public Guid? StartNode_GUID { get; set; }

        public Guid? EndNode_GUID { get; set; }

        public string LinkName { get; set; }
    }
}