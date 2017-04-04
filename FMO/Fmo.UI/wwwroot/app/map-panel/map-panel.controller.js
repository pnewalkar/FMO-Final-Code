'use strict';
angular.module('mapPanel')
    .controller('MapPanelController', ['$scope', '$timeout', MapPanelController]);

function MapPanelController($scope, $timeout) {

    var vm = this;
    vm.togglePanel = togglePanel;

    function togglePanel() {
        vm.collapsed = !vm.collapsed;
    }
    
    if (vm.oncreate)
        $timeout(vm.oncreate);
};