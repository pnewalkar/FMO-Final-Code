namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    using System;

    public class NetworkNodeDatabaseDTO
    {
        public Guid ID { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        public string TOID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public DeliveryPointDatabaseDTO DeliveryPoint { get; set; }

        public LocationDatabaseDTO Location { get; set; }
    }
}
