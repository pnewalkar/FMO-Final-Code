angular.module('search')
    .factory('searchApiService',
    ['$http', 'GlobalSettings', '$q', function ($http, GlobalSettings, $q) {
        var searchApiService = {};
 

        searchApiService.basicSearch = function (searchText) {
            return $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText);
            
        };

        searchApiService.advanceSearch = function (searchText) {
            return $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText);

        };

        searchApiService.GetDeliveryPointByUDPRN = function (udprn) {
            return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + udprn);
        };
        return searchApiService;

    }]);