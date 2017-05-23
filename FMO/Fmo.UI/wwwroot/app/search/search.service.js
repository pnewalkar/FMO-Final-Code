
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
                                  'CommonConstants'];

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
    CommonConstants) {
    var result = [];
    return {
        resultSet: resultSet,
        onEnterKeypress: onEnterKeypress,
        OnChangeItem: OnChangeItem,
        advanceSearch: advanceSearch
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
            searchService.GetDeliveryPointByUDPRN(selectedItem.udprn)
                .then(function (response) {
                    var data = response;
                    var lat = data.features[0].geometry.coordinates[1];
                    var long = data.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                });
            contextTitle = CommonConstants.TitleContextPanel;
            $state.go('searchDetails', {
                selectedItem: selectedItem
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


}

