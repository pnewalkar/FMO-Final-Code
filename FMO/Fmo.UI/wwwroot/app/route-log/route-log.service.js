angular.module('routeLog')
    .factory('routeLogService', routeLogService);
routeLogService.$inject = ['$q',
                           '$mdDialog',
                           'routeLogAPIService',
                           'CommonConstants'];

function routeLogService(
$q,
$mdDialog,
routeLogAPIService,
CommonConstants) {
    return {
        closeWindow: closeWindow,
        loadSelectionType: loadSelectionType,
        loadRouteLogStatus: loadRouteLogStatus,
        scenarioChange: scenarioChange,
        loadScenario: loadScenario,
        loadDeliveryRoute: loadDeliveryRoute,
        deliveryRouteChange: deliveryRouteChange
    };
    function closeWindow() {
        $mdDialog.cancel();
    }
    function loadSelectionType() {
        var deferred = $q.defer();
        routeLogAPIService.getSelectionType().then(function (response) {
            var selectionTypeResult = [];
            angular.forEach(response, function (value, key) {
                if (value.referenceDataValue === CommonConstants.RouteLogSelectionType.Single)
                    selectionTypeResult.push({ "RouteselectionTypeObj": response, "selectedRouteSelectionObj": value });
                deferred.resolve(selectionTypeResult);
            });
        });
        return deferred.promise;
    }
    function selectedRouteStatus() {
        loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.id);
        vm.isRouteScenarioDisabled = false;
        clearDeliveryRoute();
    }
    function loadRouteLogStatus() {
        var deferred = $q.defer();
        routeLogAPIService.getStatus().then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }
    function scenarioChange(selectionType) {
        var isDeliveryRouteDisabled = false;
        var isShowMultiSelectionRoute = false;
        if (selectionType === CommonConstants.RouteLogSelectionType.Multiple) {
            return {
                isDeliveryRouteDisabled: true,
                isShowMultiSelectionRoute: true
            };
        } else {
            return {
                isDeliveryRouteDisabled: false,
                isShowMultiSelectionRoute: false
            };
        }
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        var deferred = $q.defer();
        routeLogAPIService.getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID,selectionType) {
        var deferred = $q.defer();
        routeLogAPIService.getRoutes(operationStateID, deliveryScenarioID, selectionType).then(function (response) {
            var deliveryRouteResult = [];
            if (selectionType === CommonConstants.RouteLogSelectionType.Single) {
                deliveryRoute = response;
                multiSelectiondeliveryRoute = null;               
                deliveryRouteResult.push({ "deliveryRoute": response, "multiSelectiondeliveryRoute": null });
                deferred.resolve(deliveryRouteResult);
            } else {
                multiSelectiondeliveryRoute = response;
                deliveryRoute = null;               
                deliveryRouteResult.push({ "deliveryRoute": null, "multiSelectiondeliveryRoute": response });
                deferred.resolve(deliveryRouteResult);
            }
        });
        return deferred.promise;
    }
    function deliveryRouteChange(selectedRouteID) {
        var deferred = $q.defer();
        routeLogAPIService.getRouteDetailsByGUID(selectedRouteID).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }
}