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
    vm.addedPoints = [];
    vm.availablePoints = [{displaytext: "1 Mars Crescent BN1 1HS"}, {displaytext: "13 Mars Crescent BN1 0ER"}, {displaytext: "31 Mars Crescent BN1 3HE"},{displaytext: "51 Mars Crescent BN1 3HS"}]
    vm.initialize = initialize;
    vm.onSingleAccept = onSingleAccept;

    //vm.initialize();

    function initialize() {
        groupService.initialize().then(function (response) {
            vm.deliveryGroupTypes = response.DeliveryGroupType;
            vm.servicePointTypes = response.ServicePointType;
        });
    }

    function onSingleAccept(deliveyPoint) {
        vm.availablePoints
        vm.addedPoints.push(deliveyPoint);
    }
}

