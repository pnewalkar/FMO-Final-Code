
// Just for reference
angular.module('unitSelector')
    .factory('unitSelectorAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var unitSelectorAPIService = {};

        unitSelectorAPIService.getDeliveryUnit = function () {
            return $http({
                method: 'GET',
                url: GlobalSettings.apiUrl + '/RouteLog/DeliveryUnit'
            });
        };

        return unitSelectorAPIService;
    }]);