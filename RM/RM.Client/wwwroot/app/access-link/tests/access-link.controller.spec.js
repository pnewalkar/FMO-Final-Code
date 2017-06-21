describe('accessLink: Controller', function () {

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

    // set default stateParams
    stateMockData = {"selectedUnit":{"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","$$mdSelectId":1,"$$hashKey":"object:114"}};
    stateParamsMockData = {"accessLinkFeature":"adsdf"};

    //Set GlobalSettings mockData
    //Mocking Value of Global Setting
    var MockGlobalSettings = {
    	apiUrl: 'http://localhost:34583/api',
	    env: 'localhost', // Here set the current environment
	    indexUrl: '',
    	getAdjustedPathLength: "/accessLink/GetWorkloadLength/",
		createAccessLink: "/accessLink/CreateManual/",
		checkAccessLinkIsValid: "/accessLink/CheckAccessLinkIsValid/",
		deliveryPointLayerName: "Delivery Points"
		//----
    };
   
    //Load our module and inject with dependencies provider
    beforeEach(function () {

        module('accessLink'); //load module

        //inject with $mdDialog 
        module(function ($provide) {
        	$provide.value('GlobalSettings', MockGlobalSettings);
       		MockMdCancel = jasmine.createSpy();
		    $provide.factory('$mdDialog', function() {
		        return {cancel: MockMdCancel};
		    });
        });

        //inject with mockdata accessLinkAPIService
        module(function ($provide) {
            $provide.factory('accessLinkAPIService', function ($q) {
            	function GetAdjPathLength(accessLinkManualCreateModelDTO) {
                    deferred = $q.defer();
                    deferred.resolve(true);
                    return deferred.promise;
                }

               function CreateAccessLink(accessLinkDTO) {
                    deferred = $q.defer();
                    deferred.resolve(true);
                    return deferred.promise;
                }

                function CheckAccessLinkIsValid(accessLinkManualCreateModelDTO) {
                    deferred = $q.defer();
                    deferred.resolve(true);
                    return deferred.promise;
                }

                

                return {
                	GetAdjPathLength: GetAdjPathLength,
                	CreateAccessLink: CreateAccessLink,
                	CheckAccessLinkIsValid: CheckAccessLinkIsValid
                }
            });
        });

        //inject with mockdata mapService
        module(function ($provide) {
            $provide.factory('mapService', function () {
               function deleteAccessLinkFeature(feature) {
			        return;
			    }

			    function refreshLayers(){ return []; }

               return{
               		deleteAccessLinkFeature: deleteAccessLinkFeature,
               		refreshLayers: refreshLayers
               }
            });
        });

        //inject with mockdata mapFactory
        module(function ($provide) {
            $provide.factory('mapFactory', function () {

            	//Set up variable for mapFactory
	            var map = {
			        getView: function(){
			          return {
				        calculateExtent: function(){
				          
				        	}
				    	}
			        },
			        getSize: function(){
			          
			        }        
			    };

			    
				var miniMap = null;
			    var view = null;
			    var vectorLayer = null;
			    var vectorSource = null;
			    var viewMiniMap = null;
			    var customScaleLine = null;

			    var layers = [];

			    var units = null;
			    var dpi = 25.4 / 0.28;
			    var mpu = null;

			    var defaultResolutions = [700.0014000028002, 336.0006720013441, 168.00033600067206, 84.00016800033603, 39.20007840015681, 19.600039200078406, 9.800019600039203, 5.600011200022402, 2.800005600011201, 2.240004480008961, 1.1200022400044805, 0.5600011200022402, 0.2800005600011201, 0.14000028000056006, 0.05600011200022402, 0.02800005600011201];
			    var definedScales = [2500000, 1200000, 600000, 300000, 140000, 70000, 35000, 20000, 10000, 8000, 4000, 2000, 1000, 500, 200, 100];
			    var zoomLimitReached = false;
			    var defaultZoomScale = 2000;

			    var availableResolutionForCurrentExtent = [];
			    var maxScale = null;
			    var BNGProjection = 'EPSG:27700';

            	function initialiseMap() {
			        
			    }

			    function initialiseMiniMap() {
			       
			    }

			    function setMapScale(scale) {
			 
			    }

			    function getVectorLayer() {
			        return vectorLayer;
			    }

			    function getVectorSource() {
			        return vectorSource;
			    }

			    function getAllLayers() {
				    return layers;
				}

	            function getMap() {
				    return map;
				}

				function getMiniMap() {
				    return miniMap;
				}

				function getLayer(layerName) {
			        return;
			    }

			    function addLayer(layerObj) {

			        return layerObj;
			    }

			    function removeLayer(layerName) {
			        
			    }

			    function createLayerAsync(paramObj) {
			        return ;
			    }

			    function convertGeoJsonToOl(featureData, formatOptions) {
			        return;
			    }

			    function deleteAllFeaturesFromLayer(layerName) {
			        return;
			    }

			    function getLayerDataFromUrl(url) {
			        return;
			    }

			    function convertGeoJsonToOl(featureData, formatOptions) {
			        return;
			    }

			    function createLayerFromFeatures(features) {
			        return;
			    }

			    function addFeaturesToMap(layerName, layerGroup, features, keys, selectorVisible) {
			        return;
			    }

				function getShapeAsync(url) {
			        return ;
			    }

			    function getResolutionFromScale(scale) {
				    return;
				}

				function getScaleFromResolution(resolution) {
			        return ;
			    }

			    function setUnitBoundaries(bbox, center, unitBoundaryGeoJSONData) {
			        return;
			    }

			    function setDeliveryPoint(long, lat) {
			        return;   
			    }

			    function locateDeliveryPoint(long, lat) {
			        return;
			    }

			    function setAccessLink() {
			        return;

			    }

			    function GetRouteForDeliveryPoint(deliveryPointId) {
			        var deferred = $q.defer();
			        return deferred.promise;
			    }

				return {
					initialiseMap: initialiseMap,
			        getMap: getMap,
			        initialiseMiniMap: initialiseMiniMap,
			        getShapeAsync: getShapeAsync,
			        getMiniMap: getMiniMap,
			        getVectorLayer: getVectorLayer,
			        getVectorSource: getVectorSource,
			        getAllLayers: getAllLayers,
			        addLayer: addLayer,
			        removeLayer: removeLayer,
			        getLayer: getLayer,
			        createLayerAsync: createLayerAsync,
			        convertGeoJsonToOl: convertGeoJsonToOl,
			        deleteAllFeaturesFromLayer: deleteAllFeaturesFromLayer,
			        addFeaturesToMap: addFeaturesToMap,
			        defaultResolutions: defaultResolutions,
			        definedScales: definedScales,
			        getResolutionFromScale: getResolutionFromScale,
			        getScaleFromResolution: getScaleFromResolution,
			        setUnitBoundaries: setUnitBoundaries,
			        setDeliveryPoint: setDeliveryPoint,
			        setAccessLink : setAccessLink,
			        setMapScale: setMapScale,
			        locateDeliveryPoint: locateDeliveryPoint,
			        GetRouteForDeliveryPoint: GetRouteForDeliveryPoint
		    	}
            });
        });

        //inject with mockdata mapFactory
        module(function ($provide) {
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
        });

        //inject with mockdata mapFactory
        module(function ($provide) {
            $provide.factory('guidService', function ($q) {
                
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
        });


        //Inejct with mockdata state
        module(function ($provide) {
            $provide.value('$state', stateMockData);
         
        });

        //Inejct with mockdata stateParams
        module(function ($provide) {
            $provide.value('$stateParams', stateParamsMockData);
         
        });


        //get Instance of controller with inject properties
        inject(function (_$controller_,_$rootScope_,_accessLinkAPIService_,_$mdDialog_,_$state_,_mapService_,_mapFactory_,_coordinatesService_,_roadLinkGuidService_,_accessLinkCoordinatesService_,_intersectionPointService_,_GlobalSettings_,_guidService_,_$stateParams_) {
            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            accessLinkAPIService = _accessLinkAPIService_;
            $mdDialog = _$mdDialog_;
            $state = _$state_;
            mapService = _mapService_;
            mapFactory = _mapFactory_;
            coordinatesService = _coordinatesService_;
            roadLinkGuidService = _roadLinkGuidService_;
            accessLinkCoordinatesService = _accessLinkCoordinatesService_;
            intersectionPointService = _intersectionPointService_;
            GlobalSettings = _GlobalSettings_;
            guidService = _guidService_;
            $stateParams = _$stateParams_;


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
                $stateParams: $stateParams
                
            })

        });

    });

	it('should be contextTitle has been undefined', function() {
		expect(vm.contextTitle).toBeUndefined();
	});

    it('should be set default enableBack is `true`', function() {
    	expect(vm.enableBack).toBe(true);
    });

    it('should be set default enableBack is `false`', function() {
    	expect(vm.enableSave).toBe(false);
    });

    it('should be set default pathLength is `null`', function() {
    	expect(vm.pathLength).toBe(null);
    });

    it('should be initialize accessLink', function() {
    	vm.initialize();

    	//Spy to check accessLink have been called 
    	spyOn(vm,'accessLink');
    	vm.accessLink();

    	expect(vm.accessLink).toHaveBeenCalled();
    });

    it('should be return promise response when call createAccessLink', function(done) {
    	
    	//we need to cal api service and if return response == true then 
    	//call to factory function
    	//then check to back and save button is false
    	//call to coordinateService of setCordinates
    	//call to mapservice.refreshLayers
    	//rootScope to broadcast redirect to {object passs}

    	var accessLinkDTO = {
            "OperationalObjectPoint": "test",
            "AccessLinkLine": "test",
            "NetworkLinkGUID": "test",
            "OperationalObjectGUID": "test",
            "NetworkIntersectionPoint": "test",
            "Workloadlength": "test"
        }

    	//spyOn(vm,'createAccessLink').and.callThrough();
    	vm.createAccessLink();

    	//Call to load Scenario
        accessLinkAPIService.CreateAccessLink(accessLinkDTO).then(function (response) {
          if(response==true){

          	spyOn(mapFactory,'setAccessLink');
          	mapFactory.setAccessLink();

          	spyOn(coordinatesService,'setCordinates');
          	coordinatesService.setCordinates('');
          	

          	spyOn(mapService,'refreshLayers');
          	mapService.refreshLayers();


          	expect(response).toBe(true);
          	expect(mapFactory.setAccessLink).toHaveBeenCalled();
          	expect(coordinatesService.setCordinates).toHaveBeenCalled();
          	expect(coordinatesService.setCordinates).toHaveBeenCalledWith('');
          	expect(mapService.refreshLayers).toHaveBeenCalled();
          	expect(vm.enableSave).toBe(false);
          	expect(vm.enableBack).toBe(false);
          	expect(vm.pathLength).toBe('');

          	done(); //Asyn call
          }

        });

        $rootScope.$apply();

    });

    it('should be return promise response when called accessLink', function() {

    	var accessLinkManualCreateModelDTO = {
            "BoundingBoxCoordinates" : 'testCoordinates',
            "OperationalObjectPoint": 'testObjectPoiint',
            "AccessLinkLine": 'test link',
            "NetworkLinkGUID": 'test NetworkLinkGUID',
            "OperationalObjectGUID": 'test sevice id',
            "NetworkIntersectionPoint": 'test network intersectionpoint'
        };

    	vm.accessLink();

    	//Call to load Scenario
        accessLinkAPIService.CheckAccessLinkIsValid(accessLinkManualCreateModelDTO).then(function (response) {
          if(response==true){

          	accessLinkAPIService.GetAdjPathLength(accessLinkManualCreateModelDTO).then(function (response) {

	          	expect(response).toBe(true);
	          	expect(vm.pathLength).toBe(response);
	          	expect(vm.enableSave).toBe(true);
	          	expect(vm.enableBack).toBe(true);
	        });

          }

        });

        $rootScope.$apply();
    });

    it('should be clearAccessLink', function() {
    	vm.clearAccessLink();
 		
    	expect(vm.accessLinkFeature).toBeDefined();
    	expect(vm.enableSave).toBe(false);
    	expect(vm.pathLength).toBe('');

    });

   	it('should be accessLinkFeature defined in stateParams', function() {
   		expect(vm.accessLinkFeature).toBeDefined();
   		expect(vm.accessLinkFeature).toBe(stateParamsMockData.accessLinkFeature);
   	});	
    



});

