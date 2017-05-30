angular
     .module('reusableComponent')
     .controller("CustomSearchableDropdownController", CustomSearchableDropdownController);

CustomSearchableDropdownController.$inject = [
    '$state',
    '$scope'
];

function CustomSearchableDropdownController($state, $scope) {
    var vm = this;
    vm.clearSearchTerm = clearSearchTerm;
    vm.onChange = onChange;
    function clearSearchTerm() {
        vm.searchTerm = "";
    }
    function onChange(item) {
        //vm.stringTest = "jitendra";
        //vm.selectionChanged(vm.stringTest);
        $scope.$emit('selectionChanged', {  selectedRoute: item });
    }
}