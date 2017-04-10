angular
    .module('sideNav')
    .controller("sideNavController",['$scope','routeLogService','$mdSidenav','$mdDialog', sideNavController])

    function sideNavController ($scope, routeLogService,$mdSidenav,$mdDialog) {
        vm = this;
        vm.routeLog =routeLog;
        vm.openModalPopup=openModalPopup;
        
        
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