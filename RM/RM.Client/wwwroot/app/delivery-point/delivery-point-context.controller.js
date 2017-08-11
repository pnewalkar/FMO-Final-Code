angular
    .module('deliveryPoint')
    .controller('DeliveryPointContextController',
    DeliveryPointContextController);

DeliveryPointContextController.$inject = ['GlobalSettings',
    '$rootScope',
 'selectedDeliveryPointService', 'CommonConstants', 'popUpSettingService', '$mdDialog']

function DeliveryPointContextController(GlobalSettings,
    $rootScope,
selectedDeliveryPointService, CommonConstants, popUpSettingService, $mdDialog)
{
    var vm = this;
    vm.deleteDeliveryPoint = deleteDeliveryPoint;
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

            function deleteDeliveryPoint() {
                
                var deleteDeliveryPointTemplate = popUpSettingService.deleteDeliveryPoint();
                openModalPopup(deleteDeliveryPointTemplate);
                
            }

            function openModalPopup(modalSetting) {
                var popupSetting = modalSetting;
                $mdDialog.show(popupSetting).then(function (returnedData) {
                });


            }


    }
