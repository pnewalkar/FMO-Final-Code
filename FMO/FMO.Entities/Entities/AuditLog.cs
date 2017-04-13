using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fmo.Entities
{
    [Table("FMO.AuditLog")]
    public class AuditLog
    {
        [Key]
        public Guid AuditLog_Id { get; set; }

        public DateTime? EventTimeStamp { get; set; }

        public string EventType { get; set; }

        public string RecordId { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public string NewValue { get; set; }

        public string OriginalValue { get; set; }

        public string UserId { get; set; }
    }
}