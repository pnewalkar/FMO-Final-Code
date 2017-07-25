
angular
    .module('search')
    .service('searchBusinessService', searchBusinessService);
searchBusinessService.$inject = ['searchService',
                                  '$state',
                                  'mapFactory',
                                  'mapStylesFactory',
                                  'popUpSettingService',
                                  '$mdDialog',
                                  '$stateParams',
                                  '$timeout',
                                  '$q',
                                  'CommonConstants',
                                  'mapService',
                                  'searchDPSelectedService'];

function searchBusinessService(
    searchService,
    $state,
    mapFactory,
    mapStylesFactory,
    popUpSettingService,
    $mdDialog,
    $stateParams,
    $timeout,
    $q,
    CommonConstants,
    mapService,
    searchDPSelectedService) {
    var result = [];
    return {
        resultSet: resultSet,
        onEnterKeypress: onEnterKeypress,
        OnChangeItem: OnChangeItem,
        advanceSearch: advanceSearch,
        showDeliveryPointDetails: showDeliveryPointDetails
    };

    function resultSet(query) {
        var deferred = $q.defer();
        var results;
        var resultscount;
        var isResultDisplay;
        result = [];
        if (query.length >= 3) {
            searchService.basicSearch(query).then(function (response) {
                result.push({ "resultscount": response.searchCounts, "results": response.searchResultItems, "isResultDisplay": true })
                deferred.resolve(result);
            });
        }
        else {
            results = {};
            resultscount = { 0: { count: 0 } };
            isResultDisplay = false;
            result.push({ "resultscount": resultscount, "results": results, "isResultDisplay": false })
            deferred.resolve(result);
        }
        return deferred.promise;
    }

    function onEnterKeypress(searchText, results) {
        var contextTitle;
        if (angular.isUndefined(searchText)) {
            results = [{ displayText: CommonConstants.SearchLessThanThreeCharactersErrorMessage, type: CommonConstants.SearchErrorType }];
        }
        else {
            if (searchText.length >= 3) {
                if (results.length === 1) {
                    contextTitle = OnChangeItem(results[0]);
                }
                if (results.length > 1) {
                    advanceSearch(searchText);
                }
            }
            else {
                results = [{ displayText: CommonConstants.SearchLessThanThreeCharactersErrorMessage, type: CommonConstants.SearchErrorType }];
            }
        }
        return {
            results: results,
            contextTitle: contextTitle
        };
    }

    function OnChangeItem(selectedItem) {
        var contextTitle;
        if (selectedItem.type === CommonConstants.EntityType.DeliveryPoint) {
            searchService.GetDeliveryPointByGuid(selectedItem.deliveryPointGuid)
                .then(function (response) {
                    var data = response;
                    var lat = data.features[0].geometry.coordinates[1];
                    var long = data.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                    var deliveryPointDetails = data.features[0].properties;
                    showDeliveryPointDetails(deliveryPointDetails);
                    searchDPSelectedService.setSelectedDP(true);
                    mapService.deselectDP();
                });
        }
        return contextTitle;
    }

    function advanceSearch(query) {
        var advaceSearchTemplate = popUpSettingService.advanceSearch(query);
        openModalPopup(advaceSearchTemplate);
    }

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting).then(function (returnedData) {
        });


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


}

