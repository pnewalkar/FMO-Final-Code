using System;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DTO
{
    public class PostcodeDTO
    {
        public Guid ID { get; set; }

        public string PostcodeUnit { get; set; }

        public string OutwardCode { get; set; }

        public string InwardCode { get; set; }

        public Guid? PrimaryRouteGUID { get; set; }

        public Guid? SecondaryRouteGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}