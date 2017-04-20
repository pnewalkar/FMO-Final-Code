
// Just for reference
angular.module('routeLog')
    .factory('routeLogAPIService', routeLogAPIService);

routeLogAPIService.$inject = ['$http', '$q', 'GlobalSettings'];

function routeLogAPIService($http, $q, GlobalSettings) {

    return {
        getSelectionType: getSelectionType,
        getStatus: getStatus,
        getScenario: getScenario,
        getRoutes: getRoutes
    }

    function getSelectionType() {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsSelectionType').success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            console.log(err);
            deferred.reject(err);
        });

        return deferred.promise;
    };

    function getStatus() {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteLog/RouteLogsStatus').success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            console.log(err);
            deferred.reject(err);
        });

        return deferred.promise;
    };

    function getScenario(operationStateID, deliveryUnitID) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryScenario?operationStateID=' + operationStateID + '&deliveryUnitID=' + deliveryUnitID).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            console.log(err);
            deferred.reject(err);
        });

        return deferred.promise;
    };

    function getRoutes(operationStateID, deliveryScenarioID) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteLog/FetchDeliveryRoute?operationStateID=' + operationStateID + '&deliveryScenarioID=' + deliveryScenarioID).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            console.log(err);
            deferred.reject(err);
        });

        return deferred.promise;
    };
}