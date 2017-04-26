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
                 sideNavController])

function sideNavController($scope,
                           $state,
                           $stateParams,
                           routeLogService,
                           $mdSidenav,
                           $mdDialog,
                           sideNavApiService, SideNavConstant)
                           {
    vm = this;
    vm.routeLog = routeLog;
    vm.openModalPopup = openModalPopup;
    vm.fetchActionItems = fetchActionItems;
    vm.fetchActions = fetchActions;
    vm.closeSideNav = closeSideNav;
    vm.routeSimulation = routeSimulation;
    vm.selectedUnit = $stateParams;

    function routeSimulation(selectedDeliveryUnit) {

        $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
    }
    $scope.toggleSideNav = function () {
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

    function fetchActionItems() {
        vm.getItem = sessionStorage.getItem('roleAccessData');
        vm.RolesActionResult = JSON.parse(vm.getItem);
    }

    function closeSideNav() {
        $mdSidenav('left').close();
    };

    function fetchActions(query, selectedUnit) {
        if (query === SideNavConstant.routeLogActionName) {
            vm.routeLog(selectedUnit);
        }
        if (query === SideNavConstant.routeSimulationActionName) {
            vm.routeSimulation(selectedUnit);
        }
    }

}

