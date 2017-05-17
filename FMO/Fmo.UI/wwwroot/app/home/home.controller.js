angular
    .module('home')
    .controller('HomeController', [
        '$mdSidenav',
        HomeController]);

function HomeController(
    $mdSidenav) {

    var vm = this;
    vm.toggleSideNav = toggleSideNav;

    function toggleSideNav() {
        $mdSidenav('left').toggle();
    }
};