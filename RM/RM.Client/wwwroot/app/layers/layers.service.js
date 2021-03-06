﻿angular.module('layers')
        .service('layersService', layersService);
layersService.$inject = [
                         'mapService',
                         'mapStylesFactory',
                         'layersAPIService',
                         'licensingInfoService',
                         '$translate',
                         'licensingInformationAccessorService'];

function layersService(mapService,
                       mapStylesFactory,
                       layersAPIService,
                       licensingInfoService,
                       $translate,
                       licensingInformationAccessorService) {
    var vm = this;

    return {
        getLayerData: getLayerData,
        refreshLayer: refreshLayer,
        onChange: onChange,
        showUngrouped: showUngrouped   
    };

    function getLayerData() {
        vm.layers = [];
        vm.groups = [];
        vm.ungrouped = [];
        vm.groupNames = {};
        vm.layers = mapService.mapLayers();
        vm.layers.forEach(function (layer) {
            if (!layer.group) {
                vm.ungrouped.push(layer);
            }
            else {
                if (angular.isUndefined(vm.groupNames[layer.group.toString()])) {
                    var id = vm.groups.length;
                    vm.groupNames[layer.group.toString()] = id;
                    vm.groups[id] = { name: layer.group.toString(), layers: [] };
                } else {
                    id = vm.groupNames[layer.group.toString()];
                }
                vm.groups[id].layers.push(layer);
            }
        });
        return {
            groups: vm.groups,
            ungroup: vm.ungrouped
        }
    }

    function refreshLayer(selectedLayer) {
        mapService.refreshLayers();
        setSelectedObjectsVisibility(selectedLayer);
    }

    function onChange(changedLayer) {
        if (changedLayer.group) {
            var group = vm.groups[vm.groupNames[changedLayer.group]];
            var otherEnabled = false;
            group.layers.forEach(function (layer) {
                if (layer !== changedLayer && !layer.disabled && layer.selectorVisible) {
                    layer.selected = false;
                    otherEnabled = true;
                }
            });
            if (otherEnabled)
                changedLayer.selected = true;
        }
        refreshLayer(changedLayer.layerName);
    }

    function showUngrouped() {
        var showGrouped = false;      
        vm.ungrouped.forEach(function (layer) {
            showGrouped = layer.selectorVisible;
        })
        return showGrouped;
    }
  
    function setSelectedObjectsVisibility(selectedLayer) {
        mapService.setSelectedObjectsVisibility(selectedLayer);
        var selectedLayer = mapService.getLayerSummary();
        if (selectedLayer === $translate.instant('LICESING_INFO.NO_LAYERS_SELECTED')) {
            licensingInformationAccessorService.setLicensingInformation(null);
        }
        else {
            licensingInfoService.getLicensingText(selectedLayer);
        }
    }

}