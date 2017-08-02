
angular.module('range')
  .component('range', {
      bindings: {
          selectedDeliveryUnit: "=",
          contextTitle: "="
      },
      templateUrl: './range/range.template.html',
      controller: 'rangeController as vm'
  });


