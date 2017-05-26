angular.module('search')
    .factory('searchApiService', searchApiService);
searchApiService.$inject = ['$http', 'GlobalSettings', '$q'];

function searchApiService($http, GlobalSettings, $q) {
    var searchApiService = {};


    //searchApiService.basicSearch = function (searchText) {
    //    return $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText);

    //};

    searchApiService.basicSearch = function (searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + '/Search/BasicSearch?searchText=' + searchText).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
           //deferred.reject(err);
           return $q.reject(err);
        });
        return deferred.promise;
    }

    searchApiService.advanceSearch = function (searchText) {
        return $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText);

    };

    searchApiService.GetDeliveryPointByUDPRN = function (udprn) {
        return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + udprn);
    };
    return searchApiService;

}