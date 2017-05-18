angular.module('unitSelector')
.controller('UnitSelectorController', ['unitSelectorService', UnitSelectorController]);
function UnitSelectorController(unitSelectorService) {
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
          unitSelectorService.BindData(vm.deliveryRouteUnit).then(function (response) {
            vm.deliveryRouteUnit = response[0].deliveryRouteUnit;
            vm.selectedUser = response[0].selectedUser;
            vm.selectedDeliveryUnit = vm.selectedUser;
            vm.isDeliveryUnitDisabled = response[0].isDeliveryUnitDisabled;
        });
    }
}

