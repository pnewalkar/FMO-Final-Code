angular.module('deliveryPoint')
    .factory('deliveryPointApiService', deliveryPointApiService)
deliveryPointApiService.$inject = ['$http', 'GlobalSettings', '$q'];
function deliveryPointApiService($http, GlobalSettings, $q) {
    var deliveryPointApiService = {};

    deliveryPointApiService.GetDeliveryPointsResultSet = function (searchText) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/address/SearchAddressdetails?searchText=' + searchText).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
              
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointApiService.GetAddressByPostCode = function (selectedItem) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/address/GetAddressByPostCode?selectedItem=' + selectedItem).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
               
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointApiService.GetAddressLocation = function (udprn) {

        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/addresslocation/GetAddressLocationByUDPRN?udprn=' + udprn).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
               
                deferred.reject(err);
            });

        return deferred.promise;


    };

    deliveryPointApiService.GetPostalAddressByGuid = function (addressGuid) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + '/address/GetPostalAddressByGuid?addressGuid=' + addressGuid).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
                
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointApiService.CreateDeliveryPoint = function (addDeliveryPointDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + '/deliveryPoints/CreateDeliveryPoint/', addDeliveryPointDTO).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
                
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointApiService.UpdateDeliverypoint = function (deliveryPointModelDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + '/deliveryPoints/UpdateDeliveryPoint/', deliveryPointModelDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    

    return deliveryPointApiService;

}