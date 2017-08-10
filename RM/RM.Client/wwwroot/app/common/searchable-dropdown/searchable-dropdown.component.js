angular
    .module('searchableDropdown')
    .component('searchableDropdown', {
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
     templateUrl: './common/searchable-dropdown/searchable-dropdown.template.html',
     controller: 'SearchableDropdownController as vm'
 });