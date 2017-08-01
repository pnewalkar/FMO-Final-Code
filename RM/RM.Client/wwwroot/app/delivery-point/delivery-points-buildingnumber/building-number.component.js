angular.module('buildingNumber')
  .component('buildingNumber', {
      bindings: {
          selectedDeliveryUnit: "=",
          contextTitle: "="
      },
      templateUrl: './building-number/building-number.template.html',
      controller: 'buildingNumberController as vm'
  });


