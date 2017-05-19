angular.module('sideNav')
  .component('sideNav', {
      bindings: {
          selectedDeliveryUnit: "=",
          contextTitle: "="
      },
      templateUrl: './side-nav/side-nav.template.html',
      controller: 'SideNavController as vm'
  });


