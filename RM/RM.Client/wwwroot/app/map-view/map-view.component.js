angular.
module('mapView').
component('mapView', {
    bindings: {
        selectedDeliveryUnit: "=",
        contextTitle: "="
    },
    templateUrl: './map-view/map-view.template.html',
    controller: 'MapController as vm'
});