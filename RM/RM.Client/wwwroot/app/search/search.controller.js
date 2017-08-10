'use strict';
angular
    .module('search')
    .controller('SearchController',['searchBusinessService',
                                    '$timeout','CommonConstants',
                                     SearchController]);

function SearchController(
    searchBusinessService,
    $timeout,
    CommonConstants) {
    var vm = this;
    vm.resultSet = resultSet;
    vm.onEnterKeypress = onEnterKeypress;
    vm.OnChangeItem = OnChangeItem;
    vm.advanceSearch = advanceSearch;
    vm.onBlur = onBlur;
    vm.defaultResultCount = CommonConstants.DefaultResultCountForSearch;
    vm.noResultCount = CommonConstants.NoResultCountForSearch;


    function resultSet(query) {
        searchBusinessService.resultSet(query).then(function (response) {
            vm.resultscount = response[0].resultscount;
            vm.results = response[0].results;
            vm.isResultDisplay = response[0].isResultDisplay;
        });
    }

    function onEnterKeypress(searchText) {
        vm.isResultDisplay = true;
        var result = searchBusinessService.onEnterKeypress(searchText, vm.results);
        vm.results = result.results;
        vm.contextTitle = result.contextTitle;
    }

    function OnChangeItem(selectedItem) {
        searchBusinessService.OnChangeItem(selectedItem);
    }

    function onBlur() {
        $timeout(function () {
            vm.searchText = "";
            vm.isResultDisplay = false;
            vm.resultscount[0].count = 0;
        }, 1000);
    }

    function advanceSearch(query) {
        searchBusinessService.advanceSearch(query);
    }


}
