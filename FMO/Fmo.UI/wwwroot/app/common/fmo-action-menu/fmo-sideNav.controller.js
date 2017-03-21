
angular
  .module('fmoCommonActionMenu')


  .controller('AppCtrl', function ($scope, $mdSidenav) {
    $scope.openSideNavPanel = function() {
        $mdSidenav('left').open();
    };
});