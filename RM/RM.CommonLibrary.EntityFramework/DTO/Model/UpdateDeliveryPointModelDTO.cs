using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO.Model
{
    public class UpdateDeliveryPointModelDTO
    {
        public decimal? XCoordinate { get; set; }

        public decimal? YCoordinate { get; set; }
        public Guid ID { get; set; }
    }
}
