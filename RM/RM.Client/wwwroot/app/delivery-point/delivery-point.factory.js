angular.module('deliveryPoint')
    .factory('deliveryPointAPIService', deliveryPointAPIService)
deliveryPointAPIService.$inject = ['$http', 'GlobalSettings', '$q', 'stringFormatService'];
function deliveryPointAPIService($http, GlobalSettings, $q, stringFormatService) {
    var vm = this;
    function GetDeliveryPointsResultSet(searchText) {
        var deferred = $q.defer();

        var getDeliveryPointsParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryPointsResultSet, searchText);
        $http.get(GlobalSettings.unitManagerApiUrl + getDeliveryPointsParams).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function GetAddressByPostCode(selectedItem) {
        var deferred = $q.defer();

        var getAddressByPostCodeParams = stringFormatService.Stringformat(GlobalSettings.getAddressByPostCode, selectedItem);
        $http.get(GlobalSettings.unitManagerApiUrl + getAddressByPostCodeParams).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function GetAddressLocation(udprn) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.thirdPartyAddressLocationApiUrl + GlobalSettings.getAddressLocation + udprn).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function GetPostalAddressByGuid(addressGuid) {
        var deferred = $q.defer();

        var getPostalAddressByGuidParams = stringFormatService.Stringformat(GlobalSettings.getPostalAddressByGuid, addressGuid);
        $http.get(GlobalSettings.postalAddressApiUrl + getPostalAddressByGuidParams).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function CreateDeliveryPoint(addDeliveryPointDTO) {
        var deferred = $q.defer();
        var deliveryPointApi = addDeliveryPointDTO.DeliveryPointType === GlobalSettings.single ? GlobalSettings.createDeliveryPoint : GlobalSettings.validateDeliveryPoints;
        $http.post(GlobalSettings.deliveryPointApiUrl + deliveryPointApi, addDeliveryPointDTO).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function UpdateDeliverypoint(deliveryPointModelDTO) {
        var deferred = $q.defer();

        $http.put(GlobalSettings.deliveryPointApiUrl + GlobalSettings.updateDeliverypoint, deliveryPointModelDTO).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }
    
    function UpdateDeliverypointForRange(deliveryPointModelDTOs) {
        var deferred = $q.defer();

        $http.put(GlobalSettings.deliveryPointApiUrl + GlobalSettings.updateDeliverypointForRange, deliveryPointModelDTOs).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function createDeliveryPointsRange(postalAddressDetails) {
        var deferred = $q.defer();
        $http.post(GlobalSettings.deliveryPointApiUrl + GlobalSettings.createDeliveryPointsRange, postalAddressDetails).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    function deleteDeliveryPoint(deliveryPointId, reasonCode, reasonText) {
        var deferred = $q.defer();
        var deleteDeliveryPointParams = stringFormatService.Stringformat(GlobalSettings.deleteDeliveryPoint, deliveryPointId, reasonCode, reasonText);
        $http.delete(GlobalSettings.deliveryPointApiUrl + deleteDeliveryPointParams).then(function (response) {
            deferred.resolve(response.data);
        }).catch(function (err) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    return {
        GetDeliveryPointsResultSet: GetDeliveryPointsResultSet,
        GetAddressByPostCode: GetAddressByPostCode,
        GetAddressLocation: GetAddressLocation,
        GetPostalAddressByGuid: GetPostalAddressByGuid,
        CreateDeliveryPoint: CreateDeliveryPoint,
        UpdateDeliverypoint: UpdateDeliverypoint,
        UpdateDeliverypointForRange: UpdateDeliverypointForRange,
        createDeliveryPointsRange: createDeliveryPointsRange,
        deleteDeliveryPoint: deleteDeliveryPoint
    }

}
    /*var deliveryPointAPIService = {};

    deliveryPointAPIService.GetDeliveryPointsResultSet = function (searchText) {
        var deferred = $q.defer();

        var getDeliveryPointsParams = stringFormatService.Stringformat(GlobalSettings.getDeliveryPointsResultSet, searchText);
        $http.get(GlobalSettings.unitManagerApiUrl + getDeliveryPointsParams).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.GetAddressByPostCode = function (selectedItem) {
        var deferred = $q.defer();

        var getAddressByPostCodeParams = stringFormatService.Stringformat(GlobalSettings.getAddressByPostCode, selectedItem);
        $http.get(GlobalSettings.unitManagerApiUrl + getAddressByPostCodeParams).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.GetAddressLocation = function (udprn) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.thirdPartyAddressLocationApiUrl + GlobalSettings.getAddressLocation + udprn).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.GetPostalAddressByGuid = function (addressGuid) {
        var deferred = $q.defer();

        var getPostalAddressByGuidParams = stringFormatService.Stringformat(GlobalSettings.getPostalAddressByGuid, addressGuid);
        $http.get(GlobalSettings.postalAddressApiUrl + getPostalAddressByGuidParams).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.CreateDeliveryPoint = function (addDeliveryPointDTO) {
        var deferred = $q.defer();
        var deliveryPointApi = addDeliveryPointDTO.DeliveryPointType === GlobalSettings.single ? GlobalSettings.createDeliveryPoint : GlobalSettings.validateDeliveryPoints;
        $http.post(GlobalSettings.deliveryPointApiUrl + deliveryPointApi, addDeliveryPointDTO).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.UpdateDeliverypoint = function (deliveryPointModelDTO) {
        var deferred = $q.defer();

        $http.put(GlobalSettings.deliveryPointApiUrl + GlobalSettings.updateDeliverypoint, deliveryPointModelDTO).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.UpdateDeliverypointForRange = function (deliveryPointModelDTOs) {
        var deferred = $q.defer();

        $http.put(GlobalSettings.deliveryPointApiUrl + GlobalSettings.updateDeliverypointForRange, deliveryPointModelDTOs).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    deliveryPointAPIService.createDeliveryPointsRange = function (postalAddressDetails) {
        var deferred = $q.defer();
        $http.post(GlobalSettings.deliveryPointApiUrl + GlobalSettings.createDeliveryPointsRange, postalAddressDetails).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    deliveryPointAPIService.deleteDeliveryPoint = function (deliveryPointId, reasonCode, reasonText) {
        var deferred = $q.defer();
        var deleteDeliveryPointParams = stringFormatService.Stringformat(GlobalSettings.deleteDeliveryPoint, deliveryPointId, reasonCode, reasonText);
        $http.delete(GlobalSettings.deliveryPointApiUrl + deleteDeliveryPointParams).success(function (response) {
        deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    
    return deliveryPointAPIService;
}*/