using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class NotificationDTO
    {
        public int Notification_Id { get; set; }

        public int? NotificationType_Id { get; set; }

        public string Notification_Heading { get; set; }

        public string Notification_Message { get; set; }

        public DateTime? NotificationDueDate { get; set; }

        public int? NotificationPriority_Id { get; set; }

        public string NotificationActionLink { get; set; }

        public string NotificationSource { get; set; }

        public string PostcodeDistrict { get; set; }

        public string PostcodeSector { get; set; }

    }
}
