angular.module('sideNav')
  .component('sideNav', {
      bindings: {
          selectedDeliveryUnit: "=",
          contextTitle: "="
      },
      templateUrl: './side-nav/side-nav.template.html',
      controller: 'sideNavController as vm'
  });


