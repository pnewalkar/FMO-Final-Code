angular.module('routeLog')
    .factory('routeLogAPIService', routeLogAPIService);

routeLogAPIService.$inject = ['$http',
                              '$q',
                              'GlobalSettings',
'stringFormatService'];

function routeLogAPIService($http,
                            $q,
                            GlobalSettings,
                            stringFormatService) {

    return {
        getSelectionType: getSelectionType,
        getStatus: getStatus,
        getScenario: getScenario,
        getRoutes: getRoutes,
        getRouteDetailsByGUID: getRouteDetailsByGUID
    }

    function getSelectionType() {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getRouteLogSelectionType).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

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

        var getRouteLogScenarioParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryRouteScenario, operationStateID, deliveryUnitID);
        $http.get(GlobalSettings.apiUrl + getRouteLogScenarioParams).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function getRoutes(operationStateID, deliveryScenarioID) {
        var deferred = $q.defer();

        var getDeliveryRoutesParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryRoutes, operationStateID, deliveryScenarioID);
        $http.get(GlobalSettings.apiUrl + getDeliveryRoutesParams).success(function (response) {
        deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    function getRouteDetailsByGUID(routeId) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getRouteDetailsByGUID + routeId).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

   
}

