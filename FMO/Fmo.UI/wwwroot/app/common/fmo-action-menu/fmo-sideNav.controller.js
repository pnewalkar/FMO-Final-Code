
angular
    .module('fmoCommonActionMenu')
    .controller("fmoCommonActionMenuCtrl", fmoCommonActionMenuCtrl)
    function fmoCommonActionMenuCtrl ($scope, $mdSidenav) {
        $scope.openSideNavPanel = function() {
        $mdSidenav('left').open();
    };
};