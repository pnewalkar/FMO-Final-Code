angular.module('unitSelector')
  .component('unitSelector', {
      bindings: {
          selectedDeliveryUnit: "="
      },
      templateUrl: './unit-selector/unit-selector.template.html',
      controller: 'UnitSelectorController as vm'
      
  });

