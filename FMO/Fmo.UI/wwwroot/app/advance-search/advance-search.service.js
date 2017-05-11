angular.module('advanceSearch')
    .factory('advanceSearchService', advanceSearchService);

function advanceSearchService()
{
    return {
        advanceSearch: advanceSearch
    };
      function advanceSearch(query) {
        return {
            templateUrl: './advance-search/advance-search.template.html',
            clickOutsideToClose: false,
            controller: 'advanceSearchController as vm',
            params: { searchText: query }
        }
    }
};

