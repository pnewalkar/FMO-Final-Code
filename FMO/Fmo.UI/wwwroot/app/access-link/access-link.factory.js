angular.module('accessLink')
    .factory('accessLinkAPIService', accessLinkAPIService)
accessLinkAPIService.$inject = ['$http', 'GlobalSettings', '$q'];
function accessLinkAPIService($http, GlobalSettings, $q) {
    var accessLinkAPIService = {};

    accessLinkAPIService.CreateAccessLink = function (accessLinkDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + '/accessLink/Create/', accessLinkDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };
}