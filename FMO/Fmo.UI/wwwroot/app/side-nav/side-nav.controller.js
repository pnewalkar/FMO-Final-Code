angular
    .module('sideNav')
    .controller("sideNavController",
               ['$scope',
                '$state',
                '$stateParams',
                'routeLogService',
                '$mdSidenav',
                '$mdDialog',
                'sideNavApiService', 'SideNavConstant',
                 sideNavController]);

function sideNavController($scope,
                           $state,
                           $stateParams,
                           routeLogService,
                           $mdSidenav,
                           $mdDialog,
                           sideNavApiService,
                           SideNavConstant)
                           {
   var vm = this;
    vm.routeLog = routeLog;
    vm.openModalPopup = openModalPopup;
    vm.fetchActionItems = fetchActionItems;
    vm.fetchActions = fetchActions;
    vm.closeSideNav = closeSideNav;
    vm.routeSimulation = routeSimulation;
    vm.deliveryPoint = deliveryPoint;
    vm.selectedUnit = $stateParams;
    vm.contextTitle = "Context Panel";
    vm.fetchActionItems();

    function routeSimulation(selectedDeliveryUnit) {
        vm.contextTitle = "Simulation";
        $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
    }

    function deliveryPoint(selectedDeliveryUnit) {
        vm.contextTitle = "Delivery Point";
        $state.go("deliveryPoint", { selectedUnit: selectedDeliveryUnit });
    }
                               
    function routeLog(selectedUnit) {
        var state = $stateParams;
        var setting = routeLogService.routeLog(selectedUnit);
        vm.openModalPopup(setting);
    }

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting);
    }

    function fetchActionItems() {
        vm.getItem = sessionStorage.getItem('roleAccessData');
        vm.RolesActionResult = angular.fromJson(vm.getItem);
    }

    function closeSideNav() {
        $mdSidenav('left').close();
    }

    function fetchActions(query, selectedUnit) {
        vm.closeSideNav();
        if (query === SideNavConstant.routeLogActionName) {
            vm.routeLog(selectedUnit);
        }
        if (query === SideNavConstant.routeSimulationActionName) {
            vm.routeSimulation(selectedUnit);
        }
        if (query === SideNavConstant.deliveryPointActionName) {
            vm.deliveryPoint(selectedUnit);
        }
        if (query === "Access Link") {
            vm.contextTitle = "Reference Data";
            $state.go("referenceData");
        }
    }

}

