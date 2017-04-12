angular.module('simulation')
.controller('SimulationController', ['$scope', '$state', '$stateParams', 'simulationBusinessService', 'simulationAPIService', SimulationController])
function SimulationController($scope, $state, $stateParams, simulationBusinessService, simulationAPIService) {
    debugger;
    var vm = this;
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.selectedDeliveryUnitObj= $stateParams;
    vm.selectedRouteStatusObj = null;
    vm.selectedRouteScenario = null;
    vm.isDeliveryRouteDisabled = true;
    vm.selectedDeliveryRoute = null;
    vm.deliveryRoute = null;
    function selectedRouteStatus() {
        debugger;
        //loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj);
    }
    function loadRouteLogStatus() {
        simulationAPIService.getStatus().then(function (response) {
            debugger;
            vm.RouteStatusObj = response.data;
            vm.selectedRouteStatusObj = {
                group1: vm.RouteStatusObj[0].id,
                group2: vm.RouteStatusObj[1].id
            };
           
            loadScenario(vm.selectedRouteStatusObj.group1, vm.selectedDeliveryUnitObj.selectedUnit.id);
        });
    }
    function scenarioChange()
    {
        loadDeliveryRoute(vm.selectedRouteStatusObj.group1, vm.selectedRouteScenario.id);
        vm.isDeliveryRouteDisabled = false;
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        simulationAPIService.getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            debugger;
            vm.RouteScenario = response.data;           
        });
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        simulationAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response.data;
        });
        //simulationBusinessService.loadDeliveryRoute(selectedRouteStatusObj, selectedRouteScenario);
    }
}
