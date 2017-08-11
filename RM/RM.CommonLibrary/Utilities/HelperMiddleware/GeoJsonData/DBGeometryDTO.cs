using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.Utilities.HelperMiddleware.GeoJsonData
{
    public class DBGeometryDTO
    {
        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry Geometry { get; set; }
    }
}