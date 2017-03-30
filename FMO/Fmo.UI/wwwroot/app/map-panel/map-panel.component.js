angular.module('mapPanel')
  .component('mapPanel', {
    restrict: 'E',
    scope: {},
    transclude:true,
    bindings: {
        paneltitle: '@'
    },
    templateUrl: './map-panel/map-panel.template.html',
    controller: 'MapPanelController as vm'
  });
