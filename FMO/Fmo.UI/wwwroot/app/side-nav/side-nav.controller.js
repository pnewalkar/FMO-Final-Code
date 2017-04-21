angular
    .module('sideNav')
    .controller("sideNavController", ['$scope', '$state', '$stateParams', 'routeLogService', '$mdSidenav', '$mdDialog', 'sideNavApiService', sideNavController])

function sideNavController($scope, $state, $stateParams, routeLogService, $mdSidenav, $mdDialog, sideNavApiService) {
    vm = this;
    vm.routeLog = routeLog;
    vm.openModalPopup = openModalPopup;
    vm.fetchActionItems = fetchActionItems;
    $scope.results;
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
        sideNavApiService.fetchActionItems().then(function (response) {
            vm.results = response.data;
            vm.userUnitInfoDto = [];
            if (vm.results.length != null && vm.RolesActionResult.length != null)
                {

            for (i = 0; i < vm.results.length; i++) {

                for (j = 0; j < vm.RolesActionResult.length; j++) {
                    if (vm.RolesActionResult[j]["ActionName"] === vm.results[i]["name"]) {
                        if (vm.userUnitInfoDto.indexOf(vm.results[i]) < 0) {
                            vm.userUnitInfoDto.push(vm.results[i]);
                        }
                    }
                }
            }
                }
        });
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