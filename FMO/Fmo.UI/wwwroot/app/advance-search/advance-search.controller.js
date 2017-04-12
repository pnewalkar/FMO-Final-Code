'use strict';
angular.module('advanceSearch')
    .controller('advanceSearchController', ['$scope', 'searchApiService', AdvanceSearchController]);

function AdvanceSearchController($scope, searchApiService) {
    var vm = this;
    debugger;

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