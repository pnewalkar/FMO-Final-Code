﻿angular.module('layers')
        .service('layersService', ['mapService', 'mapStylesFactory', 'layersApiService', layersService])

function layersService(mapService, mapStylesFactory, layersApiService) {
    var vm = this;

    return{
        getLayerData: getLayerData,
        refreshLayer: refreshLayer,
        onChange: onChange,
        showUngrouped: showUngrouped,
        fetchDeliveryPoints: fetchDeliveryPoints,
        fetchAccessLinks: fetchAccessLinks,
    }


    function getLayerData() {
        vm.layers = [];
        vm.groups = [];
        vm.ungrouped = [];
        vm.groupNames = {};
        vm.layers = mapService.mapLayers();
        vm.layers.forEach(function (layer) {
            if (!layer.group){
                vm.ungrouped.push(layer);         
            }
            else{
                if (vm.groupNames[layer.group.toString()] === undefined) {
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
            groups:vm.groups,
            ungroup:vm.ungrouped            
        }
    }

    function refreshLayer() {
        mapService.refreshLayers();
    }

    function onChange(changedLayer) {
        debugger;
        // fetchDeliveryPoints();
        //  fetchAccessLinks();
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
        refreshLayer();
    }

    function showUngrouped() {
        var showGrouped = false;
        vm.ungrouped.forEach(function (layer) {
            showGrouped = layer.selectorVisible;
        })
        return showGrouped;
    }
    function fetchDeliveryPoints() {
        var data;
        var data = layersApiService.fetchDeliveryPoints();
        return showGrouped;
    }

    function fetchAccessLinks() {
        var data;
        var data = layersApiService.fetchAccessLinks();
        return showGrouped;
    }

}