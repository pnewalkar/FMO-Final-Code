'use strict';
angular.module('layers')
    .controller('LayerSelectorController', [
          '$scope',
          'layersService','$rootScope','$state',
          LayerSelectorController]);

function LayerSelectorController(
    $scope,
    layersService,
    $rootScope,
    $state
    )
{
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
        //if (changedLayer === "Group")
        //{
            addGroup();
      //  }
    }
    function addGroup() {
        $rootScope.$emit('resetMapToolbar', { "isGroupAction": true });
        $state.go("deliveryPointGroupDetails");
    }
    function showUngrouped() {
        return layersService.showUngrouped();
    }

    vm.getLayerData();

    $scope.$on("updateLayerControl", vm.getLayerData);
};
