namespace RM.Data.DeliveryPoint.WebAPI.DataDTO
{
    using System;

    public class NetworkNodeDataDTO
    {
        public Guid ID { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        public string TOID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public LocationDataDTO Location { get; set; }
    }
}
