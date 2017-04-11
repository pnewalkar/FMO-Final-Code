angular.module('FMOApp')
.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/home");
    $stateProvider
        .state("routeLog", {
            url: "/routeLog",
            templateUrl: "/app/route-log/route-log.template.html",
            controller: "RouteLogController as vm"
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
     .state("searchDetails", {
         templateUrl: "/app/search/context.template.html",
         controller: "ContextController as vm",
         params: { selectedItem: null, }
     });
});
