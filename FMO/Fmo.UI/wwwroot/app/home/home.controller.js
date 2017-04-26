angular
    .module('home')
    .controller("homeController", ['$scope', homeController])
function homeController($scope) {
    var vm = this;
    var deliveryunit = vm.selectedDeliveryUnit;
    vm.deliveryUnit = deliveryUnit;
    var title = vm.contextTitle;
    function deliveryUnit() {
      
    }
};