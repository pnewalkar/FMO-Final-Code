using System;

namespace Fmo.DTO
{
    public class NotificationDTO
    {
        public string Notification_Heading { get; set; }

        public string Notification_Message { get; set; }

        public DateTime? NotificationDueDate { get; set; }

        public int? NotificationPriority_Id { get; set; }

        public string NotificationActionLink { get; set; }

        public string NotificationSource { get; set; }

        public string PostcodeDistrict { get; set; }

        public string PostcodeSector { get; set; }

        public Guid ID { get; set; }

        public Guid? NotificationType_GUID { get; set; }

        public Guid? NotificationPriority_GUID { get; set; }
    }
}