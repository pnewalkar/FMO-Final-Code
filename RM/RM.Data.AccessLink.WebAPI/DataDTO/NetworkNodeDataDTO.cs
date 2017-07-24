using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class NetworkNodeDataDTO
    {


        public Guid ID { get; set; }

        public Guid NetworkNodeType_GUID { get; set; }

        
        public string TOID { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        // public virtual DeliveryPoint DeliveryPoint { get; set; }

        public virtual LocationDataDTO LocationDatatDTO { get; set; }

       
        public virtual ICollection<NetworkLinkDataDTO> NetworkLinksDataDTO { get; set; }

      
        public virtual ICollection<NetworkLinkDataDTO> NetworkLinks1 { get; set; }
    }
}
