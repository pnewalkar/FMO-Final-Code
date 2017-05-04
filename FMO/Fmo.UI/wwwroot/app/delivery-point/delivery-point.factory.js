angular.module('deliveryPoint')
    .factory('deliveryPointApiService',
    ['$http', 'GlobalSettings', '$q', function ($http, GlobalSettings, $q) {
        var deliveryPointApiService = {};




        deliveryPointApiService.GetDeliveryPointsResultSet = function (searchText) {
            return $http.get(GlobalSettings.apiUrl + '/address/SearchAddressdetails?searchText=' + searchText);
        };

        deliveryPointApiService.GetAddressdetails = function (postCode) {
            return $http.get(GlobalSettings.apiUrl + '/address/GetAddressdetails?postCode=' + postCode);
        };
        return deliveryPointApiService;

    }]);