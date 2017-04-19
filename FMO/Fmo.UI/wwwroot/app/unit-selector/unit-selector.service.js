
// Just for reference
angular.module('unitSelector')
    .factory('unitSelectorAPIService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var unitSelectorAPIService = {};

        unitSelectorAPIService.getDeliveryUnit = function () {
            var aValue = sessionStorage.getItem('authorizationData');
            var jobject = JSON.parse(aValue)
            //if (jobject)
            //return $http.get('/api/RouteLog/DeliveryUnit', { headers: { 'Authorization': 'Bearer ' + jobject.token } });
            // return $http.get(GlobalSettings.apiUrl + '/RouteLog/DeliveryUnit');
            if (jobject) {
                return $http({
                    method: 'GET',                    
                    url: GlobalSettings.apiUrl + '/RouteLog/DeliveryUnit'
                });
            }
        };

        return unitSelectorAPIService;

    }]);