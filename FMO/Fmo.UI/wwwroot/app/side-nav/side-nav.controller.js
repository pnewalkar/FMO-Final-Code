angular
    .module('sideNav')
    .controller("SideNavController", SideNavController);

SideNavController.$inject = [
    '$state',
    '$stateParams',
    'popUpSettingService',
    '$mdSidenav',
    '$mdDialog',
    'sideNavService',
    'CommonConstants'
];

function SideNavController(
    $state,
    $stateParams,
    popUpSettingService,
    $mdSidenav,
    $mdDialog,
    sideNavService,
    CommonConstants) {

    var vm = this;
    vm.initialize = initialize;
    vm.fetchActions = fetchActions;
    vm.closeSideNav = closeSideNav;
    vm.initialize();

    function initialize() {
        vm.RolesActionResult = sideNavService.fetchActionItems().RolesActionResult;
        vm.contextTitle = CommonConstants.TitleContextPanel;
    }

    function closeSideNav() {
        $mdSidenav('left').close();
    }

    function fetchActions(selectedItem) {
        vm.closeSideNav();
        switch (selectedItem) {
            case CommonConstants.RouteLogActionName:
                $mdDialog.show(popUpSettingService.routeLog(vm.selectedDeliveryUnit));
                break;
            case CommonConstants.RouteSimulationActionName:
                vm.contextTitle = CommonConstants.TitleSimulation;
                $state.go("routeSimulation", { selectedUnit: vm.selectedDeliveryUnit });
                break;
            case CommonConstants.DeliveryPointActionName:
                vm.contextTitle = CommonConstants.DeliveryPointActionName;
                $state.go("deliveryPoint", { selectedUnit: vm.selectedDeliveryUnit });
                break;
            case CommonConstants.AccessLinkActionName:
                vm.contextTitle = CommonConstants.AccessLinkActionName;
                $state.go("referenceData");
                break;
            default:
                break;
        }
    }
}

