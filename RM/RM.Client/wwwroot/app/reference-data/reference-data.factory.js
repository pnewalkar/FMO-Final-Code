angular.module('referencedata')
    .factory('referencedataApiService', referencedataApiService);
referencedataApiService.$inject = ['$http', '$q', 'GlobalSettings'];

function referencedataApiService($http, $q, GlobalSettings) {

    return {
        getReferenceData: getReferenceData,
        readJson: readJson,
        getSimpleListsReferenceData: getSimpleListsReferenceData,
        getNameValueReferenceData: getNameValueReferenceData
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

    function getSimpleListsReferenceData(listName) {
        var deferred = $q.defer();
       
        $http.get(GlobalSettings.referenceDataApiUrl + '/ReferenceDataManager/simpleLists?listName=' + listName).success(function (response) {
            deferred.resolve(response.item2);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }
    function getNameValueReferenceData(appGroupName) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.referenceDataApiUrl + '/ReferenceDataManager/nameValuePairs?appGroupName=' + appGroupName).success(function (response) {
            deferred.resolve(response.item2);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

}