namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Notification")]
    public partial class Notification
    {
        public Guid ID { get; private set; }

        [Column("Notification Heading")]
        [StringLength(50)]
        public string Notification_Heading { get; private set; }

        [Column("Notification Message")]
        [StringLength(300)]
        public string Notification_Message { get; private set; }

        public DateTime? NotificationDueDate { get; private set; }

        [StringLength(2000)]
        public string NotificationActionLink { get; private set; }

        [StringLength(50)]
        public string NotificationSource { get; private set; }

        [StringLength(4)]
        public string PostcodeDistrict { get; private set; }

        [StringLength(6)]
        public string PostcodeSector { get; private set; }

        public Guid? NotificationTypeGUID { get; private set; }

        public Guid? NotificationPriorityGUID { get; private set; }
    }
}
