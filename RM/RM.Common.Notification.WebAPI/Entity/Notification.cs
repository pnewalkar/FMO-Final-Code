namespace RM.Common.Notification.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Notification")]
    public partial class Notification
    {
        public Guid ID { get; set; }

        [Column("Notification Heading")]
        [StringLength(50)]
        public string Notification_Heading { get; set; }

        [Column("Notification Message")]
        [StringLength(300)]
        public string Notification_Message { get; set; }

        public DateTime? NotificationDueDate { get; set; }

        [StringLength(2000)]
        public string NotificationActionLink { get; set; }

        [StringLength(50)]
        public string NotificationSource { get; set; }

        [StringLength(4)]
        public string PostcodeDistrict { get; set; }

        [StringLength(6)]
        public string PostcodeSector { get; set; }

        public Guid? NotificationTypeGUID { get; set; }

        public Guid? NotificationPriorityGUID { get; set; }
    }
}
