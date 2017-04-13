
// Just for reference
angular.module('simulation')
    .factory('simulationAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        
        return {
            getStatus: getStatus,
            getScenario: getScenario,
            getRoutes: getRoutes
        };

        function getStatus() {
            return $http.get(GlobalSettings.apiUrl + '/RouteSimulation/RouteLogsStatus');
        };

         function getScenario(operationStateID, deliveryUnitID) {
             return $http.get(GlobalSettings.apiUrl + '/RouteSimulation/FetchDeliveryScenario?operationStateID=' + operationStateID + '&deliveryUnitID=' + deliveryUnitID);
        };

         function getRoutes(operationStateID,deliveryScenarioID) {
             return $http.get(GlobalSettings.apiUrl + '/RouteSimulation/FetchDeliveryRoute?operationStateID=' + operationStateID + '&deliveryScenarioID=' + deliveryScenarioID);
        };
    }]);