angular.module('deliveryPoint')
.controller('DeliveryPointContextController', ['$scope', '$state', '$stateParams', DeliveryPointContextController])
function DeliveryPointContextController($scope, $state, $stateParams) {
    var vm = this;
    vm.selectedDeliveryPoint = $stateParams.selectedDeliveryPoint;
    vm.showDeliveryPoint = "";
}
