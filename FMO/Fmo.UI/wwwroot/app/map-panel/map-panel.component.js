angular.module('mapPanel')
  .component('mapPanel', {
    restrict: 'E',
    scope: {},
    transclude:true,
    bindings: {
        showheader: '<',
        paneltitle: '@',
        collapsed: '=',
        isleftpanel: '<',
        oncreate: "=",
    },
    templateUrl: './map-panel/map-panel.template.html',
    controller: 'MapPanelController as vm'
  });
