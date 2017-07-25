using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class NetworkNodeDataDTO
    {
        public NetworkNodeDataDTO()
        {
            this.Location = new LocationDataDTO();
        }

        public Guid ID { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        public string TOID { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public DeliveryPointDataDTO DeliveryPoint { get; set; }

        public LocationDataDTO Location { get; set; }

    }
}
