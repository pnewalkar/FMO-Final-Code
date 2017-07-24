namespace RM.Data.DeliveryPoint.WebAPI.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.SupportingDeliveryPoint")]
    public partial class SupportingDeliveryPoint
    {
        public Guid ID { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public byte? NumberOfFloors { get; set; }

        public double? InternalDistanceMeters { get; set; }

        public double? WorkloadTimeOverrideMinutes { get; set; }

        [StringLength(300)]
        public string TimeOverrideReason { get; set; }

        public bool? TimeOverrideApproved { get; set; }

        public bool? ServicePoint { get; set; }

        public Guid? GroupTypeGUID { get; set; }

        public Guid? ServicePointTypeGUID { get; set; }

        public Guid SupportDeliveryPointTypeGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual DeliveryPoint DeliveryPoint { get; set; }
    }
}
