'use strict';
angular.module('search')
.controller('SearchController', SearchController);

function SearchController(searchApiService, $scope, $state, mapFactory, mapStylesFactory, advanceSearchService, $mdDialog, $stateParams) {
    var vm = this;

    vm.resultSet = resultSet;
    vm.onEnterKeypress = onEnterKeypress;
    vm.OnChangeItem = OnChangeItem;
    vm.advanceSearch = advanceSearch;
    vm.openModalPopup = openModalPopup;
    vm.onBlur = onBlur;

    function querySearch(query) {
        searchApiService.basicSearch(query).then(function (response) {
            vm.resultscount = response.data.searchCounts;
            vm.results = response.data.searchResultItems;
            vm.isResultDisplay = true;
        });
    }

    function resultSet(query) {
        if (query.length >= 3) {
            querySearch(query);
        }
        else {
            vm.results = {};
            vm.resultscount = { 0: { count: 0 } };
        }
    }

    function onEnterKeypress(searchText) {
        vm.isResultDisplay = true;

        if (searchText === undefined) {
            vm.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
        }
        else {
            if (searchText.length >= 3) {
                if (vm.results.length === 1) {
                    OnChangeItem(vm.results[0]);
                }
            }
            else {
                vm.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
            }
        }
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === "DeliveryPoint") {
            searchApiService.GetDeliveryPointByUDPRN(selectedItem.udprn)
                .then(function (response) {
                    var data = response.data;
                    var lat = data.features[0].geometry.coordinates[1];
                    var long = data.features[0].geometry.coordinates[0];
                    mapFactory.setDeliveryPoint(long, lat);
                });
            $state.go('searchDetails', {
                selectedItem: selectedItem
            });
        }
        vm.isResultDisplay = false;
        vm.searchText = "";
    }

    function onBlur() {
        if(vm.results.length === 0)
        {
            vm.isResultDisplay = false;
            vm.searchText = "";
        }
        else if(vm.results[0].type === "Warning")
        {
            vm.isResultDisplay = false;
            vm.searchText = "";
        }
    }

    function advanceSearch(query) {
        $stateParams.data = query;
        var state = $stateParams;
        var advaceSearchTemplate = advanceSearchService.advanceSearch(query);
        vm.openModalPopup(advaceSearchTemplate);
    }

    function openModalPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting);
    };

    
}
