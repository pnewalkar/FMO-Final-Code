
MyApp.directive('deliveryPoint', function () {
    return {
        restrict: 'E',
        scope: {},
        templateUrl: '/app/fmo/delivery-point/delivery-point.template.html',
        controller: 'deliveryPointController'
    }
});