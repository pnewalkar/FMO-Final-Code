namespace Fmo.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.POBox")]
    public partial class POBox
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int POBox_Id { get; set; }

        [Column("UDPRN ")]
        public int UDPRN_ { get; set; }

        public long POBoxNumber { get; set; }

        public int? POBoxType_Id { get; set; }

        public int? POBoxLinkedAddress_Id { get; set; }

        public Guid ID { get; set; }

        public Guid? POBoxType_GUID { get; set; }

        public Guid? POBoxLinkedAddress_GUID { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }
    }
}