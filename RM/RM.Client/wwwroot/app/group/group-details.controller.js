angular
    .module('group')
    .controller("GroupDetailsController", GroupDetailsController);

GroupDetailsController.$inject = [
    'groupService',
    'mapService',
    '$timeout',
    '$rootScope',
    'GlobalSettings',
    'groupAPIService'
];

function GroupDetailsController(
    groupService,
    mapService,
    $timeout,
    $rootScope,
    GlobalSettings,
    groupAPIService
   ) {

    var vm = this;
    vm.addedPoints = [];
    vm.availablePoints = []
    vm.initialize = initialize;
    vm.onSingleAccept = onSingleAccept;
    vm.onSingleReject = onSingleReject;
    vm.onAcceptAll = onAcceptAll;
    vm.onRejectAll = onRejectAll;
    vm.createGroup = createGroup;
    vm.isReadOnly = false;
    vm.getPostcode = getPostcode;
    //vm.ServicePointType.value = "Inside";

    //vm.initialize();
    mapService.addSelectionListener(onFeatureSelect);

    $rootScope.$on('setAssociatedDp', function (event, associatedDP) {
        vm.polygonCoordinates = associatedDP.listOfAssociatedDP.getGeometry().flatCoordinates;
        editGroup();
    });

    function editGroup() {
        vm.editingFeature = mapService.getActiveFeature();
        var layer = mapService.getLayer(GlobalSettings.deliveryPointLayerName).layer;
        var groupsLayer = mapService.getLayer(GlobalSettings.groupLayerName).layer;
        vm.initialCoordinates = vm.editingFeature.getGeometry().getCoordinates();
        vm.initialPoints = [];
        if (vm.adding)
            vm.availablePoints = [];
        else
            vm.availablePoints = mapService.getFeaturesWithinFeature(layer.getSource(), vm.editingFeature);
        vm.ignorePoints = [];
        //showModify();
        vm.editing = true;
        //$scope.editGroupsForm.$setPristine();
       // onModify();
    }

    function onModify() {
        var feature = mapService.getActiveFeature();
        var layer = mapService.getLayer(GlobalSettings.deliveryPointLayerName).layer;
        var newIntersection = mapService.getFeaturesWithinFeature(layer.getSource(), feature);
        vm.newFeatures = [];
        vm.removedFeatures = [];
       // var childIds = vm.newGroupData.children.map(function (child) { return child.id });
        vm.featureIntersection.forEach(function (feature) {
            if (newIntersection.indexOf(feature) == -1) {
                vm.removedFeatures.push(feature);
            }
        });
        newIntersection.forEach(function (feature) {
            if (vm.featureIntersection.indexOf(feature) == -1) {
                vm.newFeatures.push(feature);
                getDPFeatureById(feature);
            }
        });
        for (var i = vm.ignorePoints.length - 1; i >= 0; i--) {
            var point = vm.ignorePoints[i];
            var removedIndex = vm.removedFeatures.indexOf(point);
            var addedIndex = vm.newFeatures.indexOf(point);
            if (removedIndex != -1)
                vm.removedFeatures.splice(removedIndex, 1);
            if (addedIndex != -1)
                vm.newFeatures.splice(addedIndex, 1);
            if (addedIndex == -1 && removedIndex == -1)
                vm.ignorePoints.splice(i, 1);
        }
        $timeout(function () {
            $scope.$apply();
        });
    }

    function onFeatureSelect() {
        if (mapService.getActiveFeature() == null) {
            $timeout(function () {
                vm.selectedFeature = null;
                vm.showInfo = false;
                previousId = null;
               /// hideModify();
              //  vm.cancelEdit();
              //  getGroupsOnScreen();
            });
        } else {
            vm.selectedFeature = mapService.getActiveFeature();
            vm.showInfo = false;
            vm.loading = true;
            if (previousId != vm.selectedFeature.getId()) {
               // vm.cancelEdit();
                //getFeatureDetails(vm.selectedFeature.getId()).then(function () {
                //    vm.loading = false;
                //    vm.showInfo = vm.featureData != null || vm.adding;
                //});
            } else {
                vm.loading = false;
                vm.showInfo = vm.featureData != null;
            }
            previousId = vm.selectedFeature.getId();
        }
    }

    function initialize() {
        groupService.initialize().then(function (response) {
            vm.deliveryGroupTypes = response.DeliveryGroupType;
            vm.servicePointTypes = response.ServicePointType;
        });
    }

    function onSingleAccept(deliveyPoint) {
        vm.addedPoints.push(deliveyPoint);
        var index = vm.availablePoints.indexOf(deliveyPoint);
        vm.availablePoints.splice(index, 1);
    }

    function onSingleReject(deliveyPoint) {
        var index = vm.availablePoints.indexOf(deliveyPoint);
        vm.availablePoints.splice(index, 1);
    }

    function onAcceptAll() {
        vm.availablePoints.forEach(function (feature) {
            vm.addedPoints.push(feature);
        });
        vm.availablePoints = [];
    }

    function onRejectAll() {
        vm.availablePoints = [];
    }

    function createGroup() {
        var groupDto = getGroupDto();
        groupAPIService.CreateGroup().then(function () {
            vm.isReadOnly = true;
        });
        
    }
    
    function getGroupDto() {
        var groupDto = {
            "GroupName": vm.groupName,
            "NumberOfFloors": vm.floors,
            "InternalDistanceMeters": vm.internalDistance,
            "WorkloadTimeOverrideMinutes": vm.overrideTime,
            "TimeOverrideReason": vm.timeReason,
            "TimeOverrideApproved": vm.isOverrideTime,
            "GroupTypeGUID": vm.groupType,
            "ServicePointTypeGUID": vm.ServicePointType,
            "DeliverToReception": vm.isDeliveryToReception,
            "GroupBoundary": null,
            "AddedDeliveryPoints": vm.addedPoints,
            "GroupCoordinates": vm.polygonCoordinates
        }
        return groupDto;
    }

    function getPostcode(point) {
        var subBuildingName = point.values_.subBuildingName ? point.values_.subBuildingName : '';
        var buildingName = point.values_.name ? point.values_.name : '';
        var buildingNumber = point.values_.number ? point.values_.number : '';
        var street = point.values_.street ? point.values_.street : '';
        var postcode = point.values_.postcode ? point.values_.postcode : '';

        return subBuildingName + " " + buildingName + " " + buildingNumber + " " + street + " " + postcode;
    }
}

