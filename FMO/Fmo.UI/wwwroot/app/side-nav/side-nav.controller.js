angular
    .module('sideNav')
    .controller("sideNavController", ['$stateParams', '$state', '$scope', 'routeLogService', '$mdSidenav', '$mdDialog', sideNavController])

function sideNavController($stateParams, $state, $scope, routeLogService, $mdSidenav, $mdDialog) {
    vm = this;
    vm.routeLog = routeLog;
    vm.openModalPopup = openModalPopup;

    vm.routeSimulation = routeSimulation;
    vm.selectedUnit = $stateParams;
    function routeSimulation(selectedDeliveryUnit) {
        debugger;
        $state.go("routeSimulation", { selectedUnit: selectedDeliveryUnit });
    }
    $scope.toggleSideNav = function () {
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
};