using System;

namespace RM.DataManagement.UnitManager.WebAPI.DTO
{
    public class PostCodeDTO
    {
        public Guid ID { get; set; }
        public string PostcodeUnit { get; set; }

        public string OutwardCode { get; set; }

        public string InwardCode { get; set; }

        public string Sector { get; set; }
    }
}
