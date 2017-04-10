//angular.module('routeLog')
//.service('routeLogService', routeLogService)
//
//    function routeLogService() {
//   // var vm =this;
//    
//    this.routeLog = function() {
//    return 
//    {
//      templateUrl: './app/route-log/route-log.template.html'
//    }
//  };
//
//
//}

angular.module('routeLog').service('routeLogService', function () {

    this.routeLog = function() {
        return {
            templateUrl: './route-log/route-log.template.html',
            clickOutsideToClose:true
        }
    };
});