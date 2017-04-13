
// Just for reference
angular.module('routeLog')
    .factory('routeLogAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {

        return {
            getSelectionType:getSelectionType,
            getStatus: getStatus,
            getScenario: getScenario,
            getRoutes: getRoutes
        }

        function getSelectionType() {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsSelectionType');
        };

        function getStatus() {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsStatus');
        };

        function getScenario(operationStateID, deliveryUnitID) {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryScenario?operationStateID=' + operationStateID + '&deliveryUnitID=' + deliveryUnitID);
        };

        function getRoutes(operationStateID, deliveryScenarioID) {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryRoute?operationStateID=' + operationStateID + '&deliveryScenarioID=' + deliveryScenarioID);
        };
    }]);