using System;

namespace RM.DataManagement.PostalAddress.WebAPI.DTO
{
    public class PostcodeDTO
    {
        public Guid ID { get; set; }

        public string PostcodeUnit { get; set; }

        public string OutwardCode { get; set; }

        public string InwardCode { get; set; }

        public string Sector { get; set; }
    }
}