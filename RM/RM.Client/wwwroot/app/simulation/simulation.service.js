﻿angular.module('simulation')
        .factory('simulationService', simulationService);

simulationService.$inject = [
    'simulationAPIService',
    '$q',
    'referencedataApiService',
    'referenceDataConstants'];

function simulationService(simulationAPIService,
                           $q,
                           referencedataApiService,
                           referenceDataConstants) {
    return {
        loadRouteLogStatus: loadRouteLogStatus,
        loadScenario: loadScenario,
        loadDeliveryRoute: loadDeliveryRoute
    };

    function loadRouteLogStatus(RouteStatusObj, selectedRouteStatusObj, selectedRouteStatusObj, selectedDeliveryUnitObj) {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.OperationalStatus.DBCategoryName).then(function (response) {
            deferred.resolve(response.listItems);
        });
        return deferred.promise;
    }

    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        var deferred = $q.defer();
        simulationAPIService.getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        var deferred = $q.defer();
        simulationAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            if (response.length > 0) {
                deferred.resolve(response);
            }
        });
        return deferred.promise;
    }
};