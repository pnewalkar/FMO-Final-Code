
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
                                  '$q'];

function searchBusinessService(
    searchService,
    $state,
    mapFactory,
    mapStylesFactory,
    popUpSettingService,
    $mdDialog,
    $stateParams,
    $timeout,
    $q) {
    var vm = this;
    var result = [];
    return {
        resultSet: resultSet,
        onEnterKeypress: onEnterKeypress,
        OnChangeItem: OnChangeItem,
        advanceSearch: advanceSearch
    };

    function resultSet(query) {
        var deferred = $q.defer();
        result = [];
        if (query.length >= 3) {
            searchService.basicSearch(query).then(function (response) {
                result.push({ "resultscount": response.data.searchCounts, "results": response.data.searchResultItems, "isResultDisplay": true })
                deferred.resolve(result);
            });
        }
        else {
            vm.results = {};
            vm.resultscount = { 0: { count: 0 } };
            vm.isResultDisplay = false;
            result.push({ "resultscount": vm.resultscount, "results": vm.results, "isResultDisplay": false })
            deferred.resolve(result);
        }
        return deferred.promise;
    }

    function onEnterKeypress(searchText) {
        if (angular.isUndefined(searchText)) {
            vm.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
        }
        else {
            if (searchText.length >= 3) {
                if (vm.results.length === 1) {
                    vm.contextTitle = OnChangeItem(vm.results[0]);
                }
                if (vm.results.length > 1) {
                    advanceSearch(vm.searchText);
                }
            }
            else {
                vm.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
            }
        }
        return {
            results: vm.results,
            contextTitle: vm.contextTitle
        };
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === "DeliveryPoint") {
            searchService.GetDeliveryPointByUDPRN(selectedItem.udprn)
                .then(function (response) {
                    var data = response.data;
                    var lat = data.features[0].geometry.coordinates[1];
                    var long = data.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                });
            vm.contextTitle = "Context Panel";
            $state.go('searchDetails', {
                selectedItem: selectedItem
            });
        }
        return vm.contextTitle;

    }


    function advanceSearch(query) {
        var advaceSearchTemplate = popUpSettingService.advanceSearch(query);
        openModalPopup(advaceSearchTemplate);
    }

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting).then(function (returnedData) {
            vm.data = returnedData;
            vm.contextTitle = "Context Panel";
            vm.isResultDisplay = false;
            vm.resultscount[0].count = 0;
            vm.searchText = "";
        });


    }


}

