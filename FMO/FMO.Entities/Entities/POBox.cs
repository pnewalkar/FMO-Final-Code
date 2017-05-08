namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.POBox")]
    public partial class POBox
    {
        [Column("UDPRN ")]
        public int UDPRN_ { get; set; }

        public long POBoxNumber { get; set; }

        public Guid ID { get; set; }

        public Guid? POBoxType_GUID { get; set; }

        public Guid? POBoxLinkedAddress_GUID { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
