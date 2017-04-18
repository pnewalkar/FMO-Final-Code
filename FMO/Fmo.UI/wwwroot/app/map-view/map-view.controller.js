'use strict';
angular.module('mapView')
	.controller('MapController', ['$scope',
                                  'mapService',
                                  'mapFactory',
                                  MapController])
function MapController($scope,
                       mapService, mapFactory) {
    var vm = this;
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
