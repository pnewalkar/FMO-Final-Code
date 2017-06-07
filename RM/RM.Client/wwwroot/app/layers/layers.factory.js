
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
            url: GlobalSettings.deliveryPointApiUrl + GlobalSettings.fetchDeliveryPointsByBoundingBox + extent.join(','),
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
            url: GlobalSettings.accessLinkApiUrl + GlobalSettings.fetchAccessLinksByBoundingBox + extent.join(','),
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
            url: GlobalSettings.networkManagerApiUrl + GlobalSettings.fetchRouteLinksByBoundingBox + extent.join(','),
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