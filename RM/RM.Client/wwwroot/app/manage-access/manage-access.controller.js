'use strict';
angular
    .module('manageAccess')
    .controller('ManageAccessController', ManageAccessController);

ManageAccessController.$inject = [
    'manageAccessService'
];

function ManageAccessController(
    manageAccessService) {

    var vm = this;
    vm.activate = activate;
    vm.activate();

    function activate(){
        manageAccessService.activate(null);
    }
}
