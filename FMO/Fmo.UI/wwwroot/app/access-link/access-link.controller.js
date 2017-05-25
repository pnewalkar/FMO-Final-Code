angular
    .module('accessLink')
    .controller("AccessLinkController",
    [
        'accessLinkAPIService',
        '$scope',
        '$mdDialog',
        '$state',
        'mapService',
        'mapFactory',
        'coordinatesService',
        'roadLinkGuidService',
        'accessLinkCoordinatesService',
        'intersectionPointService',
        'guidService',
        '$stateParams',
         AccessLinkController])
function AccessLinkController(
    accessLinkAPIService,
    $scope,
    $mdDialog,
    $state,
    mapService,
    mapFactory,
    coordinatesService,
    roadLinkGuidService,
    accessLinkCoordinatesService,
    intersectionPointService,
    guidService,
    $stateParams
) {
    vm = this;
    vm.createAccessLink = createAccessLink;
    vm.accessLink = accessLink;
    vm.initialize = initialize;
    vm.pathLength = null;
    vm.createAccessLink = createAccessLink;
    vm.enableSave = false;
    vm.enableBack = true;
    vm.clearAccessLink = clearAccessLink;
    vm.accessLinkFeature = $stateParams.accessLinkFeature;
    initialize();
    function initialize() {
        accessLink();
    }


    function createAccessLink() {
        var accessLink = null;

        var accessLinkDTO = {
            "OperationalObjectPoint": JSON.stringify(coordinatesService.getCordinates()),
            "AccessLinkLine": JSON.stringify(accessLinkCoordinatesService.getCordinates()),
            "NetworkLinkGUID": roadLinkGuidService.getRoadLinkGuid(),
            "OperationalObjectGUID": guidService.getGuid(),
            "NetworkIntersectionPoint": JSON.stringify(intersectionPointService.getIntersectionPoint()),
            "Workloadlength": vm.pathLength
        }

        accessLinkAPIService.CreateAccessLink(accessLinkDTO).then(function (response) {
            if (response === true) {
                mapFactory.setAccessLink();
                vm.enableBack = false;
                vm.enableSave = false;
                vm.pathLength = '';
                mapService.refreshLayers();
            }
        });
    }

    function accessLink() {

        var map = mapFactory.getMap();
        var boundingBoxCoordinates = map.getView().calculateExtent(map.getSize());
        var accessLinkCoordinates = JSON.stringify(accessLinkCoordinatesService.getCordinates());
        var isAccessLinkValid = false;
        var accessLinkManualCreateModelDTO = {
            "BoundingBoxCoordinates" : JSON.stringify(boundingBoxCoordinates),
            "OperationalObjectPoint": JSON.stringify(coordinatesService.getCordinates()),
            "AccessLinkLine": JSON.stringify(accessLinkCoordinatesService.getCordinates()),
            "NetworkLinkGUID": roadLinkGuidService.getRoadLinkGuid(),
            "OperationalObjectGUID": guidService.getGuid(),
            "NetworkIntersectionPoint": JSON.stringify(intersectionPointService.getIntersectionPoint())
        };


        accessLinkAPIService.CheckAccessLinkIsValid(accessLinkManualCreateModelDTO).then(function (response) {
            if (response === true) {
                accessLinkAPIService.GetAdjPathLength(accessLinkManualCreateModelDTO).then(function (response) {
                    vm.pathLength = response;
                    vm.enableSave = true;
                    vm.enableBack = true;
                });
            }
            else {
                accessLinkCoordinatesService.setCordinates(null);
                roadLinkGuidService.setRoadLinkGuid(null);
                intersectionPointService.setIntersectionPoint(null);
                mapService.deleteAccessLinkFeature(vm.accessLinkFeature);
                vm.enableSave = false;
                vm.pathLength = '';
            }
        });
    };

    function clearAccessLink() {
        accessLinkCoordinatesService.setCordinates(null);
        roadLinkGuidService.setRoadLinkGuid(null);
        intersectionPointService.setIntersectionPoint(null);
        mapService.deleteAccessLinkFeature(vm.accessLinkFeature);
        vm.enableSave = false;
        vm.pathLength = '';
    }

    function calculatepathLength() {
        //api call

        //accessLink
    }
};