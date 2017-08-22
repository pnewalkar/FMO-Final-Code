'use strict';
angular.module('group')
    .factory('groupAPIService', groupAPIService);
groupAPIService.$inject = [
'$http',
'GlobalSettings',
'$q'];

function groupAPIService($http, GlobalSettings, $q) {
    var groupAPIService = {};

    groupAPIService.CreateGroup = function (groupDto) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.deliveryPointGroupManagerApiUrl + GlobalSettings.CreateDeliveryGroup, groupDto).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    return groupAPIService;
}