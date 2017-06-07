angular.module('reusableComponent')
  .component('customDropdown', {
      bindings: {
          
      },
      templateUrl: './common/dropdown.template.html',
      controller: 'customDropdownController as vm'
  });