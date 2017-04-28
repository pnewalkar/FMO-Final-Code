


angular.module('advanceSearch').service('advanceSearchService', function () {

    this.advanceSearch = function (query) {
        return {
            templateUrl: './advance-search/advance-search.template.html',
            clickOutsideToClose: false,
            controller: 'advanceSearchController as vm',
            params: { searchText: query, }
        }
    };
});

