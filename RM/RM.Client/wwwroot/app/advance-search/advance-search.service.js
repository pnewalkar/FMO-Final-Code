
angular.module('advanceSearch')
        .service('advanceSearchService', advanceSearchService);
advanceSearchService.$inject = ['advanceSearchAPIService',
                                '$q',                              
                                'searchService',
                                'mapFactory',
                                'CommonConstants',
                                '$state',
                                 'roleAccessService'];

function advanceSearchService(advanceSearchAPIService,
                              $q,                            
                              searchService,
                              mapFactory,
                              CommonConstants,
                              $state,
                              roleAccessService) {
    
    
    var deliveryPointObj = null;
    var postCodeObj = null;
    var streetnameObj = null;
    var deliveryRouteobj = null;
    var route = [];
    var obj = null;
    var arrDeliverypoints = [];
    var arrPostCodes = [];
    var arrStreetNames = [];
    var arrDeliveryRoutes = [];
    var arrRoutes = [];

    return {
        queryAdvanceSearch: queryAdvanceSearch,
        onChangeItem: onChangeItem,
        showDeliveryPointDetails: showDeliveryPointDetails
    };

    function queryAdvanceSearch(query) {
        var deferred = $q.defer();
        var advanceSearchResults = null;
      
        advanceSearchAPIService.advanceSearch(query).then(function (response) {
            advanceSearchResults = response;
            angular.forEach(advanceSearchResults.searchResultItems, function (value, key) {
                if (value.type === CommonConstants.EntityType.DeliveryPoint)
                {
                    obj = { 'displayText': value.displayText, 'UDPRN': value.udprn, 'type': CommonConstants.EntityType.DeliveryPoint, 'ID': value.deliveryPointGuid }
                    arrDeliverypoints.push(obj);
                }
                else if (value.type === CommonConstants.EntityType.Postcode) {
                   obj = { 'displayText': value.displayText }
                    arrPostCodes.push(obj);
                }
                else if (value.type === CommonConstants.EntityType.StreetNetwork) {
                   obj = { 'displayText': value.displayText }
                   arrStreetNames.push(obj);
                }
              else  if (value.type === CommonConstants.EntityType.Route) {
                    obj = { 'displayText': value.displayText }
                    arrDeliveryRoutes.push(obj);
                }
            });

            if (arrDeliverypoints.length === 1) {
                deliveryPointObj = { 'type': CommonConstants.EntityType.DeliveryPoint, 'name': arrDeliverypoints, 'open': true };
            }
            else {
                deliveryPointObj = { 'type': CommonConstants.EntityType.DeliveryPoint, 'name': arrDeliverypoints, 'open': false };
            }

            if (arrPostCodes.length === 1) {
                postCodeObj = {
                    'type': CommonConstants.EntityType.Postcode, 'name': arrPostCodes, 'open': true
                };
            }
            else {
                postCodeObj = {
                    'type': CommonConstants.EntityType.Postcode, 'name': arrPostCodes, 'open': false
                };
            }
            if (arrStreetNames.length === 1) {
                streetnameObj = { 'type': CommonConstants.EntityType.StreetNetwork, 'name': arrStreetNames, 'open': true };
            }
            else {
                streetnameObj = { 'type': CommonConstants.EntityType.StreetNetwork, 'name': arrStreetNames, 'open': false };
            }
            if (arrDeliveryRoutes.length === 1) {
                deliveryRouteobj = { 'type': CommonConstants.EntityType.Route, 'name': arrDeliveryRoutes, 'open': true };
            }
            else {
                deliveryRouteobj = { 'type': CommonConstants.EntityType.Route, 'name': arrDeliveryRoutes, 'open': false };
            }

            groupSearchEntities();
            deferred.resolve(arrRoutes);
        });
        return deferred.promise;
    }

    function onChangeItem(selectedItem) {
        var deferred = $q.defer();
        var coordinatesData = null;
   
        searchService.GetDeliveryPointByGuid(selectedItem.ID)
                .then(function (response) {
                    coordinatesData = response;
                    lat = coordinatesData.features[0].geometry.coordinates[1];
                    long = coordinatesData.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                    var deliveryPointDetails = coordinatesData.features[0].properties;
                    showDeliveryPointDetails(deliveryPointDetails);
                    deferred.resolve(coordinatesData);
                });
            return deferred.promise;
    }

    function showDeliveryPointDetails(deliveryPointDetails) {
        deliveryPointDetails.routeName = null;
        mapFactory.GetRouteForDeliveryPoint(deliveryPointDetails.deliveryPointId)
              .then(function (response) {
                  if (response != null) {
                      if (response[0].key == CommonConstants.RouteName) {
                          deliveryPointDetails.routeName = [response[0].value];
                          if (response[1].key == CommonConstants.DpUse) {
                              deliveryPointDetails.dpUse = response[1].value;
                          }
                      }
                      else if (response[0].key == CommonConstants.DpUse) {
                          deliveryPointDetails.dpUse = response[0].value;
                      }
                  }
                  $state.go('deliveryPointDetails', {
                      selectedDeliveryPoint: deliveryPointDetails
                  }, { reload: true });
              });
    }

    function groupSearchEntities() {
        var userRoleDetails = roleAccessService.fetchActionItems().RolesActionResult;
        if (userRoleDetails && userRoleDetails.length > 0) {
            switch (userRoleDetails[0].RoleName) {
                case CommonConstants.UserType.DeliveryUser:
                    {
                        arrRoutes = [];
                        if (arrPostCodes.length > 0) {
                            arrRoutes.push(postCodeObj);
                        }
                        if (arrStreetNames.length > 0) {
                            arrRoutes.push(streetnameObj);
                        }
                        if (arrDeliverypoints.length > 0) {
                            arrRoutes.push(deliveryPointObj);
                        }
                        if (arrDeliveryRoutes.length > 0) {
                            arrRoutes.push(deliveryRouteobj);
                        }
                        break;
                    }
                case CommonConstants.UserType.CollectionUser:
                    {
                        arrRoutes = [];
                        if (arrPostCodes.length > 0) {
                            arrRoutes.push(postCodeObj);
                        }
                        if (arrDeliveryRoutes.length > 0) {
                            arrRoutes.push(deliveryRouteobj);
                        }
                        if (arrStreetNames.length > 0) {
                            arrRoutes.push(streetnameObj);
                        }
                        if (arrDeliverypoints.length > 0) {
                            arrRoutes.push(deliveryPointObj);
                        }
                        break;
                    }
                case CommonConstants.UserType.ManagerUser:
                    {
                        arrRoutes = [];
                        if (arrPostCodes.length > 0) {
                            arrRoutes.push(postCodeObj);
                        }

                        if (arrDeliverypoints.length > 0) {
                            arrRoutes.push(deliveryPointObj);
                        }
                        if (arrStreetNames.length > 0) {
                            arrRoutes.push(streetnameObj);
                        }
                        if (arrDeliveryRoutes.length > 0) {
                            arrRoutes.push(deliveryRouteobj);
                        }
                        break;

                    }
                default:
                    break;

            }
        }
    }
   
}