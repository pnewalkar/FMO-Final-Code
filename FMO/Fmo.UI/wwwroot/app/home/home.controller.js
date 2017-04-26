angular
    .module('home')
    .controller("homeController", ['$scope', '$mdSidenav', homeController])
function homeController($scope, $mdSidenav) {
    var vm = this;
    var deliveryunit = vm.selectedDeliveryUnit;
    vm.deliveryUnit = deliveryUnit;
    var title = vm.contextTitle;
    vm.toggleSideNav = toggleSideNav;


    function toggleSideNav() {
        $mdSidenav('left').toggle();
        //vm.fetchActionItems();
    }

    function deliveryUnit() {
      
    }
};