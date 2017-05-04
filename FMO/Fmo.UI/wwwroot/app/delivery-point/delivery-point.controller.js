angular
    .module('deliveryPoint')
    .controller("DeliveryPointController", ['$scope', '$mdDialog', 'deliveryPointService', 'deliveryPointApiService', 'referencedataApiService',
                '$filter',
                'referenceDataConstants'
, DeliveryPointController])
function DeliveryPointController($scope, $mdDialog, deliveryPointService, deliveryPointApiService, referencedataApiService,
    $filter,
    referenceDataConstants
) {
    var vm = this;
    vm.resultSet = resultSet;
    vm.querySearch = querySearch;
    vm.deliveryPoint = deliveryPoint;
    vm.openModalPopup = openModalPopup;
    vm.closeWindow = closeWindow;
    vm.OnChangeItem = OnChangeItem;
    vm.getPostalAddress = getPostalAddress;
    vm.getAddressLocation = getAddressLocation;
    vm.onBlur = onBlur;
    vm.display = false;
    vm.disable = true;

    referenceData();

    function querySearch(query) {
        deliveryPointApiService.GetDeliveryPointsResultSet(query).then(function (response) {
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

    function onBlur() {
        $timeout(function () {
             vm.results = { };
        }, 1000);
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
        deliveryPointApiService.GetAddressByPostCode(postCode).then(function (response) {
            vm.postalAddressData = response.data;
            vm.display = true;
            vm.disable = false;
        });

    }

    function getAddressLocation(udprn) {
        deliveryPointApiService.GetAddressLocation(udprn)
                .then(function (response) {
                    debugger;
                    vm.addressLocationData = response.data;
                });
    }

    function OnChangeItem(selectedItem) {
        vm.searchText = selectedItem;
        vm.results = {};
    }

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting)
    }

    function closeWindow() {
        $mdDialog.hide(vm.close);
    }

    function referenceData() {
        debugger;
        referencedataApiService.getReferenceData().success(function (response) {
            debugger;
            vm.deliveryPointTypes = $filter('filter')(response, { categoryName: referenceDataConstants.DeliveryPointType });
        });
    }

};
