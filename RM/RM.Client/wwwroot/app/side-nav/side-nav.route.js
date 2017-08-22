angular.module('RMApp')
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
            params: { selectedUnit: null }
        })
        .state("routeSimulation", {
            url: "/routeSimulation",
            templateUrl: "./simulation/simulation.template.html",
            controller: "SimulationController as vm",
            params: { selectedUnit: null }
        })
        .state("deliveryPoint", {
            url: "/deliveryPoint",
            templateUrl: "./delivery-point/delivery-point.context.template.html",
            controller: 'DeliveryPointController as vm',
            params: { selectedUnit: null, deliveryPointList: null, positionedDeliveryPointList: null, positionedThirdPartyDeliveryPointList: null, hide: null }
        })
         .state('manageaccess', {
             url: '/',
             templateUrl: '/app/manage-access/manage-access.html',
             controller: 'ManageAccessController as vm',
             params: { username: null }
         })
        .state("searchDetails", {
            templateUrl: "./search/context.template.html",
            controller: "ContextController as vm",
            params: { selectedItem: null }
        })
        .state("deliveryPointDetails", {
            templateUrl: "./delivery-point/delivery-point-details.template.html",
            controller: "DeliveryPointContextController as vm",
            params: { selectedDeliveryPoint: null }
        })
        .state("referenceData", {
            templateUrl: "./reference-data/reference-data.template.html",
            controller: "ReferenceDataController as vm"
        })
          .state("accessLink", {
              url: "/accessLink",
              templateUrl: "./access-link/acccess-link.template.html",
              controller: 'AccessLinkController as vm',
              params: { accessLinkFeature: null }
          })
          .state("deliveryPointGroup", {
              url: "/group",
              templateUrl: "./group/group.template.html",
              controller: 'GroupController as vm'
          })    
          .state("deliveryPointGroupDetails", {
                url: "/groupdetails",
                templateUrl: "./group/group-details.template.html",
                controller: 'GroupDetailsController as vm'
    });

});
