angular.module('reusableComponent')
  .component('customSearchableDropdown', {
      bindings: {
          labelText: "<",
          placeHolder:"<",
          searchTerm: "=",
          isDeliveryRouteDisabled: "<",
          selectedRoute: "=",
          selectClass: "<",
          selectHeaderClass: "<",
          searchTerm: "=",
          deliveryRoute: "<",
          noResultFound: "<"
      },
      templateUrl: './common/searchable-dropdown.template.html',
      controller: 'CustomSearchableDropdownController as vm'
  });