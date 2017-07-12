using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.DataManagement.PostalAddress.WebAPI.DTO
{
    public class PostalAddressStatusDBDTO
    {
        /// <summary>
        /// This class represents data transfer object for PostalAddress entity
        /// </summary>
        public Guid ID { get; set; }

        public Guid PostalAddressGUID { get; set; }

        public Guid OperationalStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}
