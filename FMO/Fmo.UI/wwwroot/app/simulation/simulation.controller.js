angular.module('simulation')
.controller('SimulationController', ['$scope', '$state', '$stateParams', 'simulationBusinessService', SimulationController])
function SimulationController($scope, $state, $stateParams, simulationBusinessService) {
    debugger;
    var vm = this;
    vm.loadRouteLogStatus = loadRouteLogStatus();
    vm.loadScenario = loadScenario;
    vm.selectedStatus = selectedStatus; 
    $scope.$state = $state;
    $scope.$stateParams = $stateParams;


    function selectedRouteStatus() {
        debugger;
         vm.data.group1;
    }
    function  loadRouteLogStatus()
    {
        vm.data = {
            group1: '1',
            group2: '2'
        };
        vm.radioData = [
          { label: 'Live', value: 1 },
          { label: 'Planned', value: 2 }
        ];
    simulationBusinessService.loadRouteLogStatus();
    }

    function loadScenario()
    {
        simulationBusinessService.loadScenario();
    }
    function loadDeliveryRoute() {
        simulationBusinessService.loadDeliveryRoute();
    }
   
}
