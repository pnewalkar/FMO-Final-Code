using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM.CommonLibrary.EntityFramework.DTO
{ 
    public class ResponseData
    {
        public int number { get; set; }
        public string address { get; set; }
        public string frequency { get; set; }
        public string dpUse  { get; set; }
        public string multiOcc { get; set; }
        public string mailVol { get; set; }

        public decimal xCoor { get; set; }

        public decimal yCoor { get; set; }
    }
}