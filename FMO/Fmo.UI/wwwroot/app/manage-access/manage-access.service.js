'use strict';

angular
    .module('manageAccess')
    .factory('manageAccessService', manageAccessService);

manageAccessService.$inject = ['$http', '$q', 'GlobalSettings'];

function manageAccessService($http, $q, GlobalSettings) {
    var service = {
        getToken: getToken
    };

    return service;

    function getToken(userdata) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + '/token', userdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            alert(err);
            deferred.reject(err);
        });

        return deferred.promise;

    }
}