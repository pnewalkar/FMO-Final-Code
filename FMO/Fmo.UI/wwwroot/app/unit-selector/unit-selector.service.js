angular.module('unitSelector')
    .service('unitSelectorService', unitSelectorService);

unitSelectorService.$inject = ['$q',
                               'GlobalSettings',
                               '$filter',
                               'manageAccessService',
                               'mapFactory',
                               'unitSelectorAPIService'];
function unitSelectorService($q,
                             GlobalSettings,
                             $filter,
                             manageAccessService,
                             mapFactory,
                             unitSelectorAPIService) {
    var deliveryRouteUnit = [];
    var isDeliveryUnitDisabled = false;
    var result = [];
    var selectedDeliveryUnit = null;
    return {
        DeliveryUnit: DeliveryUnit,
        BindData: BindData
    };

    function DeliveryUnit(selectedDeliveryUnit) {
        manageAccessService.activate(selectedDeliveryUnit.id);
        updateMapAfterUnitChange(selectedDeliveryUnit);
    }
    function BindData(deliveryRouteUnit) {
        if (deliveryRouteUnit.length === 0) {
            var authData = sessionStorage.getItem('authorizationData');
            authData = angular.fromJson(authData);
            if (authData.unitGuid) {
                var deferred = $q.defer();
                unitSelectorAPIService.getDeliveryUnit().then(function (response) {
                    if (response) {
                        deliveryRouteUnit = response;
                        var newTemp = $filter("filter")(deliveryRouteUnit, { id: authData.unitGuid });
                        selectedUser = newTemp[0];
                        selectedDeliveryUnit = selectedUser;
                        result.push({ "deliveryRouteUnit": deliveryRouteUnit, "selectedUser": selectedUser, "selectedDeliveryUnit": selectedDeliveryUnit, "isDeliveryUnitDisabled": isDeliveryUnitDisabled });
                        updateMapAfterUnitChange(selectedDeliveryUnit);
                        deferred.resolve(result);
                    }
                });
                return deferred.promise;

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
                        result.push({ "deliveryRouteUnit": deliveryRouteUnit, "selectedUser": selectedUser, "selectedDeliveryUnit": selectedDeliveryUnit, "isDeliveryUnitDisabled": isDeliveryUnitDisabled });
                        updateMapAfterUnitChange(selectedDeliveryUnit);
                        deferred.resolve(result);
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