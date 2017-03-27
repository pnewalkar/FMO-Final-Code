using System.Collections.Generic;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IRouteLogRepository
    {
        List<Scenario> ListOfScenario();

        List<DeliveryRoute> ListOfRouteLogs();
    }
}