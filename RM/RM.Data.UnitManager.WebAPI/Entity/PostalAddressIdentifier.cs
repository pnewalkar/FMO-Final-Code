namespace RM.DataManagement.UnitManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostalAddressIdentifier")]
    public partial class PostalAddressIdentifier
    {
        public Guid ID { get; set; }

        public Guid? PostalAddressID { get; set; }

        public Guid? IdentifierTypeGUID { get; set; }

        [StringLength(100)]
        public string ExternalID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }
    }
}
