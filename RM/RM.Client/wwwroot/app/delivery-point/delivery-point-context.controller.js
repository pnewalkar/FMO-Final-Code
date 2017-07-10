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
        if ($stateParams.selectedDeliveryPoint != null) {
            vm.selectedDeliveryPointType = vm.selectedDeliveryPoint.type;
        }
        else {
            vm.selectedDeliveryPointType = "others";
        }
            $rootScope.$broadcast('showDeliveryPointDetails', {
                contextTitle: GlobalSettings.deliveryPointDetails,
                featureType: vm.selectedDeliveryPointType
            });      
    }
