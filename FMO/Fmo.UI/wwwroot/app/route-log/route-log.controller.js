angular
    .module('routeLog')
    .controller('RouteLogController', RouteLogController);

RouteLogController.$inject =[
        'routeLogAPIService',
        'routeLogService',
        '$mdDialog',
        'items'
];

function RouteLogController(
    routeLogAPIService,
    routeLogService,
    $mdDialog,
    items) {

    var vm = this;
    vm.loadSelectionType = loadSelectionType;
    vm.loadRouteLogStatus = loadRouteLogStatus;
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.selectionTypeChange = selectionTypeChange;
    vm.deliveryRouteChange = deliveryRouteChange;
    vm.clearSearchTerm = clearSearchTerm;
    vm.routeChange = routeChange;
    vm.selectedDeliveryUnitObj = items;
    vm.closeWindow = closeWindow;

    vm.selectedRouteStatusObj = null;
    vm.selectedRouteScenario = null;
    vm.isDeliveryRouteDisabled = true;
    vm.selectedDeliveryRoute;
    vm.deliveryRoute = null;
    vm.selectedRoute = null;
    vm.searchTerm = null;
    vm.isShowMultiSelectionRoute = false;

    function closeWindow() {
        $mdDialog.cancel();
    }

    function selectionTypeChange() {
        var type = vm.selectedRouteSelectionObj;
        vm.selectedRouteStatusObj = null;
        vm.selectedRouteScenario = null;
        vm.selectedRoute = null;
        vm.isSelectionType = false;
        vm.isRouteScenarioDisabled = true;
        vm.isDeliveryRouteDisabled = true;
        vm.isShowMultiSelectionRoute = false;
        clearDeliveryRoute();
    }

    function loadSelectionType() {
        routeLogAPIService.getSelectionType().then(function (response) {
            vm.RouteselectionTypeObj = response;
            angular.forEach(vm.RouteselectionTypeObj, function (value, key) {
                if (value.referenceDataValue === "Single")
                    vm.selectedRouteSelectionObj = value;
            });
        });
    }

    function selectedRouteStatus() {
        loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.id);
        vm.isRouteScenarioDisabled = false;
        clearDeliveryRoute();
    }

    function loadRouteLogStatus() {
        routeLogAPIService.getStatus().then(function (response) {
            vm.RouteStatusObj = response;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0];
            loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.id);
        });
    }

    function scenarioChange() {
        var type = vm.selectedRouteSelectionObj;
        if (type.referenceDataValue === "Multiple") {
            loadDeliveryRoute(vm.selectedRouteStatusObj.id, vm.selectedRouteScenario.id);
            vm.isDeliveryRouteDisabled = true;
            vm.isShowMultiSelectionRoute = true;
        } else {
            loadDeliveryRoute(vm.selectedRouteStatusObj.id, vm.selectedRouteScenario.id);
            vm.isDeliveryRouteDisabled = false;
            vm.isShowMultiSelectionRoute = false;
        }
        clearDeliveryRoute();
    }

    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        routeLogAPIService.getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            if (response.length > 0) {
                vm.RouteScenario = response;
            } else {
                vm.RouteScenario = response;
                vm.selectedRouteScenario = null;
                vm.isSelectionType = true;
                vm.selectedRoute = null;
                vm.isDeliveryRouteDisabled = true;
                vm.isShowMultiSelectionRoute = false;
            }
        });
    }

    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        routeLogAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            if (vm.selectedRouteSelectionObj.referenceDataValue === "Single") {
                vm.multiSelectiondeliveryRoute = null;
                vm.deliveryRoute = response;
            } else {
                vm.deliveryRoute = null;
                vm.multiSelectiondeliveryRoute = response;
            }
        });
    }

    function routeChange() {
        vm.selectedDeliveryRoute;
        vm.searchTerm = '';

    }

    function clearSearchTerm() {
        vm.searchTerm = '';
    }

    function deliveryRouteChange() {
        routeLogAPIService.getRouteDetailsByGUID(vm.selectedRoute.id).then(function (response) {
            vm.routeDetails = response;
        });
    }

    function clearDeliveryRoute() {
        vm.deliveryRoute = null;
        vm.routeDetails = false;
    }
}
