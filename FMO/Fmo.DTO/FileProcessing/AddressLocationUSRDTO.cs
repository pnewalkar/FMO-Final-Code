using Fmo.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fmo.DTO.FileProcessing
{
    [Serializable()]
    [XmlType(Constants.ADDRESSLOCATIONXMLROOT)]
    public class AddressLocationUSRDTO
    {
        [XmlElement(ElementName = Constants.USRUDPRN)]
        public int? UDPRN { get; set; }

        [XmlElement(ElementName = Constants.USRXCOORDINATE)]
        public decimal? XCoordinate { get; set; }

        [XmlElement(ElementName = Constants.USRYCOORDINATE)]
        public decimal? YCoordinate { get; set; }

        [XmlElement(ElementName = Constants.USRLATITUDE)]
        public decimal? Latitude { get; set; }

        [XmlElement(ElementName = Constants.USRLONGITITUDE)]
        public decimal? Longitude { get; set; }

        [XmlElement(ElementName = Constants.USRCHANGETYPE)]
        public string ChangeType { get; set; }


    }
}
