'use strict';
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
    var response;
    var CommonConstants;
    var $rootScope;      
    
    beforeEach(function () {
        module('accessLink');
        module(function ($provide) {
            $provide.value('$state', { "selectedUnit": { "displayText": "BN    Worthing  Office", "ID": "b51aa229-c984-4ca6-9c12-510187b81050", "icon": "fa-map-marker delivery"}});
            $provide.value('$stateParams',{"accessLinkFeature": "geometri"});
            $provide.value('CommonConstants', {DeliveryPointActionName: 'Delivery Point'});            
            $provide.value('GlobalSettings', { deliveryPointLayerName: 'mapLayer' });            
            $provide.factory('$mdDialog', function () {
                return { cancel: jasmine.createSpy()};
            });

            $provide.factory('accessLinkAPIService', function ($q) {
                function GetAdjPathLength(accessLinkManualCreateModelDTO){}
                function CreateAccessLink(accessLinkDTO){}
                function CheckAccessLinkIsValid(accessLinkManualCreateModelDTO) {
                    var deferred = $q.defer();
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
            
            $provide.factory('coordinatesService', function ($q) {
                return {
                    getCordinates: function () {},                    
                    setCordinates: function (value) {}
                }
            });

            $provide.factory('guidService', function () {
                return {
                    getGuid:function(){},
                    setGuid:function(value){}
                }                             
            });            
        });
        
        inject(function (
            _$controller_,
            _$rootScope_,
            _$q_,
            _accessLinkAPIService_,
            _$mdDialog_,
            _$state_,
            _mapService_,
            _mapFactory_,
            _coordinatesService_,
            _roadLinkGuidService_,
            _accessLinkCoordinatesService_,
            _intersectionPointService_,
            _GlobalSettings_,
            _guidService_,
            _$stateParams_,
            _CommonConstants_) {

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

    it('should contextTitle is undefined', function () {
        expect(vm.contextTitle).toBeUndefined();
    });

    it('should set default enableBack is `true`', function () {
        expect(vm.enableBack).toBe(true);
    });

    it('should set default enableBack is `false`', function () {
        expect(vm.enableSave).toBe(false);
    });

    it('should set default pathLength is `null`', function () {
        expect(vm.pathLength).toBe(null);
    });

    it('should promise to return a success response once createAccessLink method is called', function () {
        var deferred = $q.defer();
        var response = true;        
        spyOn(accessLinkAPIService, 'CreateAccessLink').and.returnValue(deferred.promise);
        spyOn(mapFactory, 'setAccessLink');
        spyOn(coordinatesService, 'setCordinates');
        spyOn(mapService, 'refreshLayers');
        spyOn($rootScope, '$broadcast');

        vm.createAccessLink();

        deferred.resolve(response);
        $rootScope.$apply();

        expect(accessLinkAPIService.CreateAccessLink).toHaveBeenCalled();        
        expect(mapFactory.setAccessLink).toHaveBeenCalled();
        expect(vm.enableBack).toBe(false);
        expect(vm.enableSave).toBe(false);
        expect(vm.pathLength).toBe('');
        expect(coordinatesService.setCordinates).toHaveBeenCalledWith('');
        expect($rootScope.state).toBe(true);
        expect(mapService.refreshLayers).toHaveBeenCalled();
        expect($rootScope.$broadcast).toHaveBeenCalledWith('redirectTo', { contextTitle: 'Delivery Point' });
        expect($rootScope.$broadcast).toHaveBeenCalledWith('disablePrintMap', { disable: false });
        expect(vm.isOnceClicked).toBe(false);
    });

    it('should promise to return a success response true one by one once accessLink method is called', function () {
        var deferred1 = $q.defer();
        var deferred2 = $q.defer();
        var deferred1_response = true;
        var deferred2_response = true;

        spyOn(accessLinkAPIService, 'CheckAccessLinkIsValid').and.returnValue(deferred1.promise);
        spyOn(accessLinkAPIService, 'GetAdjPathLength').and.returnValue(deferred2.promise);

        vm.accessLink();

        deferred1.resolve(deferred1_response);
        deferred2.resolve(deferred2_response);
        $rootScope.$apply();

        expect(accessLinkAPIService.CheckAccessLinkIsValid).toHaveBeenCalled();        
        expect(accessLinkAPIService.GetAdjPathLength).toHaveBeenCalled();
        expect(vm.pathLength).toBe(true);
        expect(vm.enableSave).toBe(true);
        expect(vm.enableBack).toBe(true);
    });

    it('should promise to return a `empty` response once accessLink method is called', function () {
        var deferred1 = $q.defer();
        var deferred2 = $q.defer();
        var deferred1_response = false;
        var deferred2_response = false;

        spyOn(accessLinkAPIService, 'CheckAccessLinkIsValid').and.returnValue(deferred1.promise);
        spyOn(accessLinkAPIService, 'GetAdjPathLength').and.returnValue(deferred2.promise);
        spyOn(accessLinkCoordinatesService, 'setCordinates');
        spyOn(roadLinkGuidService, 'setRoadLinkGuid');
        spyOn(intersectionPointService, 'setIntersectionPoint');
        spyOn(mapService, 'deleteAccessLinkFeature');

        vm.accessLink();

        deferred1.resolve(deferred1_response);
        $rootScope.$apply();

        expect(accessLinkAPIService.CheckAccessLinkIsValid).toHaveBeenCalled();        
        expect(accessLinkAPIService.GetAdjPathLength).not.toHaveBeenCalled();
        expect(accessLinkCoordinatesService.setCordinates).toHaveBeenCalledWith(null);
        expect(roadLinkGuidService.setRoadLinkGuid).toHaveBeenCalledWith(null);
        expect(intersectionPointService.setIntersectionPoint).toHaveBeenCalledWith(null);
        expect(mapService.deleteAccessLinkFeature).toHaveBeenCalledWith('geometri');
        expect(vm.enableSave).toBe(false);
        expect(vm.pathLength).toBe('');
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

    it('should accessLinkFeature defined in stateParams', function () {        
        expect(vm.accessLinkFeature).toBe($stateParams.accessLinkFeature);
    });
});

