angular.module('routeLog')
    .factory('routeLogService', routeLogService);
routeLogService.$inject = ['$q',
                           '$mdDialog',
                           'routeLogAPIService',
                           'CommonConstants',
                            'referencedataApiService',
                            'referenceDataConstants'];

function routeLogService(
$q,
$mdDialog,
routeLogAPIService,
CommonConstants,
referencedataApiService,
referenceDataConstants) {
    return {
        closeWindow: closeWindow,
        loadSelectionType: loadSelectionType,
        loadRouteLogStatus: loadRouteLogStatus,
        scenarioChange: scenarioChange,
        loadScenario: loadScenario,
        loadDeliveryRoute: loadDeliveryRoute,
        deliveryRouteChange: deliveryRouteChange,
        generateRouteLogSummary: generateRouteLogSummary,
        generatePdf: generatePdf
    };
    function closeWindow() {
        $mdDialog.cancel();
    }
    function loadSelectionType() {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.UI_RouteLogSearch_SelectionType.DBCategoryName).then(function (response) {
            var selectionTypeResult = [];
            angular.forEach(response.listItems, function (value, key) {
                if (value.value === CommonConstants.RouteLogSelectionType.Single)
                    selectionTypeResult.push({ "RouteselectionTypeObj": response.listItems, "selectedRouteSelectionObj": value });
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
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.SenarioOperationState.AppCategoryName).then(function (response) {
            deferred.resolve(response.listItems);
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
    function loadDeliveryRoute(deliveryScenarioID, selectionType) {
        var deferred = $q.defer();
        routeLogAPIService.getRoutes(deliveryScenarioID, selectionType).then(function (response) {
            var deliveryRouteResult = [];
            if (selectionType === CommonConstants.RouteLogSelectionType.Single) {
                deliveryRoute = response;
                multiSelectiondeliveryRoute = null;
                deliveryRouteResult.push({ "deliveryRoute": response, "multiSelectiondeliveryRoute": [] });
                deferred.resolve(deliveryRouteResult);
            } else {
                multiSelectiondeliveryRoute = response;
                deliveryRoute = null;
                deliveryRouteResult.push({ "deliveryRoute": [], "multiSelectiondeliveryRoute": response });
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
    function generateRouteLogSummary(deliveryRoute) {
        var deferred = $q.defer();
        routeLogAPIService.generateRouteLogSummary(deliveryRoute).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    };
    function generatePdf(pdfFilename) {
        var deferred = $q.defer();
        routeLogAPIService.generatePdf(pdfFilename).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    };
}