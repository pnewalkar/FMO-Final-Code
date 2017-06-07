//angular
//     .module('reusableComponent')
//     .controller("CustomSearchableDropdownController", CustomSearchableDropdownController);

//CustomSearchableDropdownController.$inject = [
//    '$state',
//    '$scope'
//];

//function CustomSearchableDropdownController($state, $scope) {
//    var vm = this;
//    vm.clearSearchTerm = clearSearchTerm;
//    vm.onChange = onChange;
//    function clearSearchTerm() {
//        vm.searchTerm = "";
//    }
//    function onChange() {
//        //vm.stringTest = "jitendra";
//        //vm.selectionChanged()(vm.selectedRoute);
//        $scope.$emit('selectionChanged', { selectedRoute: vm.selectedRoute });
//    }
//}

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

    function onChange() {
        vm.onSelectedItem()(vm.selectedItem);
    }
}