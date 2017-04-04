angular.module('simulation')
.controller('SimulationController', ['$scope', '$state', '$stateParams', SimulationController])
function SimulationController($scope, $state, $stateParams) {
    debugger;
    var vm = this;
    vm.data = {
        group1: 'live',
        group2: '2'
    };

    $scope.$state = $state;
    $scope.$stateParams = $stateParams;
    
     function RouteLogStatus() {
      //  alert('Jitu');
    }




}
