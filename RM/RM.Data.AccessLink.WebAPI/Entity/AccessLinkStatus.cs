namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
