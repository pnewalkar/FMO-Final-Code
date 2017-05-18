angular.module('accessLink')
    .factory('accessLinkApiService', accessLinkApiService)
accessLinkApiService.$inject = ['$http', 'GlobalSettings', '$q'];
function accessLinkApiService($http, GlobalSettings, $q) {
    var accessLinkApiService = {};

    accessLinkApiService.CreateAccessLink = function (accessLinkDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + '/accessLink/Create/', accessLinkDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {

            deferred.reject(err);
        });

        return deferred.promise;
    };
}