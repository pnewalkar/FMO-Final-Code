angular.module('search')
.controller('ContextController', ['$scope', '$state', '$stateParams', ContextController])
function ContextController($scope, $state, $stateParams) {
    var vm = this;
 //   vm.showDeliveryPoint ="";
    debugger;
    vm.selectedItem = $stateParams.selectedItem;
    //if ($stateParams.val) {
    vm.showDeliveryPoint ="";
    //}
}
