'use strict';
angular
    .module('mapPanel')
    .controller('MapPanelController', MapPanelController);

MapPanelController.$inject = [
        '$timeout',
];

function MapPanelController(
    $timeout) {

    var vm = this;
    vm.initialize = initialize;
    vm.togglePanel = togglePanel;

    vm.initialize();
    function togglePanel() {
        vm.collapsed = !vm.collapsed;
    }
    
    function initialize() {
        if (vm.oncreate)
            $timeout(vm.oncreate);
    }
};