angular
    .module('group')
    .controller("GroupDetailsController", GroupDetailsController);

GroupDetailsController.$inject = [
    'groupService',
    'mapService'
];

function GroupDetailsController(
    groupService,
    mapService
   ) {

    var vm = this;
    vm.addedPoints = [{ displaytext: "1 Mars Crescent BN1 1HS" }];
    vm.availablePoints = [{displaytext: "1 Mars Crescent BN1 1HS"}, {displaytext: "13 Mars Crescent BN1 0ER"}, {displaytext: "31 Mars Crescent BN1 3HE"},{displaytext: "51 Mars Crescent BN1 3HS"}]
    vm.initialize = initialize;
    vm.onSingleAccept = onSingleAccept;
    vm.createGroup = createGroup;
    vm.isReadOnly = false;
    mapService.onDrawEnd = onDrawEnd;
    vm.ServicePointType.value = "Inside";

    vm.initialize();

    function initialize() {
        groupService.initialize().then(function (response) {
            vm.deliveryGroupTypes = response.DeliveryGroupType;
            vm.servicePointTypes = response.ServicePointType;
        });
    }

    function onSingleAccept(deliveyPoint) {
        vm.addedPoints.push(deliveyPoint);
    }

    function createGroup() {
        vm.isReadOnly = true;
    }

    function onDrawEnd(buttonName, feature) {
        vm.addedPoints = feature;
        //if (vm.adding && buttonName == "group") {
        //    vm.setupToolbar();
        //    vm.editGroup();
        //    vm.selectedFeature = feature;
        //}
    }
}

