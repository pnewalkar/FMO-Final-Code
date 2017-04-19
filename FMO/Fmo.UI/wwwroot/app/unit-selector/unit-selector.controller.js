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

    function updateMapAfterUnitChange(selectedUnit) {

        mapFactory.setUnitBoundaries(selectedUnit.boundingBox, selectedUnit.boundingBoxCenter);
        
        var unitBoundaryLayer = mapFactory.getLayer("Unit Boundary");
      
        unitBoundaryLayer.layer.getSource().clear();

        unitBoundaryLayer.layer.getSource().addFeatures((new ol.format.GeoJSON()).readFeatures(selectedUnit.unitBoundaryGeoJSONData));
    }
}

