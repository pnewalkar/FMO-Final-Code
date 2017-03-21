angular.module('fmoHome',['fmoCommonHeader',
                          'fmoCommonActionMenu',
                          'fmoCommonSearch',
                          'fmoCommonUnitSelector',
                          'fmoCommonRoleSelector',
                          'fmoCommonTask',
                          'fmoLayers'
                         ])
  .component('fmoCommon', {
    restrict: 'E',
    scope: {},
    templateUrl: 'app/fmo-home.html'
  });
