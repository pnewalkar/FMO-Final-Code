'use strict';
angular.module('advanceSearch')
    .controller('advanceSearchController', ['$scope', 'searchApiService', AdvanceSearchController]);

function AdvanceSearchController($scope, searchApiService) {
    var vm = this;
    vm.queryAdvanceSearch = queryAdvanceSearch;
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
            for (var i = 0; i < vm.results.searchResultItems.length; i++)
            {
                //if (vm.results.searchResultItems[i].count > 0)
                //{
                    if (vm.results.searchResultItems[i].type == "Postcode")
                    {

                    }
                    if (vm.results.searchResultItems[i].type == "StreetNetwork") {

                    }
                    if (vm.results.searchResultItems[i].type == "DeliveryPoint") {
                        vm.routeval.push(vm.results.searchResultItems[i]);
                        console.log(vm.routeval);
                    }
                    if (vm.results.searchResultItems[i].type == "Route") {

                    }
               // }
            }
        });
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