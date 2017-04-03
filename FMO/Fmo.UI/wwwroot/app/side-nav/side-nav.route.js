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
            controller: "SimulationController as vm"
        });
});
