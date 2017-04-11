
angular.module('simulation')
        .service('simulationBusinessService', ['simulationAPIService',
                                                simulationBusinessService])
function simulationBusinessService(simulationAPIService) {
    var vm = this;
    
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
                group2: vm.RouteStatusObj[0].id
            };
        
        });
    }

    function loadScenario()
    {
        simulationAPIService.loadScenario().then(function (response) {
            debugger;
        });
    }

    function loadDeliveryRoute() {
        simulationAPIService.loadDeliveryRoute().then(function (response) {

        });
    }
    
    
}