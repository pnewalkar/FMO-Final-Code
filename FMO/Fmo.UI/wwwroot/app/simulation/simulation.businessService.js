
angular.module('simulation')
        .service('simulationBusinessService', ['simulationAPIService',
                                                simulationBusinessService])
function simulationBusinessService(simulationAPIService) {
    var vm = this;
    vm.Test = "ABC";
    vm.RouteStatusObj = null;
    vm.selectedRouteStatusObj = null;
    vm.RouteScenario = null;
    vm.deliveryRoute = null;
    return {
        loadRouteLogStatus: loadRouteLogStatus,
        loadScenario: loadScenario,
        loadDeliveryRoute: loadDeliveryRoute
    }

    function loadRouteLogStatus()
    {
       
        simulationAPIService.getStatus().then(function (response) {
            debugger;
            vm.RouteStatusObj = response.data;
            vm.selectedRouteStatusObj = {
                group1: vm.RouteStatusObj[0].id,
                group2: vm.RouteStatusObj[1].id
            };
            return vm.RouteStatusObj;
           // loadScenario(vm.selectedRouteStatusObj.group1, vm.selectedDeliveryUnitObj.selectedUnit.id);
        });
       
    }

    function loadScenario(operationStateID, deliveryUnitID)
    {
        simulationAPIService.getScenario(operationStateID, deliveryUnitID).then(function (response) {
            debugger;
            vm.RouteScenario = response.data;
        });
    }

    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        simulationAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response.data;
        });
    }
    
    
}