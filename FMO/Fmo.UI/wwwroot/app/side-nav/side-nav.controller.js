angular
    .module('sideNav')
    .controller("sideNavController",['$scope','$state','$stateParams','routeLogService','$mdSidenav','$mdDialog','advanceSearchService','sideNavApiService', sideNavController])

function sideNavController($scope, $state, $stateParams, routeLogService, $mdSidenav, $mdDialog, advanceSearchService, sideNavApiService) {
        vm = this;
        vm.routeLog =routeLog;
        vm.openModalPopup = openModalPopup;
        vm.fetchActionItems = fetchActionItems;
        $scope.results;
        vm.fetchActions = fetchActions;
        //vm.advanceSearch = advanceSearch;
        //vm.openAdvanceSearchPopup = openAdvanceSearchPopup;
        vm.closeSideNav = closeSideNav;
        vm.routeSimulation = routeSimulation;
        vm.selectedUnit = $stateParams;
        function routeSimulation(selectedDeliveryUnit) {
           
            $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
        }
        $scope.toggleSideNav = function() {
            $mdSidenav('left').toggle();
            vm.fetchActionItems();
            
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
        
    function fetchActionItems()
    {
     
        sideNavApiService.fetchActionItems().then(function (response) {

            $scope.results = response.data;
  
    });
    }

        function closeSideNav() {
            $mdSidenav('left').close();
        };

        function fetchActions(query, selectedUnit) {
          
            if(query=="Route Log")
            {
                vm.routeLog(selectedUnit);
            }
            if (query == "Route Simulation")
            {
                vm.routeSimulation(selectedUnit);
            }
        }
          
};