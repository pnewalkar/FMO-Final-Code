﻿using System;

namespace RM.DataManagement.PostalAddress.WebAPI.DataDTO
{
    public class DeliveryPointDataDTO
    {
        public Guid ID { get; set; }

        public Guid PostalAddressID { get; set; }

        public short? MultipleOccupancyCount { get; set; }

        public int? MailVolume { get; set; }

        public Guid DeliveryPointUseIndicatorGUID { get; set; }

        public byte[] RowVersion { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public virtual PostalAddressDataDTO PostalAddress { get; set; }
    }
}