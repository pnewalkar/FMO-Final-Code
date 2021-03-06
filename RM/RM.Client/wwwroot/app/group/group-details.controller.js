﻿angular
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
    vm.availablePoints = [];
    vm.initialize = initialize;
    vm.onSingleAccept = onSingleAccept;
    vm.onSingleReject = onSingleReject;
    vm.onAcceptAll = onAcceptAll;
    vm.onRejectAll = onRejectAll;
    vm.createGroup = createGroup;
    vm.isReadOnly = false;
    vm.getSummarizedAddresses = getSummarizedAddresses;
    vm.toggle = false;

    // vm.ServicePointType = "Inside";
    vm.checkDeliveryGroupType = checkDeliveryGroupType;
    vm.groupType = "";
    vm.groupType.value = "Complex";

    // vm.ServicePointType = "Inside";
    vm.getPostcode = getPostcode;

    vm.initialize();
    mapService.addSelectionListener(onFeatureSelect);

    $rootScope.$on('setAssociatedDp', function (event, associatedDP) {
        vm.polygonCoordinates = associatedDP.listOfAssociatedDP.getGeometry().flatCoordinates;
        editGroup();
    });

    function editGroup() {
        vm.editingFeature = mapService.getActiveFeature();
        var layer = mapService.getLayer(GlobalSettings.deliveryPointLayerName).layer;
        var groupsLayer = mapService.getLayer(GlobalSettings.groupLayerName).layer;
        vm.initialCoordinates = mapService.getActiveFeature().getGeometry().getCoordinates();
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

        var addedPoints = vm.addedPoints;
        vm.distinctPostcodes = {};
        for (var pivot = 0; pivot < addedPoints.length; pivot++) {
            var postcode = addedPoints[pivot].values_.postcode;
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
        groupAPIService.CreateGroup(groupDto).then(function () {
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
            "GroupTypeGUID": vm.groupType.id,
            "DeliverToReception": vm.isDeliveryToReception,
            "GroupCoordinates": vm.initialCoordinates,
            "AddedDeliveryPoints": getDeliveryPointDetails(vm.addedPoints)
        }
        return groupDto;
    }
    function getDeliveryPointDetails(addedPoints) {
        var deliveryPointDetails = [];

        angular.forEach(addedPoints, function (value, key) {
            var details = {
                "ID": value.values_.deliveryPointId,
                "PostalAddress": {
                    "OrganisationName": value.values_.organisationName,
                    "BuildingName": value.values_.name,
                    "SubBuildingName": value.values_.subBuildingName,
                    "Postcode": value.values_.postcode
                }
            }
            deliveryPointDetails.push(details);
        });

        return deliveryPointDetails;
    }

    function checkDeliveryGroupType() {
        if (vm.groupType === "Complex") {
            vm.groupType.value = "Complex";
            vm.floors = "Floor1";
            vm.internalDistance = "InternalDist";
        }

    }

    function getSummarizedAddresses() {
        vm.toggle = !vm.toggle;
        var addedPoints = vm.addedPoints;
        vm.distinctPostcodes = {};
        for (var index = 0; index < addedPoints.length; index++) {
            var postcode = addedPoints[index].values_.postcode;
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

    function getPostcode(point) {
        var subBuildingName = point.values_.subBuildingName ? point.values_.subBuildingName : '';
        var buildingName = point.values_.name ? point.values_.name : '';
        var buildingNumber = point.values_.number ? point.values_.number : '';
        var street = point.values_.street ? point.values_.street : '';
        var postcode = point.values_.postcode ? point.values_.postcode : '';

        return subBuildingName + " " + buildingName + " " + buildingNumber + " " + street + " " + postcode;
    }
}