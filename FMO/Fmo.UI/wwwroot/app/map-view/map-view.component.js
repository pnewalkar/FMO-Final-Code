angular.
module('mapView').
component('mapView', {
    restrict: 'E',
    scope: {
        selectedDeliveryUnit: "="
    },
    templateUrl: './map-view/map-view.template.html',
	controller: 'MapController as vm'
});