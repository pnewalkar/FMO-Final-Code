angular.module('search')
    .factory('searchService', searchService);
searchService.$inject = ['$http', 'GlobalSettings', '$q'];

function searchService(
    $http,
    GlobalSettings,
    $q) {
    var searchService = {};


    searchService.basicSearch = function (searchText) {
        return $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText);
    };

    searchService.advanceSearch = function (searchText) {
        return $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText);
    };

    searchService.GetDeliveryPointByUDPRN = function (udprn) {
        return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + udprn);
    };
    return searchService;
}