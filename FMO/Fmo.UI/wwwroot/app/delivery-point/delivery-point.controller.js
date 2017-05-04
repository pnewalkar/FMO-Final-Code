angular
    .module('deliveryPoint')
    .controller("DeliveryPointController", ['$scope', '$mdDialog', 'deliveryPointService', 'deliveryPointApiService', DeliveryPointController])
function DeliveryPointController($scope, $mdDialog, deliveryPointService, deliveryPointApiService) {
    var vm = this;
    vm.resultSet = resultSet;
    vm.querySearch = querySearch;
    vm.deliveryPoint = deliveryPoint;
    vm.openModalPopup = openModalPopup;
    vm.closeWindow = closeWindow;
    vm.OnChangeItem = OnChangeItem;
    vm.getPostalAddress = getPostalAddress;
    vm.display = false;
    vm.disable = true;

    querySearch
    function querySearch(query) {
        debugger;
        deliveryPointApiService.GetDeliveryPointsResultSet(query).then(function (response) {
            debugger;
            vm.results = response.data;



        });
    }

    function deliveryPoint() {
        var deliveryPointTemplate = deliveryPointService.deliveryPoint();
        vm.openModalPopup(deliveryPointTemplate);
    }

    function resultSet(query) {
        if (query.length >= 3) {
            querySearch(query);
        }
        else {
            vm.results = {};
            vm.resultscount = { 0: { count: 0 } };
        }
    }

    function getPostalAddress(selectedItem) {
        var arrSelectedItem = selectedItem.split(',');
        var postCode;
        if (arrSelectedItem.length == 2) {
            postCode = arrSelectedItem[1].trim();
        }
        else {
            postCode = arrSelectedItem[0].trim();
        }
        deliveryPointApiService.GetAddressdetails(postCode).then(function (response) {
            debugger;
            vm.postalAddressData = response.data;
            vm.display = true;
            vm.disable = false;
    });

}

    function OnChangeItem(selectedItem) {

        vm.searchText = selectedItem;
        vm.results = {
    };
}

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting)
}

    function closeWindow() {
        $mdDialog.hide(vm.close);
}
};
