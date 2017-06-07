using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO.Model
{
    public class DeliveryPointModelDTO
    {
        public int? UDPRN { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }

        public byte[] RowVersion { get; set; }

        public Guid ID { get; set; }
    }
}
