angular.module('deliveryPoint')
    .factory('deliveryPointService', function () {

        this.deliveryPoint = function () {
            return {
                templateUrl: './delivery-point/delivery-point.template.html',
                clickOutsideToClose: false,
                controller: "DeliveryPointController as vm"
            }
        };
    });

angular.module('deliveryPoint')
    .factory('coordinatesService', function () {

        var coordinates = '';
        return {
            getCordinates: function () {
                return coordinates;
            },
            setCordinates: function (value) {
                coordinates = value;
            }
        }

    });