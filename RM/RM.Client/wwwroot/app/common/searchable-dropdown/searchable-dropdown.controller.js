angular
    .module('searchableDropdown')
    .controller("SearchableDropdownController", SearchableDropdownController);

SearchableDropdownController.$inject = [
    '$state',
    '$scope'
];

function SearchableDropdownController($state, $scope) {
    var vm = this;

    vm.clearSearchTerm = clearSearchTerm;
    vm.onChange = onChange;

    function clearSearchTerm() {
        vm.searchTerm = "";
    }

    function onChange() {
        vm.onSelectedItem()(vm.selectedItem);
    }
}