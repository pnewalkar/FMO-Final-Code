angular.module('routeLog')
.controller('RouteLogController', ['$scope', RouteLogController])
function RouteLogController($scope) {
   
    RouteLogStatus();
   
     function RouteLogStatus() {
        debugger;
        alert('Jitu');
    }

}
