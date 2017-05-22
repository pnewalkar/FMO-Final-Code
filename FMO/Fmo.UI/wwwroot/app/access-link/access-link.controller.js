angular
    .module('accessLink')
    .controller("AccessLinkController",
    [
        'accessLinkAPIService',
        '$scope',
        '$mdDialog',
        '$state',
        'deliveryPointService',
        'roadLinkGuidService',
        'accessLinkCoordinatesService',
        'intersectionPointService',
        'guidService',
         AccessLinkController])
function AccessLinkController(
    accessLinkAPIService,
    $scope,
    $mdDialog,
    $state,
    deliveryPointService,
    roadLinkGuidService,
    accessLinkCoordinatesService,
    intersectionPointService,
    guidService
) {
    vm = this;
    vm.createAccessLink = createAccessLink;
    vm.accessLink = accessLink;
    vm.initialize = initialize();

    function initialize() {
        accessLink();
    }

    function createAccessLink() {
        debugger;
        var accessLink = null;

        accessLinkAPIService.createAccessLink(accessLink).then(function (response) {
            debugger;
            alert.message(response);            
        });
    }

    function accessLink() {
        var accessLinkManualCreateModelDTO = {
            "OperationalObjectPoint": JSON.stringify(deliveryPointService.getCordinates()),
            "AccessLinkLine": JSON.stringify(accessLinkCoordinatesService.getCordinates()),
            "NetworkLinkGUID": roadLinkGuidService.getRoadLinkGuid(),
            "OperationalObjectGUID": guidService.getGuid(),
            "NetworkIntersectionPoint": JSON.stringify(intersectionPointService.getIntersectionPoint())
        };
        accessLinkAPIService.GetAdjPathLength(accessLinkManualCreateModelDTO).then(function (response) {

        })
    };

    function calculatepathLength() {
        //api call

        accessLink
    }
};