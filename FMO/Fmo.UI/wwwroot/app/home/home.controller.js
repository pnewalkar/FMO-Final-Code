angular.module('home')
    .controller('HomeController', ['$scope', '$mdSidenav', HomeController]);
function HomeController($scope, $mdSidenav) {
    var vm = this;
    vm.deliveryUnit = vm.selectedDeliveryUnit;
    var title = vm.contextTitle;
    vm.toggleSideNav = toggleSideNav;

    function toggleSideNav() {
        $mdSidenav('left').toggle();
    }
};