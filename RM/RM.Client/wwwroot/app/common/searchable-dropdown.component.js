angular
    .module('reusableComponent')
    .component('customSearchableDropdown', {
     bindings: {
         headerText: "@",
         searchPlaceholder: "@",
         orderBy: "@",
         items: "=",
         disabled: "=",
         selectedItem: "=",
         onSelectedItem:"&",
         searchInputIcon: "@"
     },
     templateUrl: './common/searchable-dropdown.template.html',
     controller: 'CustomSearchableDropdownController as vm'
 });