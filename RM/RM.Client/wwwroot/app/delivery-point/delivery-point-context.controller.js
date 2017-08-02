angular
    .module('deliveryPoint')
    .controller('DeliveryPointContextController',
    DeliveryPointContextController);

DeliveryPointContextController.$inject = ['GlobalSettings',
    '$rootScope',
 'selectedDeliveryPointService','CommonConstants']

function DeliveryPointContextController(GlobalSettings,
    $rootScope,
selectedDeliveryPointService, CommonConstants)
{
        var vm = this;
        vm.selectedDeliveryPoint = selectedDeliveryPointService.getSelectedDeliveryPoint();
        if (vm.selectedDeliveryPoint != null) {
            vm.selectedDeliveryPointType = vm.selectedDeliveryPoint.type;
        }
        else {
            vm.selectedDeliveryPointType = "others";
        }
            $rootScope.$broadcast('showDeliveryPointDetails', {
                contextTitle: CommonConstants.DetailsOfDeliveryPoint,
                featureType: vm.selectedDeliveryPointType
            });      
    }
