angular.module('accessLink')
    .factory('accessLinkAPIService', accessLinkAPIService)
accessLinkAPIService.$inject = ['$http', 'GlobalSettings', '$q'];
function accessLinkAPIService($http, GlobalSettings, $q) {
    var accessLinkAPIService = {};

    accessLinkAPIService.GetAdjPathLength = function (accessLinkManualCreateModelDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + GlobalSettings.getAdjustedPathLength, accessLinkManualCreateModelDTO).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
              
                deferred.reject(err);
            });

        return deferred.promise;
    };

    accessLinkAPIService.CreateAccessLink = function (accessLinkDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + GlobalSettings.createAccessLink, accessLinkDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    accessLinkAPIService.CheckAccessLinkIsValid = function (accessLinkManualCreateModelDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + GlobalSettings.checkAccessLinkIsValid, accessLinkManualCreateModelDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };

    return accessLinkAPIService;
}