
// Just for reference
angular.module('simulation')
    .factory('simulationAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var simulationAPIService = {};
        var getStatus = function () {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsStatus');
        };

        var getScenario = function (operationStateID, deliveryUnitID) {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryScenario?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
        };

        var getRoutes = function (objEmployee) {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryRoute?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
        };

        simulationAPIService.GetStatus = getStatus;
        simulationAPIService.GetScenario = getScenario;
        simulationAPIService.GetRoutes = getRoutes;

        return simulationAPIService;

    }]);