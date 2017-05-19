
angular.module('layers')
    .factory('layersService', layersService);
layersService.$inject = [
'$http',
'GlobalSettings',
'$q'];

function layersService($http, GlobalSettings, $q) {
    var layersService = {};

    layersService.fetchDeliveryPoints = function (extent, authData) {
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

    layersService.fetchAccessLinks = function (extent, authData) {
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

    layersService.fetchRouteLinks = function (extent, authData) {
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
    return layersService;
}