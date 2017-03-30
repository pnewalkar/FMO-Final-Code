angular
    .module('sideNav')
    .controller("sideNavController", sideNavController)
    function sideNavController ($scope, $mdSidenav) {
        $scope.openSideNavPanel = function() {
        $mdSidenav('left').open();
    };
};