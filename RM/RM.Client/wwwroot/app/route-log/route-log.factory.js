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
        getRouteDetailsByGUID: getRouteDetailsByGUID,
        generateRouteLogSummary: generateRouteLogSummary,
        generatePdf: generatePdf
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
        $http.get(GlobalSettings.unitManagerApiUrl + getRouteLogScenarioParams).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function getRoutes(deliveryScenarioID) {
        var deferred = $q.defer();

        var getDeliveryRoutesParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryRoutes, deliveryScenarioID);
        $http.get(GlobalSettings.deliveryRouteApiUrl + getDeliveryRoutesParams).success(function (response) {
        deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    function getRouteDetailsByGUID(routeId) {
        var deferred = $q.defer();
        var getDeliveryRoutesParams = stringFormatService.Stringformat(GlobalSettings.getRouteDetailsByGUID, routeId);

        $http.get(GlobalSettings.deliveryRouteApiUrl + getDeliveryRoutesParams).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    function generateRouteLogSummary(deliveryRoute) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.routeLogApiUrl + GlobalSettings.generateRouteLogSummaryReport, deliveryRoute).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    function generatePdf(pdfFilename) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.pdfGeneratorApiUrl + GlobalSettings.getPdfreport + pdfFilename).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
}

