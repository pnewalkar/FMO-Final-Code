
// Just for reference
angular.module('simulation')
    .factory('simulationAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        
        return {
            getStatus: getStatus,
            getScenario: getScenario,
            getRoutes: getRoutes
        };

        function getStatus() {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsStatus');
        };

         function getScenario(operationStateID, deliveryUnitID) {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryScenario?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
        };

          function getRoutes(objEmployee) {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryRoute?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
        };
    }]);