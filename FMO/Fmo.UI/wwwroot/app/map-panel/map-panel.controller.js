'use strict';
angular.module('mapPanel')
    .controller('MapPanelController', ['$scope', '$rootScope', '$timeout', MapPanelController]);

function MapPanelController($scope, $rootScope, $timeout) {

    var vm = this;
    vm.color = "";
    if (!$rootScope.panelGroups) {
        $rootScope.panelGroups = {};
    }

    $scope.$on('expandPanel', function () {
        vm.collapsed = false;
    });

    vm.togglePanel = togglePanel;
    vm.hasCollapsedtext = hasCollapsedtext;
    vm.closePanel = closePanel;

    //$scope.$on('panelcolorchange', function (ev, args) {       
    //    vm.panelcolor = args.data;
    //});

    if (vm.group) {
        if (!$rootScope.panelGroups[vm.group]) {
            $rootScope.panelGroups[vm.group] = [];
        }
        $rootScope.panelGroups[vm.group].push(this);
    }

    function togglePanel() {
        if (vm.group) {
            $rootScope.panelGroups[vm.group].forEach(function (panel) {
                if (panel !== vm) {
                    panel.closePanel();
                }
            });
        }
        $timeout(function () { vm.collapsed = !vm.collapsed });
        if (vm.oncollapse)
            vm.oncollapse(vm.collapsed);
    }

    function hasCollapsedtext() {
        return (angular.isDefined(vm.collapsedtext) && vm.collapsedtext.length > 0)
    }

    function closePanel() {
        if (!vm.collapsed && vm.oncollapse)
            vm.oncollapse(vm.collapsed);
        vm.collapsed = true;
    }

    if (vm.oncreate)
        $timeout(vm.oncreate);
};