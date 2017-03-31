angular.module('routeLog', [])
.controller('RouteLogController', ['$scope', RouteLogController])
function RouteLogController($scope) {
    $scope.RouteLogStatus();
    $scope.RouteLogStatus = function () {

        alert('Jitu');
    }

    $scope.Scenario = function () {
    }

    $scope.Routes = function () {
    }

}
