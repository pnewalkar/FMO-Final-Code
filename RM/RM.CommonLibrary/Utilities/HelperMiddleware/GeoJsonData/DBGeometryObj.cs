using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.Utilities.HelperMiddleware.GeoJsonData
{
    public class DBGeometryObj
    {
        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry dbGeometry { get; set; }
    }
}
