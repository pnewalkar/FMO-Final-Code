'use strict';
angular.module('advanceSearch')
    .controller('advanceSearchController', ['$scope', 'searchApiService', 'mapFactory', '$state', '$mdDialog','advanceSearchService','$stateParams', AdvanceSearchController]);

function AdvanceSearchController($scope, searchApiService, mapFactory, $state, $mdDialog, advanceSearchService, $stateParams) {
    var vm = this;
    
    vm.chkRoute = true;
    vm.chkPostCode = true;
    vm.chkDeliveryPoint = true;
    vm.chkStreetNetwork = true;
    vm.queryAdvanceSearch = queryAdvanceSearch;
    vm.selectAll = selectAll;
    vm.deSelectAll = deSelectAll;
    var results = [];
    vm.searchText = $stateParams.data;
    vm.query;
    vm.routeval = [];
    var searchCount = [];
    vm.searchCount = searchCount;
    vm.OnChangeItem = OnChangeItem;
    vm.advanceSearch = advanceSearch;
    vm.openAdvanceSearchPopup = openAdvanceSearchPopup;
    var searchItem;
    debugger;
    function queryAdvanceSearch(query) {
       
        debugger;
        console.log(query);
        searchApiService.advanceSearch(query).then(function (response) {

            vm.results = response.data;
            vm.searchCount = vm.results.searchCounts;
            console.log(vm.results.searchCounts);
            vm.searchItem = results.searchResultItems;
            vm.arrRoutes = [];

            var arrDeliverypoints = [];
            var arrPostCodes = [];
            var arrStreetNames = [];
            var arrDeliveryRoutes = [];


            for (var i = 0; i < vm.results.searchResultItems.length; i++) {
                var route = vm.results.searchResultItems[i];
                var obj;
                if (route.type == 'DeliveryPoint') {
                    obj = { 'displayText': route.displayText, 'UDPRN': route.udprn,'type':"DeliveryPoint" }
                    arrDeliverypoints.push(obj);
                }
                else if (route.type == 'Postcode') {
                    obj = { 'displayText': route.displayText }
                    arrPostCodes.push(obj);
                }
                else if (route.type == 'StreetNetwork') {
                    obj = { 'displayText': route.displayText }
                    arrStreetNames.push(obj);
                }
                else if (route.type == 'Route') {
                    obj = { 'displayText': route.displayText }
                    arrDeliveryRoutes.push(obj);
                }
            }
            var deliveryPointObj = { 'type': 'DeliveryPoint', 'name': arrDeliverypoints, 'open':false };
            var postCodeObj = { 'type': 'PostCode', 'name': arrPostCodes, 'open': false };
            var streetnameObj = { 'type': 'StreetNetwork', 'name': arrStreetNames, 'open': false };
            var deliveryRouteobj = { 'type': 'Route', 'name': arrDeliveryRoutes, 'open': false };


            if (arrDeliverypoints.length > 0) {
                vm.arrRoutes.push(deliveryPointObj);
            }
            if (arrPostCodes.length > 0) {
                vm.arrRoutes.push(postCodeObj);
            }
            if (arrStreetNames.length > 0) {
                vm.arrRoutes.push(streetnameObj);
            }
            if (arrDeliveryRoutes.length > 0) {
                vm.arrRoutes.push(deliveryRouteobj);
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
    $scope.routes = [
                       {

                           "type": "Route1", "open": false,
                           "name": [
                             { "type_name": "Route 1" },
                             { "type_name": "Route 2" }]

                       },
                       {

                           "type": "Route2", "open": false,
                           "name": [
                             { "type_name": "Route 10" },
                             { "type_name": "Route 20" },
                             { "type_name": "Route 30" }]

                       }
                       ,
                       {

                           "type": "Route3", "open": false,
                           "name": [
                             { "type_name": "Route 100" },
                             { "type_name": "Route 200" },
                             { "type_name": "Route 300" },
                             { "type_name": "Route 400" }]

                       },
                        {

                            "type": "Route4", "open": false,
                            "name": [
                              { "type_name": "Route 100" },
                              { "type_name": "Route 200" },
                              { "type_name": "Route 300" },
                              { "type_name": "Route 400" }]

                        }
    ]

    $scope.toggleList = function (state) {
        debugger;
        $scope.routes.forEach(function (e) {
            e.open = state;
        });
    }

    function OnChangeItem(selectedItem) {
        debugger;

        if (selectedItem.type === "DeliveryPoint") {
           
            $state.go('searchDetails', { selectedItem: selectedItem});
        }
        $mdDialog.cancel();
    }

    function advanceSearch() {
        var advaceSearchTemplate = advanceSearchService.advanceSearch();
        vm.openAdvanceSearchPopup(advaceSearchTemplate);
        ///vm.openModalPopup("Test");
    }

    function openAdvanceSearchPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting)
    };
}