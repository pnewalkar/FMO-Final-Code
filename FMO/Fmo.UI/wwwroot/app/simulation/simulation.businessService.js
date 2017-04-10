
angular.module('simulation')
        .service('simulationBusinessService', ['simulationAPIService',
                                                simulationBusinessService])
function simulationBusinessService(simulationAPIService) {
    var vm = this;
    vm.RouteStatusObj = null;
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
            vm.data = {
                group1: '1',
                group2: '2'
            };
            vm.radioData = [
              { label: 'Live', value: 1 },
              { label: 'Planned', value: 2 }
            ];
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