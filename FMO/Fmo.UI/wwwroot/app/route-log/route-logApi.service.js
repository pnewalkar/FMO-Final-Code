
// Just for reference
angular.module('routeLog')
    .factory('routeLogAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
    var routeLogAPIService = {};
  
    //var getUsers = function (objEmployee) {
    //    return $http.post(GlobalSettings.apiUrl + '/Values', objEmployee);
    //};

    var getStatus = function () {
        return $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsStatus');
    };

    var getScenario = function (operationStateID, deliveryUnitID) {
        return $http.get(GlobalSettings.apiUrl + '/RouteLog/SearchDeliveryScenario?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
    };

    var getRoutes = function (objEmployee) {
        return $http.get(GlobalSettings.apiUrl + '/RouteLog/SearchDeliveryRoute?deliveryUnitID=' + deliveryUnitID + '&deliveryUnitID=' + deliveryUnitID);
    };

   // fmoService.getUsers = getUsers;
    routeLogAPIService.GetStatus = getStatus;
    routeLogAPIService.GetScenario = getScenario;
    routeLogAPIService.GetRoutes = getRoutes;

    return routeLogAPIService;
  
}]);