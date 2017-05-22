angular.module('deliveryPoint')
    .factory('deliveryPointAPIService', deliveryPointAPIService)
deliveryPointAPIService.$inject = ['$http', 'GlobalSettings', '$q'];
function deliveryPointAPIService($http, GlobalSettings, $q) {
    var deliveryPointAPIService = {};

    deliveryPointAPIService.GetDeliveryPointsResultSet = function (searchText) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/address/SearchAddressdetails?searchText=' + searchText).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
              
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.GetAddressByPostCode = function (selectedItem) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/address/GetAddressByPostCode?selectedItem=' + selectedItem).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
               
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.GetAddressLocation = function (udprn) {

        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/addresslocation/GetAddressLocationByUDPRN?udprn=' + udprn).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
               
                deferred.reject(err);
            });

        return deferred.promise;


    };

    deliveryPointAPIService.GetPostalAddressByGuid = function (addressGuid) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/address/GetPostalAddressByGuid?addressGuid=' + addressGuid).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
                
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.CreateDeliveryPoint = function (addDeliveryPointDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + '/deliveryPoints/CreateDeliveryPoint/', addDeliveryPointDTO).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
                
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.UpdateDeliverypoint = function (deliveryPointModelDTO) {
        var deferred = $q.defer();

        $http.put(GlobalSettings.apiUrl + '/deliveryPoints/UpdateDeliveryPoint/', deliveryPointModelDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    

    return deliveryPointAPIService;

}