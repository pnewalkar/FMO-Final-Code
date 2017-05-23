
angular.module('layers')
    .factory('layersAPIService', layersAPIService);
layersAPIService.$inject = [
'$http',
'GlobalSettings',
'$q'];

function layersAPIService($http, GlobalSettings, $q) {
    var layersAPIService = {};
    layersAPIService.fetchDeliveryPoints = function (extent, authData) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: GlobalSettings.apiUrl + '/deliveryPoints/GetDeliveryPoints?boundaryBox=' + extent.join(','),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': 'Bearer ' + authData.token
            }
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    layersAPIService.fetchAccessLinks = function (extent, authData) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: GlobalSettings.apiUrl + '/accessLink/GetAccessLinks?boundaryBox=' + extent.join(','),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': 'Bearer ' + authData.token
            }
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    layersAPIService.fetchRouteLinks = function (extent, authData) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: GlobalSettings.apiUrl + '/roadName/GetRouteLinks?boundaryBox=' + extent.join(','),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': 'Bearer ' + authData.token
            }
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    return layersAPIService;
}