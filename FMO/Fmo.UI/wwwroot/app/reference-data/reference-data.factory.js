angular.module('referencedata')
    .factory('referencedataApiService', referencedataApiService);
referencedataApiService.$inject = ['$http', '$q', 'GlobalSettings'];

function referencedataApiService($http, $q, GlobalSettings) {

    return {
        getReferenceData: getReferenceData,
        readJson: readJson
    };

    function getReferenceData() {
        return $http.get(GlobalSettings.getReferenceData);
    }

    function readJson() {
        var deferred = $q.defer();

        $http.get(GlobalSettings.readJson).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            alert(err);
            deferred.reject(err);
        });

        return deferred.promise;
    }

}