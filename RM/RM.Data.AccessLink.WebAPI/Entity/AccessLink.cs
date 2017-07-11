namespace RM.DataManagement.AccessLink.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.AccessLink")]
    public partial class AccessLink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccessLink()
        {
            AccessLinkStatus = new HashSet<AccessLinkStatus>();
        }

        public Guid ID { get; set; }

        public bool? Approved { get; set; }

        [Column(TypeName = "numeric")]
        public decimal WorkloadLengthMeter { get; set; }

        public Guid? AccessLinkTypeGUID { get; set; }

        public Guid? LinkDirectionGUID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual NetworkLink NetworkLink { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessLinkStatus> AccessLinkStatus { get; set; }
    }
}
