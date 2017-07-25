namespace RM.Data.ThirdPartyAddressLocation.WebAPI.DTO
{
    using System;

    public class NotificationDataDTO
    {
        public Guid ID { get; private set; }

        public string Notification_Heading { get; private set; }

        public string Notification_Message { get; private set; }

        public DateTime? NotificationDueDate { get; private set; }

        public string NotificationActionLink { get; private set; }

        public string NotificationSource { get; private set; }

        public string PostcodeDistrict { get; private set; }

        public string PostcodeSector { get; private set; }

        public Guid? NotificationTypeGUID { get; private set; }

        public Guid? NotificationPriorityGUID { get; private set; }
    }
}