angular
    .module('sideNav')
    .controller("sideNavController", sideNavController)
    function sideNavController ($scope, $mdSidenav) {
        $scope.toggleSideNav = function() {
        $mdSidenav('left').toggle();
    };
};