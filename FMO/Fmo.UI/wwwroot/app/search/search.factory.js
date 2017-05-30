angular.module('search')
    .factory('searchService', searchService);
searchService.$inject = [
'$http',
'GlobalSettings',
'$q'];

function searchService(
    $http,
    GlobalSettings,
    $q) {

    return {
     basicSearch: basicSearch,
     advanceSearch: advanceSearch,
     GetDeliveryPointByUDPRN:GetDeliveryPointByUDPRN
   }

    function basicSearch(searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + GlobalSettings.fetchBasicSearchResults + searchText).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
           // deferred.reject(err);
            return $q.reject(err);
        });
        return deferred.promise;
    }

    function advanceSearch(searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + GlobalSettings.fetchAdvanceSearchResults + searchText).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    function GetDeliveryPointByUDPRN(udprn) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + GlobalSettings.getDeliveryPointByUDPRN + udprn).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

}
