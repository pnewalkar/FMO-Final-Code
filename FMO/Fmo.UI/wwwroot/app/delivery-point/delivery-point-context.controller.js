angular
    .module('deliveryPoint')
    .controller('DeliveryPointContextController',
    DeliveryPointContextController);

DeliveryPointContextController.$inject = ['GlobalSettings',
    '$rootScope',
    '$stateParams']

function DeliveryPointContextController(GlobalSettings,
    $rootScope,
    $stateParams)
{
        var vm = this;
        vm.selectedDeliveryPoint = $stateParams.selectedDeliveryPoint;
       
            $rootScope.$broadcast('showDeliveryPointDetails', {
                contextTitle: GlobalSettings.deliveryPointDetails,
                featureType: vm.selectedDeliveryPoint.type
            });      
    }
