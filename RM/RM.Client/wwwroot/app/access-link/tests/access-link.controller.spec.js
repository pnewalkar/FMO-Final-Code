describe('Access Link: Controller', function () {

    var accessLinkAPIService;
    var $scope;
    var $mdDialog;
    var $state;
    var mapService;
    var mapFactory;
    var coordinatesService;
    var roadLinkGuidService;
    var accessLinkCoordinatesService;
    var intersectionPointService;
    var GlobalSettings;
    var guidService;
    var $stateParams;
    var vm;
    var $q;
    var deffered;
    var response;
    var CommonConstants;    

    stateMockData = { "selectedUnit": { "displayText": "BN    Worthing  Office", "ID": "b51aa229-c984-4ca6-9c12-510187b81050", "icon": "fa-map-marker delivery", "$$mdSelectId": 1, "$$hashKey": "object:114" } };
    stateParamsMockData = { "accessLinkFeature": "geometri" };

    beforeEach(function () {
        module('accessLink');

        module(function ($provide) {
            $provide.value('CommonConstants', {DeliveryPointActionName: 'Delivery Point'});            
            $provide.value('GlobalSettings', { deliveryPointLayerName: 'mapLayer' });
            MockMdCancel = jasmine.createSpy();
            $provide.factory('$mdDialog', function () {
                return { cancel: MockMdCancel };
            });

            $provide.factory('accessLinkAPIService', function ($q) {
                function GetAdjPathLength(accessLinkManualCreateModelDTO) {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function CreateAccessLink(accessLinkDTO) {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function CheckAccessLinkIsValid(accessLinkManualCreateModelDTO) {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                return {
                    GetAdjPathLength: GetAdjPathLength,
                    CreateAccessLink: CreateAccessLink,
                    CheckAccessLinkIsValid: CheckAccessLinkIsValid
                }
            });

            $provide.factory('mapService', function () {
                function deleteAccessLinkFeature(feature) { return; }
                function refreshLayers() { return []; }
                return {
                    deleteAccessLinkFeature: deleteAccessLinkFeature,
                    refreshLayers: refreshLayers
                }
            });
        
            $provide.factory('mapFactory', function () {
                //Set up variable for mapFactory
                var map = {
                    getView: function () {
                        return {
                            calculateExtent: function () { }
                        }
                    },
                    getSize: function () { }
                };

                function initialiseMap() { }
                function getMap() { return map; }
                function setAccessLink() { }
                return {
                    initialiseMap: initialiseMap,
                    getMap: getMap,
                    setAccessLink: setAccessLink
                }
            });

            //inject with mockdata mapFactory
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

            //inject with mockdata mapFactory
            $provide.factory('guidService', function () {

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

            $provide.value('$state', stateMockData);
            $provide.value('$stateParams', stateParamsMockData);
        });


        //get Instance of controller with inject properties
        inject(function (_$controller_, _$rootScope_, _$q_, _accessLinkAPIService_, _$mdDialog_, _$state_, _mapService_, _mapFactory_, _coordinatesService_, _roadLinkGuidService_, _accessLinkCoordinatesService_, _intersectionPointService_, _GlobalSettings_, _guidService_, _$stateParams_,_CommonConstants_) {
            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            accessLinkAPIService = _accessLinkAPIService_;
            $mdDialog = _$mdDialog_;
            $state = _$state_;
            mapService = _mapService_;
            mapFactory = _mapFactory_;
            guidService = _guidService_;
            coordinatesService = _coordinatesService_,
            roadLinkGuidService = _roadLinkGuidService_;
            accessLinkCoordinatesService = _accessLinkCoordinatesService_;
            intersectionPointService = _intersectionPointService_;
            $stateParams = _$stateParams_;
            $q = _$q_;
            deferred = _$q_.defer();

            vm = _$controller_('AccessLinkController', {
                accessLinkAPIService: accessLinkAPIService,
                $scope: $scope,
                $mdDialog: $mdDialog,
                $state: $state,
                mapService: mapService,
                mapFactory: mapFactory,
                coordinatesService: coordinatesService,
                roadLinkGuidService: roadLinkGuidService,
                accessLinkCoordinatesService: accessLinkCoordinatesService,
                intersectionPointService: intersectionPointService,
                GlobalSettings: _GlobalSettings_,
                guidService: guidService,
                $stateParams: $stateParams,
                CommonConstants : _CommonConstants_
            })
        });

    });

    it('should be contextTitle has been undefined', function () {
        expect(vm.contextTitle).toBeUndefined();
    });

    it('should be set default enableBack is `true`', function () {
        expect(vm.enableBack).toBe(true);
    });

    it('should be set default enableBack is `false`', function () {
        expect(vm.enableSave).toBe(false);
    });

    it('should be set default pathLength is `null`', function () {
        expect(vm.pathLength).toBe(null);
    });

    it('should promise to return a success response once createAccessLink method is called', function () {
        var deferred = $q.defer();
        var response = true;

        //Once promsie resolved then we have to check all spy has been called
        spyOn(accessLinkAPIService, 'CreateAccessLink').and.returnValue(deferred.promise);
        spyOn(mapFactory, 'setAccessLink');
        spyOn(coordinatesService, 'setCordinates');
        spyOn(mapService, 'refreshLayers');
        spyOn($rootScope, '$broadcast');

        vm.createAccessLink();

        deferred.resolve(response);
        $scope.$digest();

        expect(accessLinkAPIService.CreateAccessLink).toHaveBeenCalled();
        expect(response).toBe(true);
        expect(mapFactory.setAccessLink).toHaveBeenCalled();
        expect(coordinatesService.setCordinates).toHaveBeenCalled();
        expect(coordinatesService.setCordinates).toHaveBeenCalledWith('');
        expect(mapService.refreshLayers).toHaveBeenCalled();
        expect($rootScope.$broadcast).toHaveBeenCalledWith('redirectTo', { contextTitle: 'Delivery Point' });
        expect($rootScope.$broadcast).toHaveBeenCalledWith('disablePrintMap', { disable: false });

    });

    it('should promise to return a success response true once accessLink method is called', function () {
        var deferred = $q.defer();
        var response = true;

        spyOn(accessLinkAPIService, 'CheckAccessLinkIsValid').and.returnValue(deferred.promise);
        spyOn(accessLinkAPIService, 'GetAdjPathLength').and.returnValue(deferred.promise);

        vm.accessLink();

        deferred.resolve(response);
        $scope.$digest();

        expect(accessLinkAPIService.CheckAccessLinkIsValid).toHaveBeenCalled();
        expect(response).toBe(true);
        expect(accessLinkAPIService.GetAdjPathLength).toHaveBeenCalled();
        expect(vm.pathLength).toBe(true);
        expect(vm.enableSave).toBe(true);
        expect(vm.enableBack).toBe(true);
    });

    it('should be clear access link value when clearAccessLink method is called', function () {

        spyOn(accessLinkCoordinatesService, 'setCordinates');
        spyOn(roadLinkGuidService, 'setRoadLinkGuid');
        spyOn(intersectionPointService, 'setIntersectionPoint');
        spyOn(mapService, 'deleteAccessLinkFeature');

        vm.clearAccessLink();

        expect(accessLinkCoordinatesService.setCordinates).toHaveBeenCalledWith(null);
        expect(roadLinkGuidService.setRoadLinkGuid).toHaveBeenCalledWith(null);
        expect(intersectionPointService.setIntersectionPoint).toHaveBeenCalledWith(null);
        expect(mapService.deleteAccessLinkFeature).toHaveBeenCalledWith('geometri');
        expect(vm.enableSave).toBe(false);
        expect(vm.pathLength).toBe('');

    });

    it('should be accessLinkFeature defined in stateParams', function () {
        expect(vm.accessLinkFeature).toBeDefined();
        expect(vm.accessLinkFeature).toBe(stateParamsMockData.accessLinkFeature);
    });
});

