namespace FMO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.POBox")]
    public partial class POBox
    {
        [Key]
        public int POBox_Id { get; set; }

        [Column("UDPRN ")]
        public int UDPRN_ { get; set; }

        public long POBoxNumber { get; set; }

        public int? POBoxType_Id { get; set; }

        public int? POBoxLinkedAddress_Id { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}
