
// Just for reference
angular.module('routeLog')
    .factory('routeLogAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
    var routeLogAPIService = {};
    var getStatus = function () {
        return $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsStatus');
    };

    var getScenario = function (operationStateID, deliveryUnitID) {
        return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryScenario?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
    };

    var getRoutes = function (objEmployee) {
        return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryRoute?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
    };

    routeLogAPIService.GetStatus = getStatus;
    routeLogAPIService.GetScenario = getScenario;
    routeLogAPIService.GetRoutes = getRoutes;

    return routeLogAPIService;
  
}]);