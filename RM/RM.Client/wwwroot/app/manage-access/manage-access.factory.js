'use strict';

angular
    .module('manageAccess')
    .factory('manageAccessAPIService', manageAccessAPIService);

manageAccessAPIService.$inject = ['$http',
    '$q',
    'GlobalSettings'];

function manageAccessAPIService(
    $http,
    $q,
    GlobalSettings)
    {
        var service = {
            getToken: getToken
        };

        return service;

        function getToken(userdata) {
            var deferred = $q.defer();

            $http.post(GlobalSettings.actionManagerApiUrl + GlobalSettings.getToken, userdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
                deferred.resolve(response);

            }).error(function (err, status) {
                alert(err);
                deferred.reject(err);
            });

            return deferred.promise;
        }
    }