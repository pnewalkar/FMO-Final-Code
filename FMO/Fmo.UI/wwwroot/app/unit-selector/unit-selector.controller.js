angular.module('unitSelector')
.controller('UnitSelectorController', ['$filter', 'unitSelectorService', 'unitSelectorAPIService', 'mapFactory', 'manageAccessBusinessService', UnitSelectorController]);
function UnitSelectorController($filter, unitSelectorService, unitSelectorAPIService, mapFactory, manageAccessBusinessService) {
    var vm = this;
    vm.BindData = BindData;
    vm.DeliveryUnit = DeliveryUnit;
    vm.selectedDeliveryUnit = null;
    vm.deliveryRouteUnit = [];
    vm.isDeliveryUnitDisabled = false;
    BindData();

    function DeliveryUnit() {
        vm.selectedDeliveryUnit = vm.selectedUser;
        unitSelectorService.DeliveryUnit(vm.selectedDeliveryUnit);
    }

    function BindData() {
        debugger;

        var response = unitSelectorService.BindData(vm.deliveryRouteUnit).then(function (response) {
            debugger;
            vm.deliveryRouteUnit = response;
            vm.selectedUser = vm.deliveryRouteUnit[0];
            vm.selectedDeliveryUnit = vm.selectedUser;
           // vm.isDeliveryUnitDisabled = response.isDeliveryUnitDisabled;
        });

    }
}

