angular
    .module('home')
    .controller('HomeController', [
        '$mdSidenav', '$scope', 'errorService',
        HomeController]);

function HomeController(
    $mdSidenav,$scope, errorService) {

    var vm = this;
    vm.toggleSideNav = toggleSideNav;
    vm.lastOpenedDialog = "";

    function toggleSideNav() {
        $mdSidenav('left').toggle();
    }

    $scope.$on('dialogOpen', function (event, args) {
        vm.lastOpenedDialog = args;

    });

    $scope.$on('dialogClosed', function (event, args) {
        vm.lastOpenedDialog = "";

    });

    $scope.$on('errorClosed', function (event, args) {
        if (vm.lastOpenedDialog != "") {
            $scope.$broadcast("showDialog", vm.lastOpenedDialog);
        }

    });
    $scope.$on("showError", function (event, args) {
        errorService.openAlert(args);

    })
};