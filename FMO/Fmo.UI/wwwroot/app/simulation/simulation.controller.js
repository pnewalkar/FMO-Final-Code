angular
    .module('simulation')
    .controller('SimulationController',
    SimulationController);

SimulationController.$inject = [
    '$stateParams',
    'simulationService'
    ]

function SimulationController($stateParams,
                             simulationService) {
    var vm = this;
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.clearSearchTerm = clearSearchTerm;

    vm.selectedDeliveryUnitObj = $stateParams;
    vm.isDeliveryRouteDisabled = true;

    function clearSearchTerm() {
        vm.searchTerm = "";
    }
    function selectedRouteStatus() {
        loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj.selectedUnit.id);
        vm.isDeliveryRouteDisabled = true;
    }
    function loadRouteLogStatus() {
        simulationService.loadRouteLogStatus(vm.RouteStatusObj, vm.selectedRouteStatusObj).then(function(response) {
            vm.RouteStatusObj = response;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0].id;;
            loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj.selectedUnit.id);
        });
    }
    function scenarioChange() {
        loadDeliveryRoute(vm.selectedRouteStatusObj, vm.selectedRouteScenario.id);
        vm.isDeliveryRouteDisabled = false;
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitID) {
        simulationService.loadScenario(selectedRouteStatusObj, selectedDeliveryUnitID).then(function (response) {
            if (response.length > 0) {
                vm.RouteScenario = response;
            }
            else {
                vm.RouteScenario = undefined;
                vm.deliveryRoute = undefined;
                vm.selectedRoute = null;
                vm.selectedRouteScenario = null;
            }
        });
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        simulationService.loadDeliveryRoute(operationStateID, deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response;
        });
    }
}
