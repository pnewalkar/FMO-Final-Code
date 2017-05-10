using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO.Model
{
    public class CreateDeliveryPointModelDTO
    {
        public string Message { get; set; }
        public Guid ID { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
