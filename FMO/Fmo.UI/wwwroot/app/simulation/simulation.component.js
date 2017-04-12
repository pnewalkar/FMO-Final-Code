angular.module('simulation')
  .component('simulation', {
    bindings: {
        selectedDeliveryUnit:"="
    },
    templateUrl: './simulation/simulation.template.html',
    controller: 'SimulationController as vm'
  });
