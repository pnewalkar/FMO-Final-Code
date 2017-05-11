'use strict';
angular.module('advanceSearch')
    .controller('advanceSearchController', ['searchApiService',
                                            'mapFactory',
                                             '$state',
                                             '$mdDialog',
                                         'advanceSearchService',
                                         '$stateParams',
                                 AdvanceSearchController]);

function AdvanceSearchController(searchApiService,
                                  mapFactory,
                                  $state,
                                  $mdDialog,
                                  advanceSearchService,
                                  $stateParams)
{
  
    var vm = this;
    vm.toggle = vm.toggle;
    vm.toggleList = vm.toggleList;
    vm.chkRoute = true;
    vm.chkPostCode = true;
    vm.chkDeliveryPoint = true;
    vm.chkStreetNetwork = true;
    vm.queryAdvanceSearch = queryAdvanceSearch;
    vm.selectAll = selectAll;
    vm.deSelectAll = deSelectAll;
    vm.results = [];
    vm.searchText = $stateParams.data;
    vm.query;
    vm.route;
    vm.routeval = [];
    vm.searchCount = vm.searchCount;
    vm.OnChangeItem = OnChangeItem;
    vm.advanceSearch = advanceSearch;
    vm.openAdvanceSearchPopup = openAdvanceSearchPopup;
    vm.arrDeliverypoints = [];
    vm.arrPostCodes = [];
    vm.arrStreetNames = [];
    vm.arrDeliveryRoutes = [];
    vm.obj;
    vm.queryAdvanceSearch(vm.searchText);
    vm.closeWindow = closeWindow;
    vm.closeAlert = closeAlert;
    vm.close = "close";


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
                    vm.arrDeliverypoints.push( vm.obj);
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
            else
            {
                vm.deliveryPointObj = { 'type': 'DeliveryPoint', 'name': vm.arrDeliverypoints, 'open': false };
            }
           
            if (vm.arrPostCodes.length == 1) {
                vm.postCodeObj = {
                    'type': 'PostCode', 'name': vm.arrPostCodes, 'open': true
                };
            }
            else
            {
                vm.postCodeObj = {
                    'type': 'PostCode', 'name': vm.arrPostCodes, 'open': false
                };
            }
            if (vm.arrStreetNames.length == 1) {
                vm.streetnameObj = { 'type': 'StreetNetwork', 'name': vm.arrStreetNames, 'open': true };
            }
            else
            {
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

    function selectAll() {
        vm.chkRoute = true;
        vm.chkPostCode = true;
        vm.chkDeliveryPoint = true;
        vm.chkStreetNetwork = true;
    }

    function deSelectAll() {
        vm.chkRoute = false;
        vm.chkPostCode = false;
        vm.chkDeliveryPoint = false;
        vm.chkStreetNetwork = false;
    }

    vm.toggleList = function (state) {
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

    function advanceSearch() {
        var advaceSearchTemplate = advanceSearchService.advanceSearch();
        vm.openAdvanceSearchPopup(advaceSearchTemplate);
    }

    function openAdvanceSearchPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting);
    };

    function closeAlert(selectedItem) {
        if (selectedItem === "DeliveryPoint") {
            $mdDialog.hide(vm.close);
        }
    }
}