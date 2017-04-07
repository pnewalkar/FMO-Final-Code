angular.module('unitSelector')
  .component('unitSelector', {
      restrict: 'E',
      scope: {
          selectedDeliveryUnit: "="
      },
      templateUrl: './unit-selector/unit-selector.template.html',
      controller: 'UnitSelectorController as vm'
  });

