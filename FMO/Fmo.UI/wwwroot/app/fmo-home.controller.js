angular.module('fmoCommonHome',['fmoCommonHeader',
                          'fmoCommonActionMenu',
                          'fmoCommonSearch',
                          'fmoCommonUnitSelector',
                          'fmoCommonRoleSelector',
                          'fmoCommonTask',
                          'fmoContext',
                          'fmoMiniMap',
                          'fmoMapKeys',
                          'fmoLayers'
                         ])
  .component('fmoHome', {
    restrict: 'E',
    scope: {},
    templateUrl: 'app/fmo-home.html'
  });
