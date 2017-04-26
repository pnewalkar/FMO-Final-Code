angular
    .module('sideNav')
    .controller("sideNavController",
               ['$scope',
                '$state',
                '$stateParams',
                'routeLogService',
                '$mdSidenav',
                '$mdDialog',
                'sideNavApiService',
                 sideNavController])

function sideNavController($scope,
                           $state,
                           $stateParams,
                           routeLogService,
                           $mdSidenav,
                           $mdDialog,
                           sideNavApiService)
                           {
    vm = this;
    vm.routeLog = routeLog;
    vm.openModalPopup = openModalPopup;
    vm.fetchActionItems = fetchActionItems;
    vm.fetchActions = fetchActions;
    vm.closeSideNav = closeSideNav;
    vm.routeSimulation = routeSimulation;
    vm.selectedUnit = $stateParams;
    vm.fetchActionItems();


    function routeSimulation(selectedDeliveryUnit) {

        $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
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
        if (vm.selectedUnit != null) {
            vm.getItem = sessionStorage.getItem('roleAccessData');
            vm.RolesActionResult = JSON.parse(vm.getItem);
            if (vm.RolesActionResult.length != null) {
                for (i = 0; i < vm.RolesActionResult.length; i++) {

                    for (var j = i + 1; j < vm.RolesActionResult.length; j++) {
                        if (vm.RolesActionResult[i]["ActionName"] == vm.RolesActionResult[j]["ActionName"]) {
                            vm.RolesActionResult.splice(j, 1);
                        }
                    }

                }
            }
        }
    }

    function closeSideNav() {
        $mdSidenav('left').close();
    };

    function fetchActions(query, selectedUnit) {
        if (query == "Route Log") {
            vm.routeLog(selectedUnit);
        }
        if (query == "Route Simulation") {
            vm.routeSimulation(selectedUnit);
        }
    }

};