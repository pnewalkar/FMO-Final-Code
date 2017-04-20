using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO.FileProcessing
{
    public class AddressLocationUSRPOSTDTO
    {
        public int? UDPRN { get; set; }

        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string ChangeType { get; set; }
    }
}
