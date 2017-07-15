﻿using System;
using System.Collections.Generic;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;

namespace RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces
{
    public interface IScenarioDataService
    {
        /// <summary>
        /// Get the list of route scenarios by the operationstateID and locationID.
        /// </summary>
        /// <param name="operationStateID"></param>
        /// <param name="locationID"></param>
        /// <returns></returns>
        List<ScenarioDataDTO> GetRouteScenarios(Guid operationStateID, Guid locationID);
    }
}