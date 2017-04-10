'use strict';
angular.module('layers')
    .controller('LayerSelectorController', ['$scope', 'layersService', LayerSelectorController]);

function LayerSelectorController($scope, layersService) {
    var vm = this;
    vm.showUngrouped = showUngrouped;
    vm.getLayerData = getLayerData;
    vm.onChange = onChange;
    vm.groups = [];
    vm.ungrouped = [];
    vm.groupData;

    function getLayerData() {
        vm.groupData = layersService.getLayerData();
        vm.groups = vm.groupData.groups;
        vm.ungrouped = vm.groupData.ungroup;
    }

    function refreshLayer() {
        layersService.refreshLayer();
    }
    function onChange(changedLayer) {
        layersService.onChange(changedLayer);
    }

    function showUngrouped() {
        return layersService.showUngrouped();
    }

    function fetchDeliveryPoints() {
        return layersService.fetchDeliveryPoints();
    }

    function fetchAccessLinks() {
        return layersService.fetchAccessLinks();
    }

    vm.getLayerData();

    $scope.$on("updateLayerControl", vm.getLayerData);
};
