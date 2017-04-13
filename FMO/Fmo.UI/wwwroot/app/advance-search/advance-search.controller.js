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
    vm.query = "roa";
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
            vm.searchItem=results.searchResultItems;
            for (var i = 0; i < vm.results.searchCounts.length; i++)
            {
                if (vm.results.searchCounts[i].count > 0)
                {
                    if (vm.results.searchCounts[i].type == "0")
                    {

                    }
                    if (vm.results.searchCounts[i].type == "1") {

                    }
                    if (vm.results.searchCounts[i].type == "2") {
                        vm.routeval.push(vm.results.searchCounts[i]);
                        console.log(vm.routeval);
                    }
                    if (vm.results.searchCounts[i].type == "3") {

                    }
                }
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
                           
                           "type": "Route1",
                           "name": [
                             { "type_name": "Route 1" },
                             { "type_name": "Route 2" }]
                         
                       },
                       {
                          
                           "type": "Route2",
                           "name": [
                             { "type_name": "Route 10" },
                             { "type_name": "Route 20" },
                             { "type_name": "Route 30" }]
                          
                       }
                       ,
                       {
                        
                           "type": "Route3",
                           "name": [
                             { "type_name": "Route 100" },
                             { "type_name": "Route 200" },
                             { "type_name": "Route 300" },
                             { "type_name": "Route 400" }]
                          
                       },
                        {

                            "type": "Route4",
                            "name": [
                              { "type_name": "Route 100" },
                              { "type_name": "Route 200" },
                              { "type_name": "Route 300" },
                              { "type_name": "Route 400" }]

                        }
    ]
}