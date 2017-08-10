angular.module('subBuilding')
  .component('subBuilding', {
      bindings: {
          selectedDeliveryUnit: "=",
          contextTitle: "="
      },
      templateUrl: './subbuilding/subbuilding.template.html',
      controller: 'subBuildingController as vm'
  });


