'use strict';
angular.module('mapView')
	.controller('MapController', ['$scope',
                                  '$state',
                                  'mapService',
                                  'mapFactory',
                                  'CommonConstants',
                                  MapController])
function MapController($scope,
                       $state,
                       mapService,
                       mapFactory,
                       CommonConstants) {
    var vm = this;
    var contextTitle = vm.contextTitle;
    vm.initialise = initialise();
    vm.initialiseMiniMap = initialiseMiniMap;
    vm.toggleActions = toggleActions;
    vm.oncollapse = oncollapse;
    vm.dotStyle = getDotStyle();
    var unit = vm.selectedDeliveryUnit;
    vm.selectedDeliveryUnit = unit;
    vm.selectFeatures = selectFeatures
    vm.onEnterKeypress = onEnterKeypress;


    $scope.$on('refreshLayers', mapService.refreshLayers);
    $scope.$on("mapToolChange", function (event, button) {
        mapService.mapToolChange(button);
    });
    $scope.$on('redirectTo', function (event, data) {
        vm.contextTitle = data.contextTitle;
        if (data.feature) {
            $state.go("accessLink", { accessLinkFeature: data.feature }, {
                reload: 'accessLink'
            });
        }
        else {
            $state.go("deliveryPoint");
        }
    });
    $scope.$on('showDeliveryPointDetails', function (event, data) {
        if (data.featureType == 'deliverypoint') {
            vm.contextTitle = data.contextTitle;
        }
        else
        {
            vm.contextTitle = CommonConstants.TitleContextPanel;
        }
    });
    $scope.$on("deleteSelectedFeature", function (event) {
        mapService.deleteSelectedFeature();
    });
    mapService.addSelectionListener(selectFeatures);

    function initialise() {
        mapService.initialise();
    }
    function initialiseMiniMap() {
        mapService.initialiseMiniMap();
    }
    function oncollapse(collapse) {
        mapService.oncollapse(collapse);
    }
    function toggleActions() {
        vm.hideActions = !vm.hideActions;
    }
    function syncMinimapAnimation(event) {
        mapService.syncMinimapAnimation(event);
    }
    function getDotStyle() {
        mapService.getDotStyle();
    }
    function selectFeatures(features) {
        mapService.getfeature(features);
        mapService.selectFeatures();
    }

    $scope.$on('zommLevelchanged', function (event, data) {
        vm.zoomLimitReached = data.zoomLimitReached;
        vm.currentScale = data.currentScale
        vm.maximumScale = data.maximumScale
    });

    function onEnterKeypress(currentScale) {
        if (currentScale != '' && (currentScale % 100 == 0 && vm.maximumScale >= currentScale)) {
            mapFactory.setMapScale(currentScale)
        }
    }
}