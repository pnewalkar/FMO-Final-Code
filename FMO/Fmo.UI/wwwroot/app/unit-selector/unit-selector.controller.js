angular.module('unitSelector')
.controller('UnitSelectorController', ['$scope', '$stateParams', '$state', 'unitSelectorAPIService', UnitSelectorController])
function UnitSelectorController($scope, $stateParams, $state, unitSelectorAPIService) {
    var vm = this;
    vm.BindData = BindData;
    vm.DeliveryUnit = DeliveryUnit;
    vm.selectedDeliveryUnit = null;
    vm.deliveryRouteUnit = [];
    BindData();
    function DeliveryUnit() {
        vm.selectedDeliveryUnit = vm.selectedUser;
    }

    function BindData() {
        unitSelectorAPIService.getDeliveryUnit().then(function (response) {
            debugger;
            if (response.data)
            vm.deliveryRouteUnit = response.data;
            vm.selectedUser = vm.deliveryRouteUnit[0];
            vm.selectedDeliveryUnit = vm.selectedUser;

        });
    }

}
