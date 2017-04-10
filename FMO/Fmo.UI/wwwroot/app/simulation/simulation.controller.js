angular.module('simulation')
.controller('SimulationController', ['$scope', '$state', '$stateParams', 'simulationBusinessService', SimulationController])
function SimulationController($scope, $state, $stateParams, simulationBusinessService) {
    debugger;
    var vm = this;
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.selectedRouteStatus = selectedRouteStatus; 
    vm.selectedDeliveryUnitObj= $stateParams;
    vm.selectedRouteStatusObj = null;

    function selectedRouteStatus() {
        debugger;      
        loadScenario(vm.selectedRouteStatusObj, vm.selectedDeliveryUnitObj);
       
    }
    function loadRouteLogStatus(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        simulationBusinessService.loadRouteLogStatus();
    }

    function loadScenario() {
        simulationBusinessService.loadScenario();
    }
    function loadDeliveryRoute() {
        simulationBusinessService.loadDeliveryRoute();
    }

}
