angular
     .module('reusableComponent')
     .controller("CustomSearchableDropdownController", CustomSearchableDropdownController);
function CustomSearchableDropdownController() {
    var vm = this;
    vm.clearSearchTerm = clearSearchTerm;
    vm.selectionChange = selectionChange;
    function clearSearchTerm() {
        debugger;
        vm.searchTerm = "";
    }

    function selectionChange() {
        debugger;
        var item = vm.selectedRoute;
        vm.selectedRoute = item;
       
      //  vm.selectionChanged();
        vm.selectionChanged({ selectedRoute: vm.selectedRoute });
    }
 
}