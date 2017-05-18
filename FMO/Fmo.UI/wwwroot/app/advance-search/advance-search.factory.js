angular.module('advanceSearch')
    .factory('advanceSearchApiService', advanceSearchApiService);
advanceSearchApiService.$inject = ['$http', 'GlobalSettings', '$q'];

function advanceSearchApiService($http, GlobalSettings, $q) {
    var advanceSearchApiService = {};
 

    advanceSearchApiService.advanceSearch = function (searchText) {
        return $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText);

    };

    return advanceSearchApiService;

}