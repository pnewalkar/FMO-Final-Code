angular.module('unitSelector')
    .service('unitSelectorService', unitSelectorService);

unitSelectorService.$inject = ['$http', '$q', 'GlobalSettings', '$filter', 'manageAccessBusinessService', 'mapFactory', 'unitSelectorAPIService'];
function unitSelectorService($http, $q, GlobalSettings, $filter, manageAccessBusinessService, mapFactory, unitSelectorAPIService) {


    var selectedDeliveryUnit = null;
    var deliveryRouteUnit = [];
    var isDeliveryUnitDisabled = false;


    return {
        DeliveryUnit: DeliveryUnit,
        BindData: BindData
    };

    function DeliveryUnit(selectedDeliveryUnit) {
        manageAccessBusinessService.activate(selectedDeliveryUnit.id);
        updateMapAfterUnitChange(selectedDeliveryUnit);
    }

    function BindData(deliveryRouteUnit) {
        if (deliveryRouteUnit.length === 0) {
            var authData = sessionStorage.getItem('authorizationData');
            authData = angular.fromJson(authData);
            if (authData.unitGuid) {
                unitSelectorAPIService.getDeliveryUnit().then(function (response) {
                    if (response) {
                        deliveryRouteUnit = response;
                        var newTemp = $filter("filter")(deliveryRouteUnit, { id: authData.unitGuid });
                        selectedUser = newTemp[0];
                        selectedDeliveryUnit = selectedUser;
                        updateMapAfterUnitChange(selectedDeliveryUnit);

                        return {
                            deliveryRouteUnit: deliveryRouteUnit,
                            selectedUser: selectedUser,
                            selectedDeliveryUnit: selectedDeliveryUnit

                        };
                    }
                });

            } else {
                var deferred = $q.defer();
                unitSelectorAPIService.getDeliveryUnit().then(function (response) {
                    if (response) {
                        if (response.length === 1) {
                            isDeliveryUnitDisabled = true;
                        }
                        deliveryRouteUnit = response;
                        selectedUser = deliveryRouteUnit[0];
                        selectedDeliveryUnit = selectedUser;
                        updateMapAfterUnitChange(selectedDeliveryUnit);
                        deferred.resolve(response);
                    }
                });

                return deferred.promise;
            }
        }
    }

    function updateMapAfterUnitChange(selectedUnit) {
        mapFactory.setUnitBoundaries(selectedUnit.boundingBox, selectedUnit.boundingBoxCenter, selectedUnit.unitBoundaryGeoJSONData);
    }

}