﻿
angular.module('simulation')
        .factory('simulationBusinessService', simulationBusinessService);
simulationBusinessService.$inject = ['simulationAPIService'];

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
    };

    function loadRouteLogStatus() {

        simulationAPIService.getStatus().then(function (response) {
            vm.RouteStatusObj = response;
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
            vm.RouteScenario = response;
        });
    }

    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        simulationAPIService.getRoutes(operationStateID, deliveryScenarioID).then(function (response) {
            vm.deliveryRoute = response;
        });
    }


}