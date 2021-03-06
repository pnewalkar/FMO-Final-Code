angular.module('unitSelector')
    .factory('unitSelectorAPIService', unitSelectorAPIService);

unitSelectorAPIService.$inject = ['$http',
                                  '$q',
                                  'GlobalSettings'];
function unitSelectorAPIService($http,
                                $q,
                                GlobalSettings) {
    return {
        getDeliveryUnit: getDeliveryUnit
    };

    function getDeliveryUnit() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: GlobalSettings.unitManagerApiUrl + GlobalSettings.getDeliveryUnit
        }).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }
}