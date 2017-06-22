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
    'referenceDataConstants'
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
    referenceDataConstants) {
    vm = this;
    vm.positionedDeliveryPointList = [];

    return {
        deliveryPointTypes: deliveryPointTypes,
        deliveryPointUseType: deliveryPointUseType,
        resultSet: resultSet,
        openModalPopup: openModalPopup,
        closeModalPopup: closeModalPopup,
        getPostalAddress: getPostalAddress,
        bindAddressDetails: bindAddressDetails,
        setOrganisation: setOrganisation,
        isUndefinedOrNull: isUndefinedOrNull,
        UpdateDeliverypoint: UpdateDeliverypoint
    };

    //function initialize() {
    //    var deferred = $q.defer();
    //    var referenceData = {};
    //    deliveryPointTypes().then(function (response) {
    //        referenceData.deliveryPointTypes = response;
    //        deferred.resolve(referenceData);

    //    });
    //    deliveryPointUseType().then(function (response) {
    //        referenceData.dpUseType = response;
    //        deferred.resolve(referenceData);
    //    });
    //    return deferred.promise;
    //}

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
        var result = {"postalAddressData": null, "selectedValue": null, "display": null };
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
        vm.positionedDeliveryPointList = positionedDeliveryPointList;
        deliveryPointAPIService.UpdateDeliverypoint(positionedDeliveryPointList[0]).then(function (result) {
            mapFactory.setAccessLink();
            mapFactory.setDeliveryPointOnLoad(result.xCoordinate, result.yCoordinate);
            guidService.setGuid(result.id);
            $state.go('deliveryPoint', { positionedDeliveryPointList: vm.positionedDeliveryPointList });
        });
    }
}