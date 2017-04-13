angular.module('routeLog')
.controller('RouteLogController', ['$scope', '$state', '$stateParams','routeLogAPIService','routeLogService', RouteLogController])
function RouteLogController($scope, $state, $stateParams, routeLogAPIService, routeLogService) {
    var vm = this;
    vm.loadSelectionType = loadSelectionType();
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.clearSearchTerm = clearSearchTerm;
    vm.routeChange = routeChange;
    vm.selectedDeliveryUnitObj = $stateParams;
    vm.selectedRouteStatusObj = null;
    vm.selectedRouteScenario = null;
    vm.isDeliveryRouteDisabled = true;
    vm.selectedDeliveryRoute;
    vm.deliveryRoute = null;
   
    vm.selectedVegetables;
    vm.searchTerm;
   
    function selectionTypeChange()
    {

    }
    function loadSelectionType()
    {
        routeLogAPIService.getSelectionType().then(function (response) {
            debugger;
            vm.RouteselectionTypeObj = response.data;
            vm.selectedRouteSelectionObj = vm.RouteselectionTypeObj[0];
        });
    }
    function selectedRouteStatus() {
        debugger;
        //loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj);
    }
    function loadRouteLogStatus() {
        routeLogAPIService.getStatus().then(function (response) {
            debugger;
            vm.RouteStatusObj = response.data;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0];
            loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.selectedUnit.id);
        });
    }
    function scenarioChange() {
        loadDeliveryRoute(vm.selectedRouteStatusObj.id, vm.selectedRouteScenario.id);
        vm.isDeliveryRouteDisabled = false;
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        routeLogAPIService.getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            debugger;
            vm.RouteScenario = response.data;
        });
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        routeLogAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response.data;
        });
        //simulationBusinessService.loadDeliveryRoute(selectedRouteStatusObj, selectedRouteScenario);
    }
    function routeChange()
    {
        vm.selectedDeliveryRoute;
        debugger;
        vm.searchTerm = '';

    }
    function clearSearchTerm () {
        vm.searchTerm = '';
    };
    
}
