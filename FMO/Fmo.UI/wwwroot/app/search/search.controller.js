'use strict';
angular.module('search')
   .controller('SearchController', SearchController);

function SearchController(searchApiService, $scope, $state, mapFactory, mapStylesFactory, advanceSearchService, $mdDialog, $stateParams) {
    var self = this;

    self.resultSet = resultSet;
    self.presEnter = presEnter;
    self.OnChangeItem = OnChangeItem;
    self.advanceSearch = advanceSearch;
    self.openModalPopup = openModalPopup;

    function querySearch(query) {
        searchApiService.basicSearch(query).then(function (response) {
            self.resultscount = response.data.searchCounts;
            self.results = response.data.searchResultItems
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
            //mapFactory.getShapeAsync('http://localhost:34583/api/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + selectedItem.udprn)
            //    .then(function (response) {
            //        var data = response.data;
            //});
            $state.go('searchDetails', { selectedItem: selectedItem });
        }
    }

    function advanceSearch(query) {
        debugger;
        $stateParams.data = query;
        var state = $stateParams;
        var advaceSearchTemplate = advanceSearchService.advanceSearch(query);
        self.openModalPopup(advaceSearchTemplate);
      //  vm.openModalPopup("Test");
    }

    function openModalPopup(modalSetting) {
        //var state = $stateParams;
        //var setting = routeLogService.routeLog(selectedUnit);
        //debugger;
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting);
      
    };

    
}
