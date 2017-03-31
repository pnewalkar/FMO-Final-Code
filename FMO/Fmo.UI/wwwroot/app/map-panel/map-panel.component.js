angular.module('mapPanel')
  .component('mapPanel', {
    restrict: 'E',
    scope: {},
    transclude:true,
    bindings: {
        paneltitle: '@',
        oncreate: "="
    },
    templateUrl: './map-panel/map-panel.template.html',
    controller: 'MapPanelController as vm'
  });
