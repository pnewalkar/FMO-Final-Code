angular
    .module('group')
    .controller("GroupController", GroupController);

GroupController.$inject = [
    'mapToolbarService',
    '$rootScope',
    '$state'
];

function GroupController(
    mapToolbarService,
    $rootScope,
    $state
   ) {

    var vm = this;
    vm.addGroup = addGroup;

    function addGroup() {
        $rootScope.$emit('resetMapToolbar', { "isGroupAction": true });
        $state.go("deliveryPointGroupDetails");
    }
}

