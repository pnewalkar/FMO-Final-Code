angular.module('unitSelector')
.controller('UnitSelectorController', UnitSelectorController);

UnitSelectorController.$inject = [
    'unitSelectorService',
    '$scope',
    'licensingInfoService'
]

function UnitSelectorController(unitSelectorService, $scope, licensingInfoService) {
    var vm = this;
    vm.BindData = BindData;
    vm.DeliveryUnit = DeliveryUnit;
    vm.selectedDeliveryUnit = null;
    vm.deliveryRouteUnit = [];
    vm.isDeliveryUnitDisabled = false;
    vm.selectClass = "routeSearch md-text";

    BindData();

    //$scope.$on('selectionChanged', function (event, args) {
    //    vm.selectedUser = args.selectedRoute;
    //    DeliveryUnit();
    //});

    function DeliveryUnit(selectedUser) {
        // vm.selectedDeliveryUnit = vm.selectedUser;
        unitSelectorService.DeliveryUnit(selectedUser);
    }
    function BindData() {
        unitSelectorService.BindData(vm.deliveryRouteUnit).then(function (response) {

            vm.deliveryRouteUnit = response[0].deliveryRouteUnit;
            vm.selectedUser = response[0].selectedUser;
            vm.selectedDeliveryUnit = vm.selectedUser;
            vm.isDeliveryUnitDisabled = response[0].isDeliveryUnitDisabled;

            var selectdUnitDetails = {
                "displayText": vm.selectedDeliveryUnit.displayText,
                "ID": vm.selectedDeliveryUnit.ID,
                "icon": vm.selectedDeliveryUnit.icon,
                "area": vm.selectedDeliveryUnit.area,
                "unitName": vm.selectedDeliveryUnit.unitName
            };
            sessionStorage.setItem("selectedDeliveryUnit", angular.toJson((selectdUnitDetails)));
            licensingInfoService.getLicensingInfo();
        });
    }
}

