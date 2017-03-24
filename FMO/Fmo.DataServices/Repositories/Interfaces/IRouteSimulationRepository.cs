﻿using Fmo.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IRouteSimulationRepository
    {
        List<DeliveryRoute> ListOfRoute();
        List<Scenario> ListOfScenario();
    }
}
