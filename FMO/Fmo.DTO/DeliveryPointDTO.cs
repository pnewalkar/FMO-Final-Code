using Fmo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fmo.DTO
{
  public  class DeliveryPointDTO
    {
        public int DeliveryPoint_Id { get; set; }

        public int Address_Id { get; set; }

        public string LocationProvider { get; set; }

        public string OperationalStatus { get; set; }

        public DbGeometry LocationXY { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public bool Positioned { get; set; }

        public bool AccessLinkPresent { get; set; }

        public bool RMGDeliveryPointPresent { get; set; }

        public int? UDPRN { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public string DeliveryPointUseIndicator { get; set; }

        public int? DeliveryGroup_Id { get; set; }

        public bool IsUnit { get; set; }

        public  PostalAddressDTO PostalAddress { get; set; }

      

    }
}
