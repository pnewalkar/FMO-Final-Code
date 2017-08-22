angular
    .module('group')
    .controller("GroupController", GroupController);

GroupController.$inject = [
    'mapToolbarService',
    '$rootScope',
    '$state',
    'groupService'
];

function GroupController(
    mapToolbarService,
    $rootScope,
    $state,
    groupService
   ) {

    var vm = this;
    vm.addGroup = addGroup;
    vm.checkDeliveryGroupType = checkDeliveryGroupType;

    function addGroup() {
        $rootScope.$emit('resetMapToolbar', { "isGroupAction": true });
        $state.go("deliveryPointGroupDetails");
        fetchDeliveryGroupType();
    }

    function fetchDeliveryGroupType() {
        groupService.DeliveryGroupTypes()
            .then(function (response) {
                vm.DeliveryGroupTypes = response;
            });
    }

    function checkDeliveryGroupType() {
        if (vm.groupType === "Complex")
        {
            vm.Floors = "Floor1";
            vm.internalDistance = "InternalDist";
        }
        
    }
}

