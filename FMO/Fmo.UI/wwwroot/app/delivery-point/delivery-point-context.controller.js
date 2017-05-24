angular
    .module('deliveryPoint')
    .controller('DeliveryPointContextController',
    DeliveryPointContextController);

DeliveryPointContextController.$inject = ['$scope',
    '$state',
    '$stateParams']

function DeliveryPointContextController($scope,
    $state,
    $stateParams)
    {
        var vm = this;
        vm.selectedDeliveryPoint = $stateParams.selectedDeliveryPoint;
    }
