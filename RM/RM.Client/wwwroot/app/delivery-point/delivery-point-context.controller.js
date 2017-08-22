angular
    .module('deliveryPoint')
    .controller('DeliveryPointContextController',
    DeliveryPointContextController);

DeliveryPointContextController.$inject = [
    'GlobalSettings',
    '$rootScope',
    'selectedDeliveryPointService',
    'CommonConstants',
    'popUpSettingService',
    '$mdDialog',
    'deliveryPointService',
    'mapService',
    '$state']

function DeliveryPointContextController(
    GlobalSettings,
    $rootScope,
    selectedDeliveryPointService,
    CommonConstants,
    popUpSettingService,
    $mdDialog,
    deliveryPointService,
    mapService,
    $state)
{
    var vm = this;
    vm.deleteDeliveryPoint = deleteDeliveryPoint;
    vm.closeWindow = closeWindow;
    vm.fetchReasonCodesForDelete = fetchReasonCodesForDelete();
    vm.userDeleteDeliveryPoint = userDeleteDeliveryPoint;
    vm.reasonText = "";
    vm.reasonCode = "";

        vm.selectedDeliveryPoint = selectedDeliveryPointService.getSelectedDeliveryPoint();
        if (vm.selectedDeliveryPoint != null) {
            vm.selectedDeliveryPointType = vm.selectedDeliveryPoint.type;
            vm.selectedDeliveryPointId = vm.selectedDeliveryPoint.deliveryPointId;
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

            function closeWindow() {
                vm.hide = false;         
                deliveryPointService.closeModalPopup();            
            }

            function fetchReasonCodesForDelete() { 
                deliveryPointService.reasonCodeValues()
                    .then(function (response) {
                        vm.reasonCodes = response.listItems;
                    });
            }

            function userDeleteDeliveryPoint() {
                var reasonCode = vm.reasonCode.replace("/", "&frasl;");
                deliveryPointService.deleteDeliveryPoint(vm.selectedDeliveryPointId, reasonCode, vm.reasonText)
                .then(function (response) {
                    mapService.deleteDeliveryPoint(vm.selectedDeliveryPointId);
                    mapService.refreshLayers();
                    vm.closeWindow();
                    $state.go("deliveryPoint", { selectedUnit: null }, { inherit: false });
                });
               
            }
    }
