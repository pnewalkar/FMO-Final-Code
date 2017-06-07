using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class AddressLocationDTO
    {
        public int? UDPRN { get; set; }

        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry LocationXY { get; set; }
        public decimal? Lattitude { get; set; }
        public decimal? Longitude { get; set; }
        public Guid ID { get; set; }
    }
}
