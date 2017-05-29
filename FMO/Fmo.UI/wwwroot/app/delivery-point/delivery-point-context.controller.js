angular
    .module('deliveryPoint')
    .controller('DeliveryPointContextController',
    DeliveryPointContextController);

DeliveryPointContextController.$inject = [
    '$stateParams']

function DeliveryPointContextController(
    $stateParams)
    {
        var vm = this;
        vm.selectedDeliveryPoint = $stateParams.selectedDeliveryPoint;
    }
