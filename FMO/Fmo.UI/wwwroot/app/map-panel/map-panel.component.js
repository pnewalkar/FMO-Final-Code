angular.module('mapPanel')
  .component('mapPanel', {
    restrict: 'E',
    transclude: true,
    bindings: {
        showheader: '<',
        paneltitle: '@',
        collapsedtext: '@',
        collapsed: '=',
        isleftpanel: '<',
        oncreate: "=",
        oncollapse: "=",
        group: "@",
        panelcolor: "@"
    },
    templateUrl: './map-panel/map-panel.template.html',
    controller: 'MapPanelController as vm'
  });
