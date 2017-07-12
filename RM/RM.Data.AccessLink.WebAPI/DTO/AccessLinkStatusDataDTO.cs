using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.AccessLink.WebAPI.DTO
{
    public class AccessLinkStatusDataDTO
    {
        /// <summary>
        /// This class represents data transfer object for AccessLinStatus entity
        /// </summary>
        public Guid ID { get; set; }

        public Guid NetworkLinkID { get; set; }

        public Guid AccessLinkStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}
