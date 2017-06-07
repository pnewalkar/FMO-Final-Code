using System;
using System.Xml.Serialization;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DTO.FileProcessing
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