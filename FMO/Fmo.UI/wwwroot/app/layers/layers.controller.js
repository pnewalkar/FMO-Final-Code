'use strict';
angular.module('layers')
    .controller('LayerSelectorController', ['$scope', 'layersBusinessService', LayerSelectorController]);

function LayerSelectorController($scope, layersBusinessService) {
    var vm = this;
    vm.showUngrouped = showUngrouped;
    vm.getLayerData = getLayerData;
    vm.onChange = onChange;
    vm.groups = [];
    vm.ungrouped = [];
    vm.groupData;

    function getLayerData() {
        vm.groupData = layersBusinessService.getLayerData();
        vm.groups = vm.groupData.groups;
        vm.ungrouped = vm.groupData.ungroup;
    }

    function refreshLayer() {
        layersBusinessService.refreshLayer();
    }
    function onChange(changedLayer) {
        layersBusinessService.onChange(changedLayer);
    }

    function showUngrouped() {
        return layersBusinessService.showUngrouped();
    }

    function fetchDeliveryPoints() {
        return layersBusinessService.fetchDeliveryPoints();
    }

    function fetchAccessLinks() {
        return layersBusinessService.fetchAccessLinks();
    }

    vm.getLayerData();

    $scope.$on("updateLayerControl", vm.getLayerData);
};
