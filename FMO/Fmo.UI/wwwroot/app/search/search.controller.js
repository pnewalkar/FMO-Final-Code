'use strict';
angular.module('search')
   .controller('SearchController', SearchController);

function SearchController(searchApiService, $scope, $state, mapFactory, mapStylesFactory) {
    var vm = this;

    vm.resultSet = resultSet;
    vm.onEnterKeypress = onEnterKeypress;
    vm.OnChangeItem = OnChangeItem;

    function querySearch(query) {
        searchApiService.basicSearch(query).then(function (response) {
            vm.resultscount = response.data.searchCounts;
            vm.results = response.data.searchResultItems;
        });
    }

    function resultSet(query) {
        if (query.length >= 3) {
            querySearch(query);
        }
        else {
            vm.results = {};
        }
    }

    function onEnterKeypress(searchText) {
        if (searchText.length > 3) {
            if (vm.results.length === 1) {
                OnChangeItem(vm.results);
            }
        }
        else {
            vm.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
        }
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === "DeliveryPoint") {
            //mapFactory.getShapeAsync('http://localhost:34583/api/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + selectedItem.udprn)
            //    .then(function (response) {
            //        var data = response.data;
            //});
            $state.go('searchDetails', { selectedItem: selectedItem });
        }
    }
}
