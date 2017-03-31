using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class AddressLocationDTO
    {
        public int? UDPRN { get; set; }
        public string LocationXY { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
