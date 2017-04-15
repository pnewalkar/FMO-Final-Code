namespace Fmo.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AMUChangeRequest")]
    public partial class AMUChangeRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AMUChangeRequest_Id { get; set; }

        public int? ChangeRequestType_Id { get; set; }

        public int? ChangeRequestStatus_Id { get; set; }

        [StringLength(300)]
        public string UnitCommentText { get; set; }

        public int? CurrentAddress_Id { get; set; }

        [StringLength(300)]
        public string AddressChanges { get; set; }

        public int? NewAddress_Id { get; set; }

        public int? ChangeReasonCode_Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? RaisedDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ClosedDate { get; set; }

        [StringLength(300)]
        public string AMUClarificationText { get; set; }

        [StringLength(8)]
        public string RequestPostcode { get; set; }

        public Guid ID { get; set; }

        public Guid? ChangeRequestType_GUID { get; set; }

        public Guid? ChangeRequestStatus_GUID { get; set; }

        public Guid? CurrentAddress_GUID { get; set; }

        public Guid? NewAddress_GUID { get; set; }

        public Guid? ChangeReasonCode_GUID { get; set; }

        public Guid? RequestPostcode_GUID { get; set; }

        public virtual PostalAddress PostalAddress { get; set; }

        public virtual PostalAddress PostalAddress1 { get; set; }

        public virtual Postcode Postcode { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }

        public virtual ReferenceData ReferenceData2 { get; set; }
    }
}
