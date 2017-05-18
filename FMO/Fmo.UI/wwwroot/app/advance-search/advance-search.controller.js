'use strict';
angular
    .module('advanceSearch')
    .controller('AdvanceSearchController', [
        'searchApiService',
        'mapFactory',
        '$state',
        '$mdDialog',
        '$stateParams',
        'data',
        AdvanceSearchController]);

function AdvanceSearchController(
    searchApiService,
    mapFactory,
    $state,
    $mdDialog,
    $stateParams,
    data) {

    var vm = this;
    vm.initialize = initialize;
    vm.OnChangeItem = OnChangeItem;
    vm.toggleList = toggleList;
    vm.toggleSelection = toggleSelection;
    vm.queryAdvanceSearch = queryAdvanceSearch;

    vm.results = [];
    vm.searchText = data;
    vm.arrDeliverypoints = [];
    vm.arrPostCodes = [];
    vm.arrStreetNames = [];
    vm.arrDeliveryRoutes = [];
    vm.closeWindow = closeWindow;
    vm.closeAlert = closeAlert;
    vm.close = "close";

    vm.initialize()

    function initialize() {
        vm.queryAdvanceSearch(vm.searchText);
        vm.chkRoute = true;
        vm.chkPostCode = true;
        vm.chkDeliveryPoint = true;
        vm.chkStreetNetwork = true;

    }

    function closeWindow() {
        $mdDialog.hide(vm.close);
    }

    function queryAdvanceSearch(query) {

        searchApiService.advanceSearch(query).then(function (response) {

            vm.results = response.data;
            vm.searchCount = vm.results.searchCounts;
            vm.searchItem = vm.results.searchResultItems;
            vm.arrRoutes = [];




            for (var i = 0; i < vm.results.searchResultItems.length; i++) {
                vm.route = vm.results.searchResultItems[i];
                vm.obj;
                if (vm.route.type == 'DeliveryPoint') {
                    vm.obj = { 'displayText': vm.route.displayText, 'UDPRN': vm.route.udprn, 'type': "DeliveryPoint" }
                    vm.arrDeliverypoints.push(vm.obj);
                }
                else if (vm.route.type == 'Postcode') {
                    vm.obj = { 'displayText': vm.route.displayText }
                    vm.arrPostCodes.push(vm.obj);
                }
                else if (vm.route.type == 'StreetNetwork') {
                    vm.obj = { 'displayText': vm.route.displayText }
                    vm.arrStreetNames.push(vm.obj);
                }
                else if (vm.route.type == 'Route') {
                    vm.obj = { 'displayText': vm.route.displayText }
                    vm.arrDeliveryRoutes.push(vm.obj);
                }
            }
            if (vm.arrDeliverypoints.length == 1) {
                vm.deliveryPointObj = { 'type': 'DeliveryPoint', 'name': vm.arrDeliverypoints, 'open': true };
            }
            else {
                vm.deliveryPointObj = { 'type': 'DeliveryPoint', 'name': vm.arrDeliverypoints, 'open': false };
            }

            if (vm.arrPostCodes.length == 1) {
                vm.postCodeObj = {
                    'type': 'PostCode', 'name': vm.arrPostCodes, 'open': true
                };
            }
            else {
                vm.postCodeObj = {
                    'type': 'PostCode', 'name': vm.arrPostCodes, 'open': false
                };
            }
            if (vm.arrStreetNames.length == 1) {
                vm.streetnameObj = { 'type': 'StreetNetwork', 'name': vm.arrStreetNames, 'open': true };
            }
            else {
                vm.streetnameObj = { 'type': 'StreetNetwork', 'name': vm.arrStreetNames, 'open': false };
            }
            if (vm.arrDeliveryRoutes.length == 1) {
                vm.deliveryRouteobj = { 'type': 'Route', 'name': vm.arrDeliveryRoutes, 'open': true };
            }
            else {
                vm.deliveryRouteobj = { 'type': 'Route', 'name': vm.arrDeliveryRoutes, 'open': false };
            }

            if (vm.arrDeliverypoints.length > 0) {
                vm.arrRoutes.push(vm.deliveryPointObj);
            }
            if (vm.arrPostCodes.length > 0) {
                vm.arrRoutes.push(vm.postCodeObj);
            }
            if (vm.arrStreetNames.length > 0) {
                vm.arrRoutes.push(vm.streetnameObj);
            }
            if (vm.arrDeliveryRoutes.length > 0) {
                vm.arrRoutes.push(vm.deliveryRouteobj);
            }

        });
    }

    function toggleSelection(selectedFlag) {
        vm.chkRoute = selectedFlag;
        vm.chkPostCode = selectedFlag;
        vm.chkDeliveryPoint = selectedFlag;
        vm.chkStreetNetwork = selectedFlag;
    }

    function toggleList(state) {
        vm.arrRoutes.forEach(function (e) {
            e.open = state;
        });
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === "DeliveryPoint") {
            searchApiService.GetDeliveryPointByUDPRN(selectedItem.UDPRN)
                .then(function (response) {
                    vm.data = response.data;
                    vm.lat = vm.data.features[0].geometry.coordinates[1];
                    vm.long = vm.data.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(vm.long, vm.lat);
                });
            $state.go('searchDetails', { selectedItem: selectedItem });
        }
        vm.closeAlert(selectedItem.type);
    }

    function closeAlert(selectedItem) {
        if (selectedItem === "DeliveryPoint") {
            $mdDialog.hide(vm.close);
        }
    }
}