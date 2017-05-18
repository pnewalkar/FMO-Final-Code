angular.module('advanceSearch')
    .factory('advanceSearchApiService', advanceSearchApiService);
advanceSearchApiService.$inject = [
    '$http',
    'GlobalSettings',
    '$q'];

function advanceSearchApiService(
    $http,
    GlobalSettings,
    $q)
    {
    var advanceSearchApiService = {};
 

    advanceSearchApiService.advanceSearch = function (searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    return advanceSearchApiService;

}