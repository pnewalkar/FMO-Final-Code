using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fmo.Entities
{
    [Table("FMO.AuditLog")]
    public class AuditLog
    {
        [Key]
        public object Id { get; set; }

        public object EventTimeStamp { get; set; }

        public string EventType { get; set; }

        public object RecordId { get; set; }

        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public object NewValue { get; set; }

        public string OriginalValue { get; set; }

        public string UserId { get; set; }
    }
}