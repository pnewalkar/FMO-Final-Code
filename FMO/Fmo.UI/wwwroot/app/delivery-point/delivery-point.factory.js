angular.module('deliveryPoint')
    .factory('deliveryPointAPIService', deliveryPointAPIService)
deliveryPointAPIService.$inject = ['$http', 'GlobalSettings', '$q'];
function deliveryPointAPIService($http, GlobalSettings, $q) {
    var deliveryPointAPIService = {};

    deliveryPointAPIService.GetDeliveryPointsResultSet = function (searchText) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getDeliveryPointsResultSet + searchText).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
              
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.GetAddressByPostCode = function (selectedItem) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getAddressByPostCode + selectedItem).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
               
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.GetAddressLocation = function (udprn) {

        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getAddressLocation + udprn).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
               
                deferred.reject(err);
            });

        return deferred.promise;


    };

    deliveryPointAPIService.GetPostalAddressByGuid = function (addressGuid) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.apiUrl + GlobalSettings.getPostalAddressByGuid + addressGuid).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
                
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.CreateDeliveryPoint = function (addDeliveryPointDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.apiUrl + GlobalSettings.createDeliveryPoint, addDeliveryPointDTO).success(function (response) {
            deferred.resolve(response);

            }).error(function (err, status) {
                
                deferred.reject(err);
            });

        return deferred.promise;
    };

    deliveryPointAPIService.UpdateDeliverypoint = function (deliveryPointModelDTO) {
        var deferred = $q.defer();

        $http.put(GlobalSettings.apiUrl + GlobalSettings.updateDeliverypoint, deliveryPointModelDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    

    return deliveryPointAPIService;

}