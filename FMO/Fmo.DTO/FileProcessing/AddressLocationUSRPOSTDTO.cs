using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO.FileProcessing
{
    public class AddressLocationUSRPOSTDTO
    {
        public int? udprn { get; set; }

        public decimal? xCoordinate { get; set; }

        public decimal? yCoordinate { get; set; }

        public decimal? latitude { get; set; }

        public decimal? longitude { get; set; }

        public string changeType { get; set; }
    }
}
