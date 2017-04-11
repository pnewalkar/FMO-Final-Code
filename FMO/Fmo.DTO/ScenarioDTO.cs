using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
   public class ScenarioDTO
    {
        public int DeliveryScenario_Id { get; set; }

        public string ScenarioName { get; set; }

        public int? DeliveryUnit_Id { get; set; }

        public int? OperationalState_Id { get; set; }

        public Guid ID { get; set; }

        public Guid? DeliveryUnit_GUID { get; set; }

        public Guid? OperationalState_GUID { get; set; }

    }
}
