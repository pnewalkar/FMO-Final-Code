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
    vm.addedPoints = [{ displaytext: "1 Mars Crescent BN1 1HS" }];
    vm.availablePoints = [{displaytext: "1 Mars Crescent BN1 1HS" }, { displaytext: "13 Mars Crescent BN1 0ER" }, { displaytext: "31 Mars Crescent BN1 3HE" }, { displaytext: "51 Mars Crescent BN1 3HS" }]
    vm.initialize = initialize;
    vm.onSingleAccept = onSingleAccept;
    vm.createGroup = createGroup;
    vm.isReadOnly = false;
    vm.getSummarizedAddresses = getSummarizedAddresses;
    vm.toggle = false;

    // vm.ServicePointType = "Inside";

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

    function getSummarizedAddresses() {
        vm.toggle = !vm.toggle;
        var addedPoints = vm.addedPoints;
        vm.distinctPostcodes = {};
        for (var index = 0; index < addedPoints.length; index++) {
            var postcode = addedPoints[index].postcode;
            if (!(postcode in vm.distinctPostcodes)) {
                vm.distinctPostcodes[postcode] = 1;
            }
            else if (postcode in vm.distinctPostcodes) {
                vm.distinctPostcodes[postcode] += 1;
            }
        }
        vm.summarizedCount = [];
        for (var key in vm.distinctPostcodes) {
            if (vm.distinctPostcodes.hasOwnProperty(key)) {
                vm.summarizedCount.push({
                    postcode: key,
                    deliveyPoints: vm.distinctPostcodes[key]
                });
            }
        }
    }
}

