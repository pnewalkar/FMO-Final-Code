namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.AccessLinkStatus")]
    public partial class AccessLinkStatus
    {
        public Guid ID { get; set; }

        public Guid NetworkLinkID { get; set; }

        public Guid AccessLinkStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual AccessLink AccessLink { get; set; }
    }
}