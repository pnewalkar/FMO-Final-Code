angular
    .module('sideNav')
    .controller("sideNavController", ['$stateParams', '$state', '$scope', '$mdSidenav', sideNavController])
function sideNavController($stateParams, $state, $scope, $mdSidenav) {
    debugger;
    $scope.toggleSideNav = function () {
        $mdSidenav('left').toggle();
    };
    var vm = this;
    vm.routeSimulation = routeSimulation;


    

    function routeSimulation(selectedDeliveryUnit) {
        debugger;
        $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
    }

}