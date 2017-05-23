angular.module('advanceSearch')
    .factory('advanceSearchAPIService', advanceSearchAPIService);
advanceSearchAPIService.$inject = [
    '$http',
    'GlobalSettings',
    '$q'];

function advanceSearchAPIService(
    $http,
    GlobalSettings,
    $q)
    {
    var advanceSearchAPIService = {};
 

    advanceSearchAPIService.advanceSearch = function (searchText) {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + '/Search/AdvanceSearch?searchText=' + searchText).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    return advanceSearchAPIService;

}