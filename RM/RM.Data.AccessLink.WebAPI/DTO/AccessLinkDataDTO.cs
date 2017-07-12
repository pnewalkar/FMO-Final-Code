using RM.DataManagement.AccessLink.WebAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.AccessLink.WebAPI.DTO
{
    public class AccessLinkDataDTO
    {
        public AccessLinkDataDTO()
        {
            AccessLinkStatus = new List<AccessLinkStatus>();
        }

        /// <summary>
        /// This class represents data transfer object for AccessLink entity
        /// </summary>
        public Guid ID { get; set; }

        public bool? Approved { get; set; }
       
        public decimal WorkloadLengthMeter { get; set; }

        public Guid? AccessLinkTypeGUID { get; set; }

        public Guid? LinkDirectionGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public IList<AccessLinkStatus> AccessLinkStatus { get; set; }
    }
}
