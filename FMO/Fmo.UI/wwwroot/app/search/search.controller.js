'use strict';
angular.module('search')
    .directive('ngEnter', function () {
        return function (scope, element, attrs) {
            element.bind("keydown keypress", function (event) {
                if(event.which === 13) {
                    scope.$apply(function (){
                        scope.$eval(attrs.ngEnter);
                    });

                    event.preventDefault();
                }
            });
        };
    })
       .controller('SearchController', SearchController);

function SearchController($timeout, $q, $log, searchApiService,$scope) {
    var self = this;

    // list of `state` value/display objects
    self.states = loadAll();
    self.resultSet = resultSet;
    self.presEnter = presEnter;

    // ******************************
    // Internal methods
    // ******************************

    /**
     * Search for states... use $timeout to simulate
     * remote dataservice call.
     */
    function querySearch(query) {
        var results = query ? self.states.filter(createFilterFor(query)) : self.states,
            deferred;
        if (self.simulateQuery) {
        var deferred = $q.defer();
        deferred = $q.defer();
        $timeout(function () { deferred.resolve(results); }, Math.random() * 1000, false);
            return deferred.promise;
        } else {
            return results;
        }
        //var results = loadAll(query);
    }

    function resultSet(query) {
        var result = querySearch(query);
        if (result.length > 5) {
            
        }
        else {
            
        }
        self.results = result;
       // return result;
    }

    function presEnter() {
        if ($scope.vm.searchText.length > 3) {
            if ($scope.results > 0) {

            }
        }
        else {
            //alert("At least three characters must be input for a Search");
           self.results = [{ value: "At least three characters must be input for a Search", display: "At least three characters must be input for a Search" }];
        }
    }

    /**
     * Build `states` list of key/value pairs
     */
    function loadAll() {
        var allStates = 'Alabama, Alaska, Arizona, Arkansas, California, Colorado, Connecticut, Delaware,\
              Florida, Georgia, Hawaii, Idaho, Illinois, Indiana, Iowa, Kansas, Kentucky, Louisiana,\
              Maine, Maryland, Massachusetts, Michigan, Minnesota, Mississippi, Missouri, Montana,\
              Nebraska, Nevada, New Hampshire, New Jersey, New Mexico, New York, North Carolina,\
              North Dakota, Ohio, Oklahoma, Oregon, Pennsylvania, Rhode Island, South Carolina,\
              South Dakota, Tennessee, Texas, Utah, Vermont, Virginia, Washington, West Virginia,\
              Wisconsin, Wyoming';

        return allStates.split(/, +/g).map(function (state) {
            return {
                value: state.toLowerCase(),
                display: state
            };
        });

        //searchApiService.basicSearch(query).then(function (response) {
        //    var data = response.data;
        //});

    }

    /**
     * Create filter function for a query string
     */
    function createFilterFor(query) {
        var lowercaseQuery = angular.lowercase(query);

        return function filterFn(state) {
            return (state.value.indexOf(lowercaseQuery) === 0);
        };

    }
}
