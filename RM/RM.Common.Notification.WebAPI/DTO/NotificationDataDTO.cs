using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Common.Notification.WebAPI.DTO
{
    public class NotificationDataDTO
    {
        public Guid ID { get; set; }

        public string Notification_Heading { get; set; }

        public string Notification_Message { get; set; }

        public DateTime? NotificationDueDate { get; set; }

        public string NotificationActionLink { get; set; }

        public string NotificationSource { get; set; }

        public string PostcodeDistrict { get; set; }

        public string PostcodeSector { get; set; }

        public Guid? NotificationTypeGUID { get; set; }

        public Guid? NotificationPriorityGUID { get; set; }
    }
}
