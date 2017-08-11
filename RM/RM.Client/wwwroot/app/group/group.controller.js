angular
    .module('group')
    .controller("GroupController", GroupController);

GroupController.$inject = [
    'mapToolbarService',
    '$rootScope'
];

function GroupController(
    mapToolbarService,
    $rootScope
   ) {

    var vm = this;
    vm.addGroup = addGroup;

    function addGroup() {
        $rootScope.$emit('resetMapToolbar', { "isGroupAction": true });
    }
}

