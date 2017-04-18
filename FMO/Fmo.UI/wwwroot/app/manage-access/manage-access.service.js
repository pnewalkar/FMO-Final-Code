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

        //var data = "username=" + userdata.userName + "&password=" + "pass";

        var deferred = $q.defer();

        $http.post('http://localhost:34583' + '/api/token', userdata, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            console.log(err);
            deferred.reject(err);
        });

        return deferred.promise;


        //JSON.stringify(userdata)
        //var resp = $http({
        //    url: "http://localhost:34583/api/token",
        //    method: "POST",
        //    data: JSON.stringify(userdata),
        //    headers: {
        //        'Content-Type': 'application/x-www-form-urlencoded'
        //    },
        //});

        //return $http.post(GlobalSettings.apiUrl + '/token', 'username=TEST&password=TEST123');

        //$http({
        //    method: 'POST',
        //    url: GlobalSettings.apiUrl + '/token',
        //    data: JSON.stringify(username=TEST&password=TEST123,
        //    headers: {
        //        'Content-Type': 'application/x-www-form-urlencoded'
        //    }
        //}).then(function (result) {
        //    console.log(result);
        //    return result;
        //}, function (error) {
        //    console.log(error);
        //});
    }
}