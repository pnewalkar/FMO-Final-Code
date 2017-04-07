angular.module('unitSelector')
.controller('UnitSelectorController', ['$scope', '$stateParams', '$state' ,'unitSelectorAPIService', UnitSelectorController])
function UnitSelectorController($scope, $stateParams, $state, unitSelectorAPIService) {
    var vm = this;
    vm.DeliveryUnit = DeliveryUnit;
    vm.selectedDeliveryUnit = null;
    vm.deliveryUnit = [
       { id: 1, name: 'Unit One' },
       { id: 2, name: 'Unit Two' },
       { id: 3, name: 'Unit Three' }
    ];
    vm.selectedUser = { id: 1, name: 'Bob' };
    DeliveryUnit();
    function DeliveryUnit() {
        
        unitSelectorAPIService.getDeliveryUnit().then(function(response)
        {
            debugger;

        })
        
        //$state.transitionTo('routeSimulation', {
        //    selectedUnit: vm.selectedUser
        //});


       // $state.go("routeSimulation", { selectedUnit: vm.selectedUser }, {location: false, inherit: false
   // });
        //$state.go('routeSimulation', { selectedUnit: vm.selectedUser });
        //unitSelectorAPIService.GetDeliveryUnit().then(function (response) {
        //    vm.deliveryUnit = response.data;
        //});
    }

}
