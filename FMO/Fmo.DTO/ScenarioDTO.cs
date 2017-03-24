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

        public int? ScenarioUnitType_Id { get; set; }

    }
}
