namespace RM.DataManagement.PostalAddress.WebAPI.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.PostalAddressStatus")]
    public partial class PostalAddressStatus
    {
        public Guid ID { get; set; }

        public Guid PostalAddressGUID { get; set; }

        public Guid OperationalStatusGUID { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }
    }
}