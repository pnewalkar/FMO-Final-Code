'use strict';
angular.module('mapView')
	.controller('MapController', ['$scope',                                                               
                                  'mapService',                                 
                                  MapController])
function MapController($scope,
                       mapService) {
    var vm = this;    
    vm.initialise = initialise();
    vm.initialiseMiniMap = initialiseMiniMap;
    vm.toggleActions = toggleActions;
    vm.oncollapse = oncollapse;
    vm.dotStyle = getDotStyle();
    var unit = vm.selectedDeliveryUnit;
    vm.selectedDeliveryUnit = unit;
    $scope.$on('refreshLayers',mapService.refreshLayers);
    $scope.$on("mapToolChange", function (event, button) {
        mapService.mapToolChange(button);
    });
    $scope.$on("deleteSelectedFeature", function (event) {
        mapService.deleteSelectedFeature();
    }); 
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
}
