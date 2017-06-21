using System;
using System.Xml.Serialization;
using RM.CommonLibrary.EntityFramework.Utilities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DTO.FileProcessing
{
    [Serializable()]
    [XmlType(DTOConstants.ADDRESSLOCATIONXMLROOT)]
    public class AddressLocationUSRDTO
    {
        

        [XmlElement(ElementName = DTOConstants.USRUDPRN)]
        public int? UDPRN { get; set; }

        [XmlElement(ElementName = DTOConstants.USRXCOORDINATE)]
        public decimal? XCoordinate { get; set; }

        [XmlElement(ElementName = DTOConstants.USRYCOORDINATE)]
        public decimal? YCoordinate { get; set; }

        [XmlElement(ElementName = DTOConstants.USRLATITUDE)]
        public decimal? Latitude { get; set; }

        [XmlElement(ElementName = DTOConstants.USRLONGITITUDE)]
        public decimal? Longitude { get; set; }

        [XmlElement(ElementName = DTOConstants.USRCHANGETYPE)]
        public string ChangeType { get; set; }
    }
}