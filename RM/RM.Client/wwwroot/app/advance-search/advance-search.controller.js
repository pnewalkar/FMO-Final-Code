'use strict';
angular
    .module('advanceSearch')
    .controller('AdvanceSearchController', [
        '$mdDialog',
        'searchText',
        'advanceSearchService',
        'CommonConstants',
        
        AdvanceSearchController]);

function AdvanceSearchController(
    $mdDialog,
    searchText,
    advanceSearchService,
    CommonConstants
   ) {

    var vm = this;
    vm.initialize = initialize;
    vm.OnChangeItem = OnChangeItem;
    vm.toggleList = toggleList;
    vm.toggleSelection = toggleSelection;
    vm.queryAdvanceSearch = queryAdvanceSearch;
    vm.searchText = searchText;
    vm.closeWindow = closeWindow;
    vm.initialize()

    function initialize() {
        vm.queryAdvanceSearch(vm.searchText);
        vm.chkRoute = true;
        vm.chkPostCode = true;
        vm.chkDeliveryPoint = true;
        vm.chkStreetNetwork = true;
    }

    function closeWindow() {
        $mdDialog.cancel();
    }

    function queryAdvanceSearch(query) {

        advanceSearchService.queryAdvanceSearch(query).then(function (response) {
            vm.arrRoutes = response;
        });
    }

    function toggleSelection(selectedFlag) {
        vm.chkRoute = selectedFlag;
        vm.chkPostCode = selectedFlag;
        vm.chkDeliveryPoint = selectedFlag;
        vm.chkStreetNetwork = selectedFlag;
    }

    function toggleList(state) {
        vm.arrRoutes.forEach(function (e) {
            e.open = state;
        });
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === CommonConstants.EntityType.DeliveryPoint) {
            advanceSearchService.onChangeItem(selectedItem).then(function (response) {
                $mdDialog.hide();
            });
        }

    }
}