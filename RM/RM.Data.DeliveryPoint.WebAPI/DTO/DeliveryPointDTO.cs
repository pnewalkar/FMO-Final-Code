using System;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    /// <summary>
    /// This class represents data transfer object for DeliveryPoint entity
    /// </summary>
    public class DeliveryPointDTO
    {
        public DeliveryPointDTO()
        {
            this.PostalAddress = new PostalAddressDTO();
        }

        public Guid ID { get; set; }
        public bool AccessLinkPresent { get; set; }
        public short? MultipleOccupancyCount { get; set; }
        public int? MailVolume { get; set; }
        public bool IsUnit { get; set; }
        public Guid Address_GUID { get; set; }
        public Guid DeliveryPointUseIndicator_GUID { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime RowCreateDateTime { get; set; }
  
        public PostalAddressDTO PostalAddress { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        public string LocationProvider { get; set; }

        public string OperationalStatus { get; set; }

        [JsonConverter(typeof(DbGeometryConverter))]
        public DbGeometry LocationXY { get; set; }

        public bool Positioned { get; set; }

        public bool RMGDeliveryPointPresent { get; set; }

        public int? UDPRN { get; set; }

        public string DeliveryPointUseIndicator { get; set; }

        public Guid? LocationProvider_GUID { get; set; }

        public Guid? OperationalStatus_GUID { get; set; }

        public Guid? DeliveryGroup_GUID { get; set; }

        public Guid DeliveryRoute_Guid { get; set; }
    }
}