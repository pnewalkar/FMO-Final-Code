using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace RM.CommonLibrary.EntityFramework.DTO
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
