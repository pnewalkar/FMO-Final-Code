namespace RM.Common.ActionManager.WebAPI.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.PostalAddressIdentifier")]
    public partial class PostalAddressIdentifier
    {
        public Guid ID { get; private set; }

        public Guid? PostalAddressID { get; private set; }

        public Guid? IdentifierTypeGUID { get; private set; }

        [StringLength(100)]
        public string ExternalID { get; private set; }

        public DateTime RowCreateDateTime { get; private set; }

        [StringLength(50)]
        public string Name { get; private set; }

        public virtual ReferenceData ReferenceData { get; private set; }
    }
}
