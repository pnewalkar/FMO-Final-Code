﻿using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Interfaces
{
   public interface IDeliveryRouteBusinessService
    {
        List<DTO.ReferenceDataDTO> ListOfRouteLogStatus();
        List<DTO.DeliveryRouteDTO> ListOfRoute();
        List<DTO.ScenarioDTO> ListOfScenario();
    }
}