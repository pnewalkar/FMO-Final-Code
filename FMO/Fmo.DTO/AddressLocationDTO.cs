using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity.Spatial;

namespace Fmo.DTO
{
    public class AddressLocationDTO
    {
        public int? UDPRN { get; set; }
        public DbGeometry LocationXY { get; set; }
        public decimal? Lattitude { get; set; }
        public decimal? Longitude { get; set; }
        public Guid ID { get; set; }
    }
}
