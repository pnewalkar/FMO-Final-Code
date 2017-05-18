'use strict';
angular
    .module('advanceSearch')
    .controller('AdvanceSearchController', [
        '$state',
        '$mdDialog',
        '$stateParams',
        'advanceSearchBusinessService',
        
        AdvanceSearchController]);

function AdvanceSearchController(
    $state,
    $mdDialog,
    $stateParams,
    advanceSearchBusinessService
   ) {

    var vm = this;
    vm.initialize = initialize;
    vm.OnChangeItem = OnChangeItem;
    vm.toggleList = toggleList;
    vm.toggleSelection = toggleSelection;
    vm.queryAdvanceSearch = queryAdvanceSearch;
    vm.searchText = $stateParams.data;
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
        advanceSearchBusinessService.queryAdvanceSearch(query).then(function (response) {
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
        if (selectedItem.type === "DeliveryPoint") {
            advanceSearchBusinessService.onChangeItem(selectedItem).then(function (response) {
                $state.go('searchDetails', { selectedItem: selectedItem });
                $mdDialog.hide();
            });
        }
    }
}