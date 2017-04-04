angular.module('unitSelector')
.controller('UnitSelectorController', ['$scope', 'unitSelectorAPIService', UnitSelectorController])
function UnitSelectorController($scope, unitSelectorAPIService) {
    var vm = this;
   
    vm.deliveryUnit = [
       { id: 1, name: 'Unit One' },
       { id: 2, name: 'Unit Two' },
       { id: 3, name: 'Unit Three' }
    ];
    vm.selectedUser = { id: 1, name: 'Bob' };
    
    function DeliveryUnit() {
        unitSelectorAPIService.GetDeliveryUnit().then(function (response) {
            vm.deliveryUnit = response.data;
        });
    }

}
