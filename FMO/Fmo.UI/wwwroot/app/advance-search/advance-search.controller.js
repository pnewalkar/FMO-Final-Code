'use strict';
angular.module('advanceSearch')
    .controller('advanceSearchController', ['$scope', 'searchApiService', AdvanceSearchController]);

function AdvanceSearchController($scope, searchApiService) {
    var vm = this;
    vm.chkRoute = true;
    vm.chkPostCode = true;
    vm.chkDeliveryPoint = true;
    vm.chkStreetNetwork = true;
    vm.queryAdvanceSearch = queryAdvanceSearch;
    vm.selectAll = selectAll;
    vm.deSelectAll = deSelectAll;
    var results = [];
    vm.searchText;
    vm.query = "road";
    vm.routeval = [];
    var searchCount = [];
    vm.searchCount = searchCount;
    var searchItem;
    debugger;
    function queryAdvanceSearch() {
        debugger;
        console.log(vm.query);
        searchApiService.advanceSearch(vm.query).then(function (response) {

            vm.results = response.data;
            vm.searchCount = vm.results.searchCounts;

            vm.results.searchCounts.splice(-1, 1)
            // vm.searchCount.remove('seven');
            console.log(vm.results.searchCounts);
            vm.searchItem = results.searchResultItems;
            vm.arrRoutes = [];
            for (var i = 0; i < vm.results.searchResultItems.length; i++) {
                var route = vm.results.searchResultItems[i];
                var routeItem = { 'name': route.displayText }
                var routeArray = [];
                routeArray.push(routeItem);
                var routeobj = { 'type': route.type, 'name': routeArray };
                vm.arrRoutes.push(routeobj);
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

    $scope.toggle = function (state) {
        debugger;
        $scope.routes.forEach(function (e) {
            e.open = state;
        });
    }
}