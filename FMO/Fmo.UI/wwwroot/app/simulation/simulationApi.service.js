
// Just for reference
angular.module('simulation')
    .factory('simulationAPIService', simulationAPIService);

simulationAPIService.$inject = ['$http', '$q', 'GlobalSettings'];

function simulationAPIService($http, $q, GlobalSettings) {

    return {
        getStatus: getStatus,
        getScenario: getScenario,
        getRoutes: getRoutes
    };

    function getStatus() {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteSimulation/RouteLogsStatus').success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function getScenario(operationStateID, deliveryUnitID) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteSimulation/FetchDeliveryScenario?operationStateID=' + operationStateID + '&deliveryUnitID=' + deliveryUnitID).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function getRoutes(operationStateID, deliveryScenarioID) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/RouteSimulation/FetchDeliveryRoute?operationStateID=' + operationStateID + '&deliveryScenarioID=' + deliveryScenarioID).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }
}