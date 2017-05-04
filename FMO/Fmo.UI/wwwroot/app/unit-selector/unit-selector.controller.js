angular.module('unitSelector')
.controller('UnitSelectorController', ['$scope', '$stateParams', '$state', '$filter', 'unitSelectorAPIService', 'mapFactory', 'manageAccessBusinessService', UnitSelectorController])
function UnitSelectorController($scope, $stateParams, $state, $filter, unitSelectorAPIService, mapFactory, manageAccessBusinessService) {
    var vm = this;
    vm.BindData = BindData;
    vm.DeliveryUnit = DeliveryUnit;
    vm.selectedDeliveryUnit = null;
    vm.deliveryRouteUnit = [];
    vm.isDeliveryUnitDisabled = false;
    BindData();

    function DeliveryUnit() {
        vm.selectedDeliveryUnit = vm.selectedUser;
        manageAccessBusinessService.activate(vm.selectedDeliveryUnit.id);
        updateMapAfterUnitChange(vm.selectedDeliveryUnit);

    }

    function BindData() {
        if (vm.deliveryRouteUnit.length === 0) {
            var authData = sessionStorage.getItem('authorizationData');
            authData = JSON.parse(authData);
            if (authData.unitGuid) {
                unitSelectorAPIService.getDeliveryUnit().then(function (response) {
                    if (response) {
                        vm.deliveryRouteUnit = response;
                        var newTemp = $filter("filter")(vm.deliveryRouteUnit, { id: authData.unitGuid });
                        vm.selectedUser = newTemp[0];
                        vm.selectedDeliveryUnit = vm.selectedUser;
                        updateMapAfterUnitChange(vm.selectedDeliveryUnit);
                    }
                });

            } else {
                unitSelectorAPIService.getDeliveryUnit().then(function (response) {
                    if (response) {
                        if (response.length === 1)
                        {
                            vm.isDeliveryUnitDisabled = true;
                        }
                        vm.deliveryRouteUnit = response;
                        vm.selectedUser = vm.deliveryRouteUnit[0];
                        vm.selectedDeliveryUnit = vm.selectedUser;
                        updateMapAfterUnitChange(vm.selectedDeliveryUnit);
                    }
                });
            }
        }
    }

    function updateMapAfterUnitChange(selectedUnit) {
        mapFactory.setUnitBoundaries(selectedUnit.boundingBox, selectedUnit.boundingBoxCenter, selectedUnit.unitBoundaryGeoJSONData);
    }
}

