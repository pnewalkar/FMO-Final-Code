angular
    .module('sideNav')
    .controller("sideNavController", ['$stateParams', '$state','$scope', 'routeLogService', '$mdSidenav', '$mdDialog', sideNavController])

function sideNavController($stateParams, $state, $scope, routeLogService, $mdSidenav, $mdDialog) {
        vm = this;
        vm.routeLog =routeLog;
        vm.openModalPopup=openModalPopup;
        
        vm.routeSimulation = routeSimulation;




        function routeSimulation(selectedDeliveryUnit) {
            debugger;
            $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
        }
        $scope.toggleSideNav = function() {
        $mdSidenav('left').toggle();
                }
        
    function routeLog(){
        var setting =routeLogService.routeLog(); 
        vm.openModalPopup(setting);
         ///vm.openModalPopup("Test");
    }
        
        function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
      $mdDialog.show(popupSetting)
    };
};