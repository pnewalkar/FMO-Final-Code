angular.module('simulation', [])
.controller('SimulationController', ['$scope', SimulationController])
function SimulationController($scope) {
    $scope.RouteLogStatus();
    $scope.RouteLogStatus = function () {
      //  alert('Jitu');
    }

    $scope.Scenario = function () {
    }

    $scope.Routes = function () {
    }

}
