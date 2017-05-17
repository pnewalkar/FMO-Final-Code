'use strict';
angular
    .module('manageAccess')
    .controller('ManageAccessController', ManageAccessController);

ManageAccessController.$inject = [
    'manageAccessBusinessService'
];

function ManageAccessController(manageAccessBusinessService) {
    var vm = this;
    vm.activate = activate;
    vm.activate();

    function activate(){
        manageAccessBusinessService.activate(null);
    }
}
