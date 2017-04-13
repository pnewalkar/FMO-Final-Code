angular
    .module('sideNav')
    .controller("sideNavController",['$scope','$state','$stateParams','routeLogService','$mdSidenav','$mdDialog','advanceSearchService', sideNavController])

function sideNavController($scope, $state, $stateParams,routeLogService, $mdSidenav, $mdDialog, advanceSearchService) {
        vm = this;
        vm.routeLog =routeLog;
        vm.openModalPopup=openModalPopup;
        vm.advanceSearch = advanceSearch;
        vm.openAdvanceSearchPopup = openAdvanceSearchPopup;
        vm.closeSideNav = closeSideNav;
        vm.routeSimulation = routeSimulation;
        vm.selectedUnit = $stateParams;
        function routeSimulation(selectedDeliveryUnit) {
            debugger;
            $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
        }
        $scope.toggleSideNav = function() {
        $mdSidenav('left').toggle();
    }

    function routeLog(selectedUnit) {
        var state = $stateParams;
        var setting = routeLogService.routeLog(selectedUnit);
        vm.openModalPopup(setting);
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