angular.module('deliveryPoint')
    .factory('deliveryPointApiService',
    ['$http', 'GlobalSettings', '$q', function ($http, GlobalSettings, $q) {
        var deliveryPointApiService = {};




        deliveryPointApiService.GetDeliveryPointsResultSet = function (searchText) {
            return $http.get(GlobalSettings.apiUrl + '/address/SearchAddressdetails?searchText=' + searchText);
        };

        deliveryPointApiService.GetAddressByPostCode = function (postCode) {
            return $http.get(GlobalSettings.apiUrl + '/address/GetAddressByPostCode?postCode=' + postCode);
        };

        deliveryPointApiService.GetAddressLocation = function (udprn) {
            return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/GetAddressLocationByUDPRN?udprn=' + udprn);
        };
        return deliveryPointApiService;

    }]);