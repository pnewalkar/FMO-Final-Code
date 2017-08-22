angular.module('deliveryPoint')
        .factory('deliveryPointService', deliveryPointService);

deliveryPointService.$inject = [
    'referencedataApiService',
    '$filter',
    '$q',
    'deliveryPointAPIService',
    'guidService',
    '$mdDialog',
    '$state',
    'mapFactory',
    'referenceDataConstants',
    '$rootScope'
];

function deliveryPointService(
    referencedataApiService,
    $filter,
    $q,
    deliveryPointAPIService,
    guidService,
    $mdDialog,
    $state,
    mapFactory,
    referenceDataConstants,
    $rootScope) {
    vm = this;
    vm.positionedDeliveryPointList = [];

    return {
        initialize: initialize,
        resultSet: resultSet,
        openModalPopup: openModalPopup,
        closeModalPopup: closeModalPopup,
        getPostalAddress: getPostalAddress,
        bindAddressDetails: bindAddressDetails,
        setOrganisation: setOrganisation,
        isUndefinedOrNull: isUndefinedOrNull,
        UpdateDeliverypoint: UpdateDeliverypoint,
        UpdateDeliverypointForRange: UpdateDeliverypointForRange,
        getSubBuildingTypes: getSubBuildingTypes,
        createDeliveryPointsRange: createDeliveryPointsRange,
        reasonCodeValues: reasonCodeValues,
        deleteDeliveryPoint: deleteDeliveryPoint
    };

    function initialize() {
        var deferred = $q.defer();

        var result = { "DeliveryPointTypes": null, "DpUseType": null, "SubBuildingTypes": null, "RangeOptions": null };
        $q.all([
            deliveryPointTypes(),
            deliveryPointUseType(),
           getSubBuildingTypes(),
          getRangeOptions()
        ]).then(function (response) {
            result.DeliveryPointTypes = response[0];
            result.DpUseType = response[1];
            result.SubBuildingTypes = response[2];
            result.RangeOptions = response[3];
            deferred.resolve(result)
        })
        return deferred.promise;
    }

    function deliveryPointTypes() {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.UI_DeliveryPoint_Type.DBCategoryName).then(function (response) {
            deferred.resolve(response.listItems);
        });
        return deferred.promise;
    }

    function deliveryPointUseType() {
        var deferred = $q.defer();

        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.DeliveryPointUseIndicator.DBCategoryName).then(function (response) {
            deferred.resolve(response.listItems);
        });
        return deferred.promise;
    }

    function resultSet(query) {
        var deferred = $q.defer();
        result = [];
        if (query.length >= 3) {
            deliveryPointAPIService.GetDeliveryPointsResultSet(query).then(function (response) {
                deferred.resolve(response);
            });
        }
        else {
            deferred.resolve(result);
        }
        return deferred.promise;
    }

    function openModalPopup(popupSetting) {
        $mdDialog.show(popupSetting)
    }

    function closeModalPopup() {
        $mdDialog.hide();
    }

    function getPostalAddress(postcode) {
        var deferred = $q.defer();
        var result = { "postalAddressData": null, "selectedValue": null, "display": null };
        deliveryPointAPIService.GetAddressByPostCode(postcode).then(function (response) {
            result.postalAddressData = response;
            if (response && response.nybAddressDetails) {
                result.selectedValue = response.nybAddressDetails[0].value;
            }

            if (response) {
                result.display = true;
            }
            else {
                result.display = false;
            }

            deferred.resolve(result);
        });
        return deferred.promise;
    }

    function bindAddressDetails(notyetBuilt) {
        var deferred = $q.defer();
        deliveryPointAPIService.GetPostalAddressByGuid(notyetBuilt)
            .then(function (response) {
                deferred.resolve(response);
            });
        return deferred.promise;
    }

    function setOrganisation(addressDetails, dpUseType) {
        var deferred = $q.defer();
        var result = { "dpUse": null, "selectedDPUse": null };

        if ((addressDetails.organisationName)) {
            result.dpUse = $filter('filter')(dpUseType, {
                value: "Organisation"
            });
            result.selectedDPUse = result.dpUse[0];
        }
        else {
            result.dpUse = $filter('filter')(dpUseType, {
                value: "Residential"
            });
            result.selectedDPUse = result.dpUse[0];
        }
        deferred.resolve(result);
        return deferred.promise;
    }

    function isUndefinedOrNull(obj) {
        if (obj !== null && angular.isDefined(obj)) {
            return obj;
        }
        else {
            return "";
        }
    }

    function UpdateDeliverypoint(positionedDeliveryPointList) {
        var deferred = $q.defer();
        vm.positionedDeliveryPointList = positionedDeliveryPointList;
        deliveryPointAPIService.UpdateDeliverypoint(positionedDeliveryPointList[0]).then(function (result) {
            $rootScope.$broadcast('disablePrintMap', {
                disable: false
            });
            mapFactory.setAccessLink();
            mapFactory.setDeliveryPointOnLoad(result.xCoordinate, result.yCoordinate, true);
            guidService.setGuid(result.id);
            $state.go('deliveryPoint', { positionedDeliveryPointList: vm.positionedDeliveryPointList });
        });
        return deferred.promise;
    }

    function UpdateDeliverypointForRange(positionedDeliveryPointList) {
        var deferred = $q.defer();
        var isSingle = false;
        var xCoordinate = undefined;
        var yCoordinate = undefined;
        vm.positionedDeliveryPointList = positionedDeliveryPointList;
        deliveryPointAPIService.UpdateDeliverypointForRange(positionedDeliveryPointList).then(function (result) {
            result
            $rootScope.$broadcast('disablePrintMap', {
                disable: false
            });
            mapFactory.setAccessLink();
            if(result && result.length === 1){
                isSingle = true;
                xCoordinate = result[0].xCoordinate;
                yCoordinate = result[0].yCoordinate;
            }
            mapFactory.setDeliveryPointOnLoad(xCoordinate, yCoordinate, isSingle);
            mapFactory.setAccessLink();
            $state.go('deliveryPoint', { positionedDeliveryPointList: vm.positionedDeliveryPointList });
            deferred.resolve(result);
        }).catch(function (err) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    function getSubBuildingTypes() {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.SubBuildingType.DBCategoryName).then(function (response) {
            deferred.resolve(response.listItems);
        });
        return deferred.promise;
    }

    function getRangeOptions() {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.UI_Range_Options.DBCategoryName).then(function (response) {
            deferred.resolve(response.listItems);
        });
        return deferred.promise;
    }

    


    function createDeliveryPointsRange(postalAddressDetails) {
        var deferred = $q.defer();
        deliveryPointAPIService.createDeliveryPointsRange(postalAddressDetails).then(function (result) {
            deferred.resolve(result);
        }).catch(function (err) {
            deferred.reject(err);
        });
        return deferred.promise;
    }

    function reasonCodeValues() {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.UI_Delete_DP_ReasonCode.AppCategoryName).then(function (response) { 
            deferred.resolve(response);   
        });
        return deferred.promise;
        }
    function deleteDeliveryPoint(id, reasonCode, reasonText) {
        var deferred = $q.defer();
        deliveryPointAPIService.deleteDeliveryPoint(id, reasonCode, reasonText).then(function (result) {
            deferred.resolve(result);
        });
        return deferred.promise;
    }
}