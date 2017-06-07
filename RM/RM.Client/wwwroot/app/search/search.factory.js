angular.module('search')
    .factory('searchService', searchService);
searchService.$inject = [
'$http',
'GlobalSettings',
'$q',
'stringFormatService'];

function searchService(
    $http,
    GlobalSettings,
    $q,
    stringFormatService) {

    return {
     basicSearch: basicSearch,
     advanceSearch: advanceSearch,
     GetDeliveryPointByUDPRN:GetDeliveryPointByUDPRN
   }

    function basicSearch(searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.searchManagerApiUrl + GlobalSettings.fetchBasicSearchResults + searchText).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
           // deferred.reject(err);
            return $q.reject(err);
        });
        return deferred.promise;
    }

    function advanceSearch(searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.searchManagerApiUrl + GlobalSettings.fetchAdvanceSearchResults + searchText).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    function GetDeliveryPointByUDPRN(udprn) {
        var deferred = $q.defer();
        var getDeliveryPointsUDPRNParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryPointByUDPRN, udprn);

        $http.get(GlobalSettings.deliveryPointApiUrl + getDeliveryPointsUDPRNParams).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

}
