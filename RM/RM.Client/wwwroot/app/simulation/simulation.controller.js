angular
    .module('simulation')
    .controller('SimulationController',
    SimulationController);

SimulationController.$inject = [
    '$stateParams',
    'simulationService',
    '$scope'
]

function SimulationController($stateParams,
                             simulationService, $scope) {
    var vm = this;
    vm.selectClass = "routeSearch md-text";
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.clearSearchTerm = clearSearchTerm;
    vm.selectionChanged = selectionChanged;

    vm.selectedDeliveryUnitObj = $stateParams;
    vm.isDeliveryRouteDisabled = true;
    vm.selectedRoute;


    function clearSearchTerm() {
        vm.searchTerm = "";
    }
    function selectedRouteStatus() {
        loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj.selectedUnit.ID);
        vm.isDeliveryRouteDisabled = true;
    }
    function loadRouteLogStatus() {
        simulationService.loadRouteLogStatus(vm.RouteStatusObj, vm.selectedRouteStatusObj).then(function (response) {
            vm.RouteStatusObj = response;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0].id;;
            loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj.selectedUnit.ID);
        });
    }
    function scenarioChange() {
        loadDeliveryRoute(vm.selectedRouteScenario.id);  
        vm.deliveryRoute = null;
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitID) {
        simulationService.loadScenario(selectedRouteStatusObj, selectedDeliveryUnitID).then(function (response) {
            if (response.length > 0) {
                vm.RouteScenario = response;
            }
            else {
                vm.RouteScenario = [];
                vm.deliveryRoute = undefined;
                vm.selectedRoute = null;
                vm.selectedRouteScenario = null;
            }
        });
    }
    function loadDeliveryRoute(deliveryScenarioID) {
        simulationService.loadDeliveryRoute(deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response;
            if (vm.deliveryRoute.length > 0) {
                vm.isDeliveryRouteDisabled = false;
            }
        });       
            vm.isDeliveryRouteDisabled = true;  
    }

    function selectionChanged(selectedRoute) {
        let valueSR = document.getElementById('selectedRouteId').innerHTML;
    }
}
