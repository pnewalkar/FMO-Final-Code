angular.module('search')
  .component('search', {
    restrict: 'E',
    scope: {},
    templateUrl: './search/search.template.html',
    controller: 'SearchSelectorController as vm'
  });
