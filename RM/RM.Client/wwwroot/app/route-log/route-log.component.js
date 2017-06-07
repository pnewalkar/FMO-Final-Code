angular.module('routeLog')
  .component('routeLog', {
      bindings: {
          selectedDeliveryUnit: "="
      },
    templateUrl: 'app/route-log/route-log.template.html',
    controller: 'RouteLogController as vm'
  });