
// Just for reference
angular.module('simulation')
    .factory('simulationAPIService', simulationAPIService);

simulationAPIService.$inject = ['$http', '$q', 'GlobalSettings', 'stringFormatService'];

function simulationAPIService($http, $q, GlobalSettings, stringFormatService) {

    return {
        getStatus: getStatus,
        getScenario: getScenario,
        getRoutes: getRoutes
    };

    function getStatus() {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getRouteLogStatus).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function getScenario(operationStateID, deliveryUnitID) {
        var deferred = $q.defer();


        var getRouteSimulationScenarioParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryRouteScenario, operationStateID, deliveryUnitID);
        $http.get(GlobalSettings.unitManagerApiUrl + getRouteSimulationScenarioParams).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function getRoutes(operationStateID, deliveryScenarioID) {
        var deferred = $q.defer();

        var getDeliveryRoutesParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryRoutes, operationStateID, deliveryScenarioID);
        $http.get(GlobalSettings.deliveryRouteApiUrl + getDeliveryRoutesParams).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }
}