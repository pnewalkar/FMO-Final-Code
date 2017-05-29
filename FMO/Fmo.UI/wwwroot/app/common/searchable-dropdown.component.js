angular.module('reusableComponent')
  .component('customSearchableDropdown', {
      bindings: {
          labelText: "<",
          searchTerm: "=",
          isDeliveryRouteDisabled: "<",
          selectedRoute: "=",
          selectClass: "<",
          selectHeaderClass: "<",
          searchTerm: "=",
          deliveryRoute: "<",
          noResultFound: "<",
          selectionChanged:"&"
      },
      templateUrl: './common/searchable-dropdown.template.html',
      controller: 'CustomSearchableDropdownController as vm'
  });