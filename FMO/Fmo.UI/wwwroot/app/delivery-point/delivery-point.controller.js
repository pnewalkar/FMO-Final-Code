angular
    .module('deliveryPoint')
    .controller("DeliveryPointController", ['$scope','$mdDialog','deliveryPointService', DeliveryPointController])
function DeliveryPointController($scope,$mdDialog,deliveryPointService) {
    var vm = this;
    
    vm.deliveryPoint = deliveryPoint;
    vm.openModalPopup = openModalPopup;
    vm.closeWindow = closeWindow;    
    
    function deliveryPoint() {
        var deliveryPointTemplate = deliveryPointService.deliveryPoint();
        vm.openModalPopup(deliveryPointTemplate);
    }
    
    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting)
    }
    
    function closeWindow() {
        $mdDialog.hide(vm.close);
    }
};