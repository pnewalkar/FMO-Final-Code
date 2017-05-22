angular.module('accessLink')
    .service('accessLinkService', function () {

        this.accessLink = function () {
            return {
                templateUrl: './access-link/acccess-link.template.html',
                clickOutsideToClose: false,
                controller: "AccessLinkController as vm"
            }
        };
    });

angular.module('accessLink')
    .factory('accessLinkCoordinatesService', function () {

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

angular.module('accessLink')
    .factory('roadLinkGuidService', function () {

        var roadLinkGuid = '';
        return {
            getRoadLinkGuid: function () {
                return roadLinkGuid;
            },
            setRoadLinkGuid: function (value) {
                roadLinkGuid = value;
            }
        }

    });

angular.module('accessLink')
    .factory('intersectionPointService', function () {

        var intersectionPoint = '';
        return {
            getIntersectionPoint: function () {
                return intersectionPoint;
            },
            setIntersectionPoint: function (value) {
                intersectionPoint = value;
            }
        }

    });