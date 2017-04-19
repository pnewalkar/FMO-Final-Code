'use strict';

angular
    .module('manageAccess')
    .controller('manageAccessController', manageAccessController);

manageAccessController.$inject = ['$stateParams', 'manageAccessBusinessService'];

function manageAccessController($stateParams,manageAccessBusinessService) {
    /* jshint validthis:true */
    var vm = this;
    vm.activate = activate;
    activate();
    function activate()
    {
        manageAccessBusinessService.activate(null);
    }
}
