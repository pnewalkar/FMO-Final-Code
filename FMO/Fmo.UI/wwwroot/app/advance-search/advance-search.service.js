


angular.module('advanceSearch').service('advanceSearchService', function () {

    this.advanceSearch = function () {
        return {
            templateUrl: './advance-search/advance-search.template.html',
            clickOutsideToClose: true,
            controller: 'advanceSearchController as vm'
        }
    };
});

