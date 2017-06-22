describe('deliveryPoint: Controller', function () {

    var mapToolbarService;
    var $scope;
    var $mdDialog;
    var popUpSettingService;
    var deliveryPointAPIService;
    var $filter;
    var mapFactory;
    var coordinatesService;
    var guidService;
    var $state;
    var $stateParams;
    var deliveryPointService
    var vm;
    var $q;
    var deffered;
    var $controller;
    var stateMockData;
    var stateParamsMockingData;

    // set default stateParams
    stateMockData = {"selectedUnit":{"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","$$mdSelectId":1,"$$hashKey":"object:114"}};
    stateParamsMockingData = {
      hide:true,
      positionedThirdPartyDeliveryPointList: 'thirdparty',
      positionedDeliveryPointList: 'poiintlist',
      deliveryPointList: '2ndpointlist'
    };

    //Set GlobalSettings mockData
    //Mocking Value of Global Setting
    var MockGlobalSettings = {
      apiUrl: 'http://localhost:34583/api',
      env: 'localhost', // Here set the current environment
      indexUrl: '',
      /*getAdjustedPathLength: "/accessLink/GetWorkloadLength/",
      createAccessLink: "/accessLink/CreateManual/",
      checkAccessLinkIsValid: "/accessLink/CheckAccessLinkIsValid/",*/
      deliveryPointLayerName: "Delivery Points"
      //----
    };
   
   
    //Load our module and inject with dependencies provider
    beforeEach(function () {

        module('deliveryPoint'); //load module

        //inject with $mdDialog 
        module(function ($provide) {
          $provide.value('GlobalSettings', MockGlobalSettings);
          //MockMdCancel = jasmine.createSpy();
          //MockclickOutsideToClose
          $provide.factory('$mdDialog', function() {
            //return {confirm: MockMdCancel};
            return {};
          });
        });

        //inject with mockdata mapToolbarService
        module(function ($provide) {
            $provide.factory('mapToolbarService', function ($q) {
              function getShapeForButton(button) {
                    return [];
                }

                return {
                  getShapeForButton: getShapeForButton
                }
            });
        });


        module(function($provide){
          $provide.factory('referencedataApiService', function(){
              
              return {
                
              }
          });
        });

        module(function ($provide) {
            $provide.factory('stringFormatService', function ($q) {                
                return '';
            });
        });

        module(function($provide){
          $provide.factory('deliveryPointService', function($q){
              function initialize() {
                  deferred = $q.defer();
                  deferred.resolve({deliveryPointTypes:'single',dpUseType:'circle'});
                  return deferred.promise;
              }

              function resultSet(query) {
                  deferred = $q.defer();
                  deferred.resolve('');
                  return deferred.promise;
              }

              function openModalPopup(popupSetting) {
                  //$mdDialog.show(popupSetting)
              }

              function closeModalPopup() {
                  $mdDialog.hide();
              }

              function getPostalAddress(postcode) {
                  deferred = $q.defer();
                  deferred.resolve('');
                  return deferred.promise;
              }

              function bindAddressDetails(notyetBuilt) {
                  deferred = $q.defer();
                  deferred.resolve('');
                  return deferred.promise;
              }

              function setOrganisation(addressDetails, dpUseType) {
                  deferred = $q.defer();
                  deferred.resolve('');
                  return deferred.promise;
              }

              function isUndefinedOrNull(obj) {
                  if (obj !== null && angular.isDefined(obj)) {
                      return obj;
                  }
                  else {
                      return "";
                  }
              }

              function UpdateDeliverypoint(positionedDeliveryPointList) {
                return ;
              }

              function deliveryPointTypes(positionedDeliveryPointList) {
                return ;
              }

              


              return {
                  initialize: initialize,
                  resultSet: resultSet,
                  openModalPopup: openModalPopup,
                  closeModalPopup: closeModalPopup,
                  getPostalAddress: getPostalAddress,
                  bindAddressDetails: bindAddressDetails,
                  setOrganisation: setOrganisation,
                  isUndefinedOrNull: isUndefinedOrNull,
                  UpdateDeliverypoint: UpdateDeliverypoint,
                  deliveryPointTypes: deliveryPointTypes
              };
          });
        });

        module(function($provide){
          $provide.factory('popUpSettingService', function(){
              function deliveryPoint() {
                
              }
              return {
                deliveryPoint: deliveryPoint
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
            $provide.value('$stateParams', stateParamsMockingData);
         
        });


        //get Instance of controller with inject properties
        inject(function (_$controller_,_$rootScope_,_mapToolbarService_,_$mdDialog_,_popUpSettingService_,_deliveryPointAPIService_,_$filter_,_mapFactory_,_coordinatesService_,_guidService_,_$state_,_$stateParams_,_deliveryPointService_) {
            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            mapToolbarService = _mapToolbarService_;
            $mdDialog = _$mdDialog_;
            popUpSettingService = _popUpSettingService_;
            deliveryPointAPIService = _deliveryPointAPIService_;
            $filter = _$filter_;
            mapFactory = _mapFactory_;
            coordinatesService = _coordinatesService_;
            guidService = _guidService_;
            $state = _$state_;
            $stateParams = _$stateParams_;
            deliveryPointService = _deliveryPointService_;


            vm = _$controller_('DeliveryPointController', {
                $scope : $scope,
                mapToolbarService : mapToolbarService,
                $mdDialog : $mdDialog,
                popUpSettingService : popUpSettingService,
                deliveryPointAPIService : deliveryPointAPIService,
                $filter : $filter,
                mapFactory : mapFactory,
                coordinatesService : coordinatesService,
                guidService : guidService,
                $state : $state,
                $stateParams : $stateParams,
                deliveryPointService : deliveryPointService
            })

        });

    });

    it('should be set isError `false`', function() {
        expect(vm.isError).toBe(false);
    });

    it('should be set isDisable `false`', function() {
        expect(vm.isDisable).toBe(false);
    });

    it('should be set positionedSaveDeliveryPointList `array`', function() {
        expect(vm.positionedSaveDeliveryPointList).toEqual([ ]);
    });

    it('should be set defaultNYBValue', function() {
        expect(vm.defaultNYBValue).toBeDefined();
        expect(vm.defaultNYBValue).toBe("00000000-0000-0000-0000-000000000000");
    });

    it('should be set errorMessageDisplay is `false`', function() {
        expect(vm.errorMessageDisplay).toBe(false);
    });

    it('should be set selectedItem `null`', function() {
        expect(vm.selectedItem).toBe(null);
    });

    it('should be set positionedCoOrdinates `[]`', function() {
        expect(vm.positionedCoOrdinates).toEqual([]);
    });

    it('should be set alias `null`', function() {
        expect(vm.alias).toBe(null);
    });

    it('should be set display `false`', function() {
        expect(vm.display).toBe(false);
    });

    it('should be set disable `true`', function() {
        expect(vm.disable).toBe(true);
    });

    it('should be set items `array`', function() {
        expect(vm.items).toEqual([]);
    });

    it('should be hide stateParams value', function() {

        expect(vm.hide).toBeDefined();
        expect(vm.hide).toBe(true);
    });

    xit('should be auto initialize method', function() {
        spyOn(vm,'initialize');
        vm.initialize();
        expect(vm.initialize).toHaveBeenCalled();
    });

    xit('should be return promise responce when initialize ', function() {

        vm.initialize();

        deliveryPointService.initialize().then(function (response) {
            expect(vm.deliveryPointTypes).toBe(response.deliveryPointTypes);
            expect(vm.dpUseType).toBe(response.dpUseType);
        });

        $rootScope.$apply();
    });

    it('should be return isError and isDisable `false` when called Ok method', function() {
        vm.Ok();
        expect(vm.isError).toBe(false);
        expect(vm.isDisable).toBe(false);
    });

    it('should be return comma separated value', function() {
        expect(vm.getCommaSeparatedVale('value1', 'value2')).toBeDefined();
        expect(vm.getCommaSeparatedVale('value1', 'value2')).toEqual('value1, value2');
    });

    it('should be return first value with no comma separated', function() {
        expect(vm.getCommaSeparatedVale('value1', '')).toBeDefined();
        expect(vm.getCommaSeparatedVale('value1', '')).toEqual('value1');
    });

    it('should be return second value with no comma separated', function() {
        expect(vm.getCommaSeparatedVale('', 'value2')).toBeDefined();
        expect(vm.getCommaSeparatedVale('', 'value2')).toEqual('value2');
    });

    it('should be return a object addAlias ', function() {
        vm.addAlias();

        expect(JSON.stringify(vm.items)).toBe(JSON.stringify([{ Preferred: false, DPAlias: null }]));
        expect(vm.alias).toBeDefined();
        expect(vm.alias).toBe('');
    });

    it('should be remove alias', function() {
        vm.items = [1,2,3,4,5];
        vm.removeAlias();
        var lastItem = vm.items.length - 1;
        vm.items.splice(lastItem);
        expect(lastItem).toBe(3);
    });


});

