using System.Collections.Generic;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IRouteSimulationRepository
    {
        List<DeliveryRoute> ListOfRoute();

        List<Scenario> ListOfScenario();
    }
}