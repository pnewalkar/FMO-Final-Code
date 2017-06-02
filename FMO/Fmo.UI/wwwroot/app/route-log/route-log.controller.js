angular
    .module('routeLog')
    .controller('RouteLogController',
                 RouteLogController);
RouteLogController.$inject=['routeLogAPIService',
                            'routeLogService',
                            'items','$scope'];

function RouteLogController(routeLogAPIService,
                            routeLogService,                           
                            items, $scope) {
    var vm = this;
    vm.initialize = initialize;
    vm.loadSelectionType = loadSelectionType;
    vm.loadRouteLogStatus = loadRouteLogStatus;
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.selectionTypeChange = selectionTypeChange;
    vm.deliveryRouteChange = deliveryRouteChange;
    vm.clearSearchTerm = clearSearchTerm;
    vm.selectedDeliveryUnitObj = items;
    vm.closeWindow = closeWindow;
    vm.selectClass = "routeSearch md-text";
    vm.initialize();
    function initialize() {
        vm.loadSelectionType();
        vm.loadRouteLogStatus();
    }

    $scope.$on('selectionChanged', function (event, args) {
        let selectedRoute = args.selectedRoute;
        deliveryRouteChange(selectedRoute.id);
    });

    function closeWindow() {
        routeLogService.closeWindow();
    }
    function selectionTypeChange() {
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
        routeLogService.loadSelectionType().then(function (response) {
            vm.RouteselectionTypeObj = response[0].RouteselectionTypeObj;
            vm.selectedRouteSelectionObj = response[0].selectedRouteSelectionObj;
        })
    }
    function selectedRouteStatus() {
        loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.id);
        vm.isRouteScenarioDisabled = false;
        clearDeliveryRoute();
    }
    function loadRouteLogStatus() {
        routeLogService.loadRouteLogStatus().then(function (response) {
            vm.RouteStatusObj = response;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0];
            loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.id);
        });
    }
    function scenarioChange() {       
        var result = routeLogService.scenarioChange(vm.selectedRouteSelectionObj.referenceDataValue);
        vm.isDeliveryRouteDisabled= result.isDeliveryRouteDisabled;
        vm.isShowMultiSelectionRoute=result.isShowMultiSelectionRoute;
        loadDeliveryRoute(vm.selectedRouteStatusObj.id, vm.selectedRouteScenario.id);
        clearDeliveryRoute();
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        routeLogService.loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            if (response.length > 0) {
                vm.RouteScenario = response;
            } else {
                vm.RouteScenario = undefined;
                vm.selectedRouteScenario = null;
                vm.isSelectionType = true;
                vm.selectedRoute = null;
                vm.isDeliveryRouteDisabled = true;
                vm.isShowMultiSelectionRoute = false;
            }
        });
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        routeLogService.loadDeliveryRoute(operationStateID, deliveryScenarioID, vm.selectedRouteSelectionObj.referenceDataValue).then(function (response) {
            vm.multiSelectiondeliveryRoute = response[0].multiSelectiondeliveryRoute;
            vm.deliveryRoute = response[0].deliveryRoute; 
        });
    }
    function clearSearchTerm() {
        vm.searchTerm = '';
    }
    function deliveryRouteChange(selectedRouteValue) {
        routeLogService.deliveryRouteChange(selectedRouteValue).then(function (response) {
            vm.routeDetails = response;
        });
    }
    function clearDeliveryRoute() {
        vm.deliveryRoute = null;
        vm.routeDetails = false;
    }
}
