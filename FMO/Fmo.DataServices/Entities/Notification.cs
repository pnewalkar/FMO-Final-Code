namespace Fmo.DataServices.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.Notification")]
    public partial class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Notification_Id { get; set; }

        public int? NotificationType_Id { get; set; }

        [Column("Notification Heading")]
        [StringLength(50)]
        public string Notification_Heading { get; set; }

        [Column("Notification Message")]
        [StringLength(300)]
        public string Notification_Message { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? NotificationDueDate { get; set; }

        public int? NotificationPriority_Id { get; set; }

        [StringLength(2000)]
        public string NotificationActionLink { get; set; }

        [StringLength(50)]
        public string NotificationSource { get; set; }

        [StringLength(4)]
        public string PostcodeDistrict { get; set; }

        [StringLength(6)]
        public string PostcodeSector { get; set; }

        public virtual ReferenceData ReferenceData { get; set; }

        public virtual ReferenceData ReferenceData1 { get; set; }
    }
}
