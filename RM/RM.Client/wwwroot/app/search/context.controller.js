angular.module('search')
.controller('ContextController', ['$scope', '$state', '$stateParams', ContextController])
function ContextController($scope, $state, $stateParams) {
    var vm = this;
    vm.selectedItem = $stateParams.selectedItem;
    vm.showDeliveryPoint ="";
}
