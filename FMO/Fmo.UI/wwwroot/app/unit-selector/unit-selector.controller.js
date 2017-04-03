angular.module('unitSelector')
.controller('UnitSelectorController', ['$scope', 'unitSelectorAPIService', UnitSelectorController])
function UnitSelectorController($scope, unitSelectorAPIService) {
    var vm = this;
    vm.deliveryUnit = null;
    function DeliveryUnit() {
        unitSelectorAPIService.GetDeliveryUnit().then(function (response) {
            vm.deliveryUnit = response.data;
        });
    }

}
