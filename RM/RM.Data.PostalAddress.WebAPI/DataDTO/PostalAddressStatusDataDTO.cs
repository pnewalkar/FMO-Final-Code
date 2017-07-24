using System;

namespace RM.DataManagement.PostalAddress.WebAPI.DataDTO
{
    public class PostalAddressStatusDataDTO
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