'use strict';
angular.module('search')
   .controller('SearchController', SearchController);

function SearchController(searchApiService, $scope, $state) {
    var self = this;

    self.resultSet = resultSet;
    self.presEnter = presEnter;
    self.OnChangeItem = OnChangeItem;

    function querySearch(query) {
        searchApiService.basicSearch(query).then(function (response) {
            self.resultscount = response.data.searchCounts;
            self.results = response.data.searchResultItems
            $state.go('searchDetails', { selectedItem: self.results });
        });
    }

    function resultSet(query) {
        if (query.length >= 3) {
            querySearch(query);
        }
        else {
            self.results = {};
        }
    }

    function presEnter(searchText) {
        if (searchText.length > 3) {
            if (self.results.length === 1) {
                OnChangeItem(self.results);
            }
        }
        else {
            self.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
        }
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === "DeliveryPoint") {
            $state.go('searchDetails', { selectedItem: selectedItem });
        }
    }
}
