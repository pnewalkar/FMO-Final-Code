angular.module('unitSelector')
.controller('UnitSelectorController', ['$scope', '$stateParams', '$state', 'unitSelectorAPIService', 'mapFactory','manageAccessBusinessService', UnitSelectorController])
function UnitSelectorController($scope, $stateParams, $state, unitSelectorAPIService, mapFactory, manageAccessBusinessService) {
    var vm = this;
    vm.BindData = BindData;
    vm.DeliveryUnit = DeliveryUnit;
    vm.selectedDeliveryUnit = null;
    vm.deliveryRouteUnit = [];
    BindData();

    function DeliveryUnit() {        
        vm.selectedDeliveryUnit = vm.selectedUser;
        manageAccessBusinessService.activate(vm.selectedDeliveryUnit.id);
        updateMapAfterUnitChange(vm.selectedDeliveryUnit);          

    }

    function BindData() {
        unitSelectorAPIService.getDeliveryUnit().then(function (response) {
            if (response.data)
                vm.deliveryRouteUnit = response.data;
            vm.selectedUser = vm.deliveryRouteUnit[0];
            vm.selectedDeliveryUnit = vm.selectedUser;
            updateMapAfterUnitChange(vm.selectedDeliveryUnit);
        });
        
    }

    function updateMapAfterUnitChange(selectedUnit)
    {       
        mapFactory.setUnitBoundaries(selectedUnit.boundingBox, selectedUnit.boundingBoxCenter);
      
        var map = mapFactory.getMap();
        var layerToRemove;
        map.getLayers().forEach(function (el) {
            if (el.get('name') == 'deliveryUnitVectorLayer');
            {
                layerToRemove = el;
            }
            
        })
     
        if (layerToRemove) {
            console.log(layerToRemove.get('name'));
            mapFactory.getMap().removeLayer(layerToRemove);
        }

        var deliveryUnitVectorLayer = new ol.layer.Vector({
            source: new ol.source.Vector({
                format: new ol.format.GeoJSON({ defaultDataProjection: 'EPSG:27700' }),
                features: (new ol.format.GeoJSON()).readFeatures(selectedUnit.unitBoundaryGeoJSONData)
            })
        });

        deliveryUnitVectorLayer.set('name', 'deliveryUnitVectorLayer')
        mapFactory.getMap().addLayer(deliveryUnitVectorLayer);

    }
}

