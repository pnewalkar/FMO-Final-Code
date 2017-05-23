angular.module('deliveryPoint')
    .service('deliveryPointService', function () {

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

angular.module('deliveryPoint')
    .factory('guidService', function () {

        var guid = '';
        return {
            getGuid: function () {
                return guid;
            },
            setGuid: function (value) {
                guid = value;
            }
        }

    });