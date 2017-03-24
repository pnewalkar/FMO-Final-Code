using Fmo.Entities;
using Fmo.DataServices.Repositories.Interfaces;
using System.Collections.Generic;

namespace Fmo.DataServices.Repositories
{
    public class RouteSimulationRepository : IRouteSimulationRepository
    {
        public List<DeliveryRoute> ListOfRoute()
        {
            List<DeliveryRoute> listOfRouteLogs = new List<DeliveryRoute>();
            return listOfRouteLogs;

        }

        public List<Scenario> ListOfScenario()
        {
            List<Scenario> listOfScenario = new List<Scenario>();
            return listOfScenario;
        }
    }
}
