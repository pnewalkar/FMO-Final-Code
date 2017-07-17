using RM.DataManagement.AccessLink.WebAPI.DataDTOs;
using RM.DataManagement.AccessLink.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.DataManagement.AccessLink.WebAPI.DataDTOs
{
    public class AccessLinkDataDTO
    {
       

        /// <summary>
        /// This class represents data transfer object for AccessLink entity
        /// </summary>
        public Guid ID { get; set; }

        public bool? Approved { get; set; }
       
        public decimal WorkloadLengthMeter { get; set; }

        public Guid? AccessLinkTypeGUID { get; set; }

        public Guid? LinkDirectionGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public  NetworkLinkDataDTO NetworkLinkDatatDTO { get; set; }

        public  NetworkLinkDataDTO NetworkLinkDataDTO1 { get; set; }

        public DateTime RowCreateDateTime { get; set; }
       

   
        public virtual AccessLinkStatusDataDTO AccessLinkStatusDataDTO { get; set; }
        public NetworkNodeDataDTO NetworkNodeDataDTO { get; set; }
    }
}
