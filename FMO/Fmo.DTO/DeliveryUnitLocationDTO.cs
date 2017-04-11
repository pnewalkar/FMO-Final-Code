using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
  public class DeliveryUnitLocationDTO
    {
        public int DeliveryUnit_Id { get; set; }
        
        public string ExternalId { get; set; }

        public string UnitName { get; set; }

        public int UnitAddressUDPRN { get; set; }

        public Guid ID { get; set; }

    }
}
