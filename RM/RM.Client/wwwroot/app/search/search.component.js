angular.module('search')
  .component('search', {
      restrict: 'E',
      bindings: {
          contextTitle: "="
      },
      templateUrl: './search/search.template.html',
      controller: 'SearchController as vm'
  });
