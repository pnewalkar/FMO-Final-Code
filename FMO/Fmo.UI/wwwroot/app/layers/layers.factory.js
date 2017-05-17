
angular.module('layers')
    .factory('layersApiService', layersApiService);
layersApiService.$inject = ['$http', 'GlobalSettings', '$q'];

function layersApiService($http, GlobalSettings, $q) {
    var layersApiService = {};

    layersApiService.fetchDeliveryPoints = function () {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + '/deliveryPoints/fetchDeliveryPoint').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    layersApiService.fetchAccessLinks = function () {
        var deferred = $q.defer();
        $http.get(GlobalSettings.apiUrl + '/accessLinks/fetchAccessLink').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    return layersApiService;
}