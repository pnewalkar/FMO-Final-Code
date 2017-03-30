using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    [Serializable()]
    public class USR
    {
        public int? UDPRN { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public decimal? Lat { get; set; }

        public decimal? Long { get; set; }

        public string CHANGE_TYPE { get; set; }


    }
}
