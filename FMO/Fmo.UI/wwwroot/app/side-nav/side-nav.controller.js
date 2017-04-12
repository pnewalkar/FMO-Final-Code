angular
    .module('sideNav')
    .controller("sideNavController",['$scope','routeLogService','$mdSidenav','$mdDialog','advanceSearchService', sideNavController])

function sideNavController($scope, routeLogService, $mdSidenav, $mdDialog, advanceSearchService) {
        vm = this;
        vm.routeLog =routeLog;
        vm.openModalPopup=openModalPopup;
        vm.advanceSearch = advanceSearch;
        vm.openAdvanceSearchPopup = openAdvanceSearchPopup;
        vm.closeSideNav = closeSideNav;
        
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
        
        function advanceSearch() {
            var advaceSearchTemplate = advanceSearchService.advanceSearch();
            vm.openAdvanceSearchPopup(advaceSearchTemplate);
            ///vm.openModalPopup("Test");
        }

        function openAdvanceSearchPopup(modalSetting) {
            var popupSetting = modalSetting;
            $mdDialog.show(popupSetting)
        };

        function closeSideNav() {
            $mdSidenav('left').close();
        };
          
};