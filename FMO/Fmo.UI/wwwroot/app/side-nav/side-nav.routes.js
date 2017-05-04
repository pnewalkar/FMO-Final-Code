angular.module('FMOApp')
.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise("/");

    $stateProvider
        .state("/", {
            url: "/",
            template: ''
        })

        .state("routeLog", {
            url: "/routeLog",
            templateUrl: "./route-log/route-log.template.html",
            controller: "RouteLogController as vm",
            params: { selectedUnit: null, }
        })
        .state("routeSimulation", {
            url: "/routeSimulation",
            templateUrl: "./simulation/simulation.template.html",
            controller: "SimulationController as vm",
            params: { selectedUnit: null, }
        })
        .state("deliveryPoint", {
            url: "/deliveryPoint",
            templateUrl: "./delivery-point/delivery-point.context.template.html",
            controller: 'DeliveryPointController as vm',
            params: { selectedUnit: null, }
        })
         .state('manageaccess', {
             url: '/',
             templateUrl: '/app/manage-access/manage-access.html',
             controller: 'manageAccessController as vm',
             params: { username: null, }
         })
        .state("searchDetails", {
            templateUrl: "./search/context.template.html",
            controller: "ContextController as vm",
            params: { selectedItem: null, }
        })
        .state("referenceData", {
            templateUrl: "./reference-data/reference-data.templete.html",
            controller: "ReferenceDataController as vm"
        });
  
})
.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});