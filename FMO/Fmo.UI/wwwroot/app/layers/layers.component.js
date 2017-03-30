angular.module('layers')
  .component('layers', {
      restrict: 'E',
      scope: {},
      templateUrl: './layers/layers.template.html',
      controller: 'LayerSelectorController as vm'
  });