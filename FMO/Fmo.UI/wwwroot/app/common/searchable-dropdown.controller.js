angular
     .module('reusableComponent')
     .controller("CustomSearchableDropdownController", CustomSearchableDropdownController);

CustomSearchableDropdownController.$inject = [
    '$state'
];

function CustomSearchableDropdownController($state) {
    var vm = this;
    vm.clearSearchTerm = clearSearchTerm;
    vm.onChange = onChange;
    function clearSearchTerm() {
        vm.searchTerm = "";
    }

    function onChange() {
        vm.selectionChanged();
    }
}