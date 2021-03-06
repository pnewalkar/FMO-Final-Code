﻿using System;

namespace RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO
{
    public class SupportingDeliveryPointDataDTO
    {
        public Guid ID { get; set; }

        public string GroupName { get; set; }

        public byte? NumberOfFloors { get; set; }

        public double? InternalDistanceMeters { get; set; }

        public double? WorkloadTimeOverrideMinutes { get; set; }

        public string TimeOverrideReason { get; set; }

        public bool? TimeOverrideApproved { get; set; }

        public Guid? GroupTypeGUID { get; set; }

        public Guid? ServicePointTypeGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public bool? DeliverToReception { get; set; }

        public DeliveryPointDataDTO DeliveryPoint { get; set; }
    }
}