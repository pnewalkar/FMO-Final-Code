angular.module('FMOApp')
.config(function ($stateProvider, $urlRouterProvider) {

    //$urlRouterProvider.otherwise("home");

    $stateProvider
        .state("home", {
            url: "/",
            templateUrl: "/app/index.html"
        })

        .state("routeLog", {
            url: "/routeLog",
            templateUrl: "/app/route-log/route-log.template.html",
            controller: "RouteLogController as vm",
            params: { selectedUnit: null, }
        })
        .state("routeSimulation", {
            url: "/routeSimulation",
            templateUrl: "/app/simulation/simulation.template.html",
            controller: "SimulationController as vm",
            params: { selectedUnit: null, }
            //resolve: {
            //    selectedUnit: ['$stateParams', function ($stateParams) {
            //        return $stateParams.selectedUnit;
            //  }]
            // }
        })
         .state('manageaccess', {
             url: '/',
             templateUrl: '/app/manage-access/manage-access.html',
             controller: 'manageAccessController as vm',
             params: { username: null, }
         })
     .state("searchDetails", {
         templateUrl: "/app/search/context.template.html",
         controller: "ContextController as vm",
         params: { selectedItem: null, }
     });
});
//.config(function ($httpProvider) {
//    $httpProvider.interceptors.push('authInterceptorService');
//    // $httpProvider.defaults.headers.get = {}
//});
//.run(function ($http) {
//    //var aValue = sessionStorage.getItem('authorizationData');
//    //var jobject = JSON.parse(aValue)
//    //if(jobject)
//    //$http.defaults.headers.common.Authorization = "Bearer " + jobject.token;
//});
