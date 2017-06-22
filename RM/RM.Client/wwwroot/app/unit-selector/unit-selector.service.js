angular.module('unitSelector')
    .service('unitSelectorService', unitSelectorService);

unitSelectorService.$inject = ['$q',
                               'GlobalSettings',
                               '$filter',
                               'manageAccessService',
                               'mapFactory',
                               'unitSelectorAPIService','licensingInfoService'];
function unitSelectorService($q,
                             GlobalSettings,
                             $filter,
                             manageAccessService,
                             mapFactory,
                             unitSelectorAPIService, licensingInfoService) {
    var deliveryRouteUnit = [];
    var isDeliveryUnitDisabled = false;
    var result = [];
    var selectedDeliveryUnit = null;
    return {
        DeliveryUnit: DeliveryUnit,
        BindData: BindData
    };

    function DeliveryUnit(selectedDeliveryUnit) {      
        manageAccessService.activate(selectedDeliveryUnit.ID);
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
                        //deliveryRouteUnit = response;
                        angular.forEach(response, function (value, key) {
                            deliveryRouteUnit.push({ displayText: value.area + '    ' + value.unitName, ID: value.id, icon: "fa-map-marker delivery" ,area: value.area,
                            boundingBox: value.boundingBox,
                            boundingBoxCenter: value.boundingBoxCenter,
                            unitBoundaryGeoJSONData: value.unitBoundaryGeoJSONData })
                        });

                        var newTemp = $filter("filter")(deliveryRouteUnit, { ID: authData.unitGuid });
                        selectedUser = newTemp[0];
                        selectedDeliveryUnit = response[0];
                        result.push({ "deliveryRouteUnit": deliveryRouteUnit, "selectedUser": selectedUser, "selectedDeliveryUnit": selectedDeliveryUnit, "isDeliveryUnitDisabled": isDeliveryUnitDisabled });
                        updateMapAfterUnitChange(selectedUser);
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
                      //  deliveryRouteUnit = response;
                        angular.forEach(response, function (value, key) {
                            deliveryRouteUnit.push({ displayText: value.area + '    ' + value.unitName, ID: value.id, icon: "fa-map-marker delivery" ,area: value.area,
                            boundingBox: value.boundingBox,
                            boundingBoxCenter: value.boundingBoxCenter,
                            unitBoundaryGeoJSONData: value.unitBoundaryGeoJSONData })
                        });
                        selectedUser = deliveryRouteUnit[0];
                        selectedDeliveryUnit = response[0];
                        
                        result.push({ "deliveryRouteUnit": deliveryRouteUnit, "selectedUser": selectedUser, "selectedDeliveryUnit": selectedDeliveryUnit, "isDeliveryUnitDisabled": isDeliveryUnitDisabled});
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