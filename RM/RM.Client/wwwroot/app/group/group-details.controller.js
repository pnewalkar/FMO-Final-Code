angular
    .module('group')
    .controller("GroupDetailsController", GroupDetailsController);

GroupDetailsController.$inject = [
    'groupService'
];

function GroupDetailsController(
    groupService
   ) {

    var vm = this;
    vm.initialize = initialize;

    vm.initialize();

    function initialize() {
        groupService.initialize().then(function (response) {
            vm.deliveryGroupTypes = response.DeliveryGroupType;
            vm.servicePointTypes = response.ServicePointType;
        });
    }
}

