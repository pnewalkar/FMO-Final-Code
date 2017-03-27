using System;
using System.Collections.Generic;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.Entities;

namespace Fmo.DataServices.Repositories
{
    public class RouteLogRepository : IRouteLogRepository
    {
        public List<DeliveryRoute> ListOfRouteLogs()
        {
            List<DeliveryRoute> listOfRouteLogs = new List<DeliveryRoute>();
            return listOfRouteLogs;
        }

        public List<Scenario> ListOfScenario()
        {
            throw new NotImplementedException();
        }
    }
}