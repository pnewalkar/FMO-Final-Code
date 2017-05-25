'use strict';
angular.module('search')
.controller('SearchController', SearchController);

SearchController.$inject = [
   'searchApiService',
    '$state',
    'mapFactory',
    'popUpSettingService',
    '$mdDialog',
    '$timeout',
    'errorService'
  
];

function SearchController(
    searchApiService,
    $state,
    mapFactory,
    popUpSettingService,
    $mdDialog,
    $timeout,
    errorService
    ) {

    var vm = this;
    vm.resultSet = resultSet;
    vm.onEnterKeypress = onEnterKeypress;
    vm.OnChangeItem = OnChangeItem;
    vm.advanceSearch = advanceSearch;
    vm.openModalPopup = openModalPopup;
    vm.onBlur = onBlur;
    vm.openAlert = openAlert;

    function querySearch(query) {
        searchApiService.basicSearch(query).then(function (response) {
            vm.resultscount = response.data.searchCounts;
            vm.results = response.data.searchResultItems;
            vm.isResultDisplay = true;
        }).catch(function (e) {
            errorService.openAlert(e.error);
            //openAlert(e.error);
            throw e;
        })
       
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

        if (angular.isUndefined(searchText)) {
            vm.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
        }
        else {
            if (searchText.length >= 3) {
                if (vm.results.length === 1) {
                    OnChangeItem(vm.results[0]);
                }
                if (vm.results.length > 1) {
                    advanceSearch(vm.searchText);
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
            vm.contextTitle = "Context Panel";
            $state.go('searchDetails', {
                selectedItem: selectedItem
            });
        }
        vm.isResultDisplay = false;
        vm.searchText = "";
    }

    function onBlur() {
        $timeout(function () {
            vm.searchText = "";
            vm.isResultDisplay = false;
        }, 1000);
    }

    function advanceSearch(query) {
        var advaceSearchTemplate = popUpSettingService.advanceSearch(query);
        vm.openModalPopup(advaceSearchTemplate);
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
    };

    function openAlert(text) {
        debugger;
        var confirm =
          $mdDialog.confirm()
            .clickOutsideToClose(true)
            .title('Error')
            .textContent(text)
            .ariaLabel('Left to right demo')
            .ok('Ok')
            

        $mdDialog.show(confirm).then(function () {
          
         
        }, function () {
        });
    };
}
