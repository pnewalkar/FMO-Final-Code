
// Just for reference
angular.module('unitSelector')
    .factory('unitSelectorAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var unitSelectorAPIService = {};

        unitSelectorAPIService.getDeliveryUnit = function () {
            var deferred = $q.defer();

            $http({
                method: 'GET',
                url: GlobalSettings.apiUrl + '/UnitLocation/DeliveryUnitsForUser'
            }).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            console.log(err);
            deferred.reject(err);
        });

            return deferred.promise;

        }

        return unitSelectorAPIService;
    }]);