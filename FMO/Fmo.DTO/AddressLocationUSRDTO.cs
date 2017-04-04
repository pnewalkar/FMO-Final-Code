using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fmo.DTO
{
    [Serializable()]
    [XmlRoot("USR"), XmlType("addressLocation")]
    public class AddressLocationUSRDTO
    {
        public int? udprn { get; set; }

        public decimal? xCoordinate { get; set; }

        public decimal? yCoordinate { get; set; }

        public decimal? latitude { get; set; }

        public decimal? longitude { get; set; }

        public string changeType { get; set; }


    }
}
