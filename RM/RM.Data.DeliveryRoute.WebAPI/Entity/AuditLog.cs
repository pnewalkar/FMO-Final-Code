using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Data.DeliveryRoute.WebAPI.Entities
{
    [Table("FMO.AuditLog")]
    public partial class AuditLog
    {
        [Key]
        public Guid AuditLog_Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TableName { get; set; }

        [Required]
        [StringLength(50)]
        public string RecordId { get; set; }

        [StringLength(50)]
        public string ColumnName { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string EventType { get; set; }

        public DateTime EventTimeStamp { get; set; }
    }
}