namespace RM.CommonLibrary.EntityFramework.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostalAddressLocation")]
    public partial class PostalAddressLocation
    {
        public Guid ID { get; set; }

        public Guid? LocationGUID { get; set; }

        public Guid PostalAddressGUID { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime RowCreateDateTime { get; set; }

        public virtual Location Location { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }
    }
}
