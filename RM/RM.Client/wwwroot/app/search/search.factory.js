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
     GetDeliveryPointByGuid: GetDeliveryPointByGuid
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

    function GetDeliveryPointByGuid(Guid) {
        var deferred = $q.defer();
        var getDeliveryPointsGuidParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryPointById, Guid);

        $http.get(GlobalSettings.deliveryPointApiUrl + getDeliveryPointsGuidParams).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

}
