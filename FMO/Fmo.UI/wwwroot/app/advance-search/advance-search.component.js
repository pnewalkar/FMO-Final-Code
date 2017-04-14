angular.module('advanceSearch')
  .component('advanceSearch', {
    restrict: 'E',
    scope: {},
    bindings: {
        searchText: "="
    },
    templateUrl: './advance-search/advance-search.template.html',
    controller: 'advanceSearchController1 as vm'
  });