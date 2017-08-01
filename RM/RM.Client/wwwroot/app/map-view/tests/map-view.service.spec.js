describe('Map-View: Service', function () {

    var $q;
    var deferred;
    var stateMockData;
    var stateparamMockData;
    var mapService;
    var routeLogAPIService;
    var CommonConstants;
    var scope;
    var MockCommonConstants;
    var GlobalSettings;

    //Mocking Value of Global Setting
    var MockGlobalSettings = {
        apiUrl: 'http://localhost:34583/api',
        env: 'localhost', // Here set the current environment
        indexUrl: '',
    };

    /////////////MOCK CONSTANTS VALUES/////////////
    var MockCommonConstants = {};

   //Load our module and inject with dependencies provider
    beforeEach(function () {

        module('mapView'); //load module

        //Inejct with mockdata mapFactory with attached all function
        module(function ($provide) {
            $provide.factory('mapFactory', function ($q) {
                function getPolygonTransparency() {
                    deferred = $q.defer();
                    deferred.resolve();
                    return deferred.promise;
                }
                return {
                    getPolygonTransparency: getPolygonTransparency,
                };
            });

            $provide.factory('mapStylesFactory', function ($q) {
                return '';
            });

            $provide.factory('coordinatesService', function ($q) {

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

            $provide.factory('roadLinkGuidService', function ($q) {

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

            $provide.factory('accessLinkCoordinatesService', function ($q) {

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

            $provide.factory('intersectionPointService', function ($q) {

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

            $provide.factory('accessLinkAPIService', function ($q) {

                return '';
            });

            $provide.factory('guidService', function ($q) {

                return '';
            });
        });


        module(function ($provide) {
            $provide.value('GlobalSettings', MockGlobalSettings);
            $provide.value('$state', stateMockData);
            $provide.value('$stateParams', stateparamMockData);
        });

        //Injec mockdaa CommonConstants with attached all constant value
        module(function ($provide) {
            $provide.constant('CommonConstants', MockCommonConstants);
        });

        //get Instance of controller with inject properties
        inject(function (_$rootScope_, _$q_, _mapService_, _$http_, _mapFactory_, _GlobalSettings_, _$state_, _$stateParams_) {
            scope = _$rootScope_.$new(); //Generate new scope
            $q = _$q_;
            deferred = _$q_.defer();
            mapService = _mapService_;
            $http = _$http_;
            mapFactory = _mapFactory_;
            GlobalSettings = _GlobalSettings_;
            $state = _$state_;
            $stateParams = _$stateParams_;
        });
    });

    xit('Polygon Transparency method should have been called and return the opacity', function () {
        spyOn(mapFactory, 'getPolygonTransparency').and.callFake(function () {
            var deferred = $q.defer(); 
            deferred.resolve([{value: 10}]);
            return deferred.promise;
        });
        mapService.SetPolygonTransparency();
        scope.$apply();
        expect(mapFactory.getPolygonTransparency).toHaveBeenCalled();
        expect(mapFactory.getPolygonTransparency).toHaveBeenCalledTimes(1);
    });
});
