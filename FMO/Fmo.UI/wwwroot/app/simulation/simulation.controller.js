angular.module('simulation')
.controller('SimulationController', ['$scope', '$state', '$stateParams', 'simulationBusinessService', 'simulationAPIService', SimulationController])
function SimulationController($scope, $state, $stateParams, simulationBusinessService, simulationAPIService) {

    var vm = this;
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.selectedDeliveryUnitObj = $stateParams;
    vm.selectedRouteStatusObj = null;
    vm.selectedRouteScenario = null;
    vm.isDeliveryRouteDisabled = true;
    vm.selectedDeliveryRoute = null;
    vm.deliveryRoute = null;
    vm.selectedVegetables;
    vm.searchTerm;

    function selectedRouteStatus() {

        loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj.selectedUnit.id);
    }
    function loadRouteLogStatus() {
        simulationAPIService.getStatus().then(function (response) {
            vm.RouteStatusObj = response;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0].id;
            //vm.selectedRouteStatusObj = {
            //    group1: vm.RouteStatusObj[0].id,
            //    group2: vm.RouteStatusObj[1].id
            //};

            loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj.selectedUnit.id);
        });
    }
    function scenarioChange() {

        loadDeliveryRoute(vm.selectedRouteStatusObj, vm.selectedRouteScenario.id);
        vm.isDeliveryRouteDisabled = false;
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        simulationAPIService.getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            if (response.length > 0) {
                vm.RouteScenario = response;
            } else {
                vm.RouteScenario = undefined;
                vm.deliveryRoute = undefined;
            }
        });
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        simulationAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            if (response.length > 0) {
                vm.deliveryRoute = response;
            } else { vm.deliveryRoute = undefined; }
        });
    }
}
