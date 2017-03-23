using Fmo.Entities;
using Fmo.DataServices.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories
{
  public  class RouteSimulationRepository : IRouteSimulationRepository
    {
        public List<DeliveryRoute> ListOfRoute()
        {
            List<DeliveryRoute> ListOfRouteLogs = new List<DeliveryRoute>();
            return ListOfRouteLogs;

        }
    }
}
