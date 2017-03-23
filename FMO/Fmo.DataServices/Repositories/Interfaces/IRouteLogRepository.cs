using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
  public interface IRouteLogRepository
    {
        List<Scenario> ListOfScenario();
        List<DeliveryRoute> ListOfRouteLogs();

        
    }
}
