
// Just for reference
angular.module('unitSelector')
    .factory('unitSelectorAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var unitSelectorAPIService = {};

        var getDeliveryUnit = function () {
            return $http.get(GlobalSettings.apiUrl + '/RouteLog/DeliveryUnit');
        };
      
        unitSelectorAPIService.GetDeliveryUnit = getDeliveryUnit;      
        return simulationAPIService;

    }]);