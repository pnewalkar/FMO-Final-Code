angular.module('routeLog')
    .service('routeLogService',['routeLogAPIService',routeLogService])
function routeLogService(routeLogAPIService) {
    var vm = this;
    vm.RouteStatusObj = null;
    vm.selectedRouteStatusObj = null;
    vm.RouteScenario = null;
    vm.deliveryRoute = null;
    return {
        routeLog: routeLog,
        loadRouteLogStatus: loadRouteLogStatus,
        loadScenario: loadScenario,
        loadDeliveryRoute: loadDeliveryRoute
    };

    function loadRouteLogStatus() {
        simulationAPIService.getStatus().then(function (response) {            
            vm.RouteStatusObj = response.data;
            vm.selectedRouteStatusObj = {
                group1: vm.RouteStatusObj[0].id,
                group2: vm.RouteStatusObj[1].id
            };
            return vm.RouteStatusObj;
            // loadScenario(vm.selectedRouteStatusObj.group1, vm.selectedDeliveryUnitObj.selectedUnit.id);
        });

    }

    function loadScenario(operationStateID, deliveryUnitID) {
        simulationAPIService.getScenario(operationStateID, deliveryUnitID).then(function (response) {            
            vm.RouteScenario = response.data;
        });
    }

    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        simulationAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response.data;
        });
    }

    function routeLog (selectedUnit){
        return {
            templateUrl: './route-log/route-log.template.html',
            clickOutsideToClose: true,
            controller: 'RouteLogController as vm',
            params: { selectedUnit: selectedUnit, }
        }
    }
}