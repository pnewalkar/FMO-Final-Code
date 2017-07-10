describe('deliveryPoint: Service', function () {

    var referencedataApiService;
    var $filter;
    var $q;
    var deliveryPointAPIService;
    var guidService;
    var $mdDialog;
    var $state;
    var mapFactory;
    var $filter;
    var $rootScope;

    /////////////Mock data//////////////////////
    var getPostalAddressMockData = {"postcodeType":"S","organisationName":null,"departmentName":null,"buildingName":"Abinger House","buildingNumber":null,"subBuildingName":null,"thoroughfare":"Abbey Road","dependentThoroughfare":null,"dependentLocality":null,"doubleDependentLocality":null,"postTown":"Worthing","postcode":"BN11 3RW","deliveryPointSuffix":"1B","smallUserOrganisationIndicator":null,"udprn":2342864,"amuApproved":null,"poBoxNumber":"      ","id":"b59b21f6-c4eb-4b76-9451-0510cfa17eff","postCodeGUID":"9cfff01f-d539-4957-9f69-2638e7a30e57","addressType_GUID":"a21f3e46-2d0d-4989-a5d5-872d23b479a2","addressStatus_GUID":"ee479380-c4f7-4fa3-96b2-dd54a7091baa","isValidData":false,"inValidRemarks":null,"date":null,"time":null,"amendmentType":null,"amendmentDesc":null,"fileName":null,"nybAddressDetails":[{"value":"null","displayText":"Not Shown"}],"routeDetails":[{"value":"a5065aec-a4ba-4f94-8c0a-0045d1131966","displayText":"1415 BROADWATER ST EAST       "}]};
	  var bindAddressDetailsMockData = {"address":"mumbai"};
    var UpdateDeliverypointMockData = {"xCoordinate":512073.18010136025,"yCoordinate":107209.5124445403,"id":"4124f93d-e679-498e-b454-c82768a4732e"};
    var resultSetMockData = {"postcodeType":"S","organisationName":null,"departmentName":null,"buildingName":"Abinger House","buildingNumber":null,"subBuildingName":null,"thoroughfare":"Abbey Road","dependentThoroughfare":null,"dependentLocality":null,"doubleDependentLocality":null,"postTown":"Worthing","postcode":"BN11 3RW","deliveryPointSuffix":"1B","smallUserOrganisationIndicator":null,"udprn":2342864,"amuApproved":null,"poBoxNumber":"      ","id":"b59b21f6-c4eb-4b76-9451-0510cfa17eff","postCodeGUID":"9cfff01f-d539-4957-9f69-2638e7a30e57","addressType_GUID":"a21f3e46-2d0d-4989-a5d5-872d23b479a2","addressStatus_GUID":"ee479380-c4f7-4fa3-96b2-dd54a7091baa","isValidData":false,"inValidRemarks":null,"date":null,"time":null,"amendmentType":null,"amendmentDesc":null,"fileName":null,"nybAddressDetails":[{"value":"null","displayText":"Not Shown"}],"routeDetails":[{"value":"a5065aec-a4ba-4f94-8c0a-0045d1131966","displayText":"1415 BROADWATER ST EAST       "}]};

    ///////////////GLOBALSETTINGS///////////////////////
    //Mocking Value of Global Setting
    var MockGlobalSettings = {
      apiUrl: 'http://localhost:34583/api',
      env: 'localhost', // Here set the current environment
      indexUrl: '',
        getRouteLogSelectionType: "/RouteLog/RouteLogsSelectionType",
        getRouteLogStatus: "/RouteLog/RouteLogsStatus",
        //getDeliveryRouteScenario: "/RouteLog/FetchDeliveryScenario?operationStateID={0}&deliveryUnitID={1}&fields=ScenarioName,ID",
        //getDeliveryRouteScenario: "/UnitManager/scenario/{0}/{1}/ScenarioName,ID",
        getDeliveryRouteScenario: "/UnitManager/scenario/{0}/{1}",
        //getDeliveryRoutes: "/RouteLog/FetchDeliveryRoute?operationStateID={0}&deliveryScenarioID={1}&fields=RouteNameNumber,RouteName,ID",
        getDeliveryRoutes:"/DeliveryRouteManager/deliveryroute/{0}/{1}",
        getRouteDetailsByGUID: "/DeliveryRouteManager/deliveryroute/routedetails/",
        getReferenceData: "./reference-data/ReferenceData.js"
    };

    ///////////////////$stateProvidder mockdata///////////////
    // set default stateParams
    stateMockData = {selectedUnit:{"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","$$mdSelectId":1,"$$hashKey":"object:114"},
                      deliveryPoint:"",
                        go: function(){}
                    };
    stateParamsMockData = {"accessLinkFeature":"adsdf"};


    //Load our module and inject with dependencies provider
    beforeEach(function () {

        module('deliveryPoint'); //load module

        module(function($provide){
            $provide.factory('guidService',function(){
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

        //Inejct with mockdata routeLogAPIService with attached all function
        module(function ($provide) {
            $provide.factory('deliveryPointAPIService', function ($q) {
                
                function GetDeliveryPointsResultSet(searchText) {
                  deferred = $q.defer();
                  deferred.resolve(resultSetMockData);
                  return deferred.promise;
                }

                function GetAddressByPostCode(selectedItem) {
                  deferred = $q.defer();
                  deferred.resolve(getPostalAddressMockData);
                  return deferred.promise;
                }

                function GetAddressLocation(udprn) {
                  deferred = $q.defer();
                  deferred.resolve(stateParamsMockData);
                  return deferred.promise;
                }

                function GetPostalAddressByGuid(addressGuid) {
                  deferred = $q.defer();
                  deferred.resolve(bindAddressDetailsMockData);
                  return deferred.promise;
                }

                function CreateDeliveryPoint(addDeliveryPointDTO) {
                  deferred = $q.defer();
                  deferred.resolve();
                  return deferred.promise;
                }

                function UpdateDeliverypoint(deliveryPointModelDTO) {
                  deferred = $q.defer();
                  deferred.resolve(UpdateDeliverypointMockData);
                  return deferred.promise;
                }


                return {
                    GetDeliveryPointsResultSet: GetDeliveryPointsResultSet,
                    GetAddressByPostCode: GetAddressByPostCode,
                    GetAddressLocation: GetAddressLocation,
                    GetPostalAddressByGuid: GetPostalAddressByGuid,
                    CreateDeliveryPoint: CreateDeliveryPoint,
                    UpdateDeliverypoint: UpdateDeliverypoint
                };
            });
        });

        module(function ($provide) {
            $provide.factory('referencedataApiService', function ($q) {

                function getReferenceData() {
                    return $http.get("http://localhost:34583/api/reference-data/ReferenceData.js");
                }

                return {
                    getReferenceData: getReferenceData,
                };
            });
        });
        //inject with mockdata mapFactory
        module(function ($provide) {
            $provide.factory('mapFactory', function () {

              //Set up variable for mapFactory
              var map = {
                  getView: function(){
                    return {
                        calculateExtent: function(){}
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

        //Injec mockdaa CommonConstants with attached all constant value
        module(function ($provide) {
          $provide.value('$state', stateMockData);
          $provide.value('GlobalSettings', MockGlobalSettings);
       		MockMdHide = jasmine.createSpy(true);
          MockMdShow = jasmine.createSpy();
    		    $provide.factory('$mdDialog', function() {
    		        return {
                        hide: MockMdHide,
                        show: MockMdShow
                      };
    		    });
        });


        //get Instance of controller with inject properties
        inject(function (_$rootScope_,_deliveryPointService_,_referencedataApiService_,_$filter_,_$q_,_deliveryPointAPIService_,_guidService_,_$mdDialog_,_$state_,_mapFactory_,_$filter_) {
        	  $rootScope = _$rootScope_;
            scope = _$rootScope_.$new(); //Generate new scope
            deliveryPointService = _deliveryPointService_;
            deliveryPointAPIService = _deliveryPointAPIService_;
            referencedataApiService = _referencedataApiService_;
            guidService = _guidService_;
            $filter = _$filter_;
            $q = _$q_;
            $mdDialog = _$mdDialog_;
            $state = _$state_;
            mapFactory = _mapFactory_;
            $filter = _$filter_;
        });

    });
    
    it('should be return promise response when resultSet length is `greater then 3` method call', function() {

        var query = [{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"}];

        ///Call method if length > 3 then api promise return call else with promise return
        deferred = $q.defer();
        result = [];
        if (query.length >= 3) {
            deliveryPointAPIService.GetDeliveryPointsResultSet(query).then(function (response) {
                deferred.resolve(response);
                deferred.promise;

                expect(response).toBeDefined();
                expect(response).toBe(resultSetMockData);
            });
        }

        deliveryPointService.resultSet(query);
        
        $rootScope.$apply();
    });

    it('should be return promise response when resultSet length is `less then 3` method call', function() {

        var query = [{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"}];

        result = [];
        if (query.length <= 3) {
            deliveryPointAPIService.GetDeliveryPointsResultSet(query).then(function (response) {
                expect(response).toBeDefined();
                expect(response).toBe(resultSetMockData);
            });
        }

        deliveryPointService.resultSet(query);
        
        $rootScope.$apply();
    });

    it('should be dialog show when popup open', function() {
        deliveryPointService.openModalPopup();
        expect($mdDialog.show).toHaveBeenCalled();
    });

    it('should be dialog model close when popup close', function() {
        deliveryPointService.closeModalPopup();
        expect($mdDialog.hide).toHaveBeenCalled();
    });
 
    it('should be return promise response getPostalAddress', function() {
        var result = {"postalAddressData": null, "selectedValue": null, "display": null };
        var postcode = "Abbey Road,BN11 3RW";

        deliveryPointService.getPostalAddress(postcode);

        //this test has been done with asy call with done fun and should be expect data 
        deliveryPointAPIService.GetAddressByPostCode(postcode).then(function (response) {        
            expect(response).toBe(getPostalAddressMockData);
        });

        //task perform to asy cal so jasmine need to time interval so update to digest cycle
          scope.$digest(); //scope is digest life cycle
    });

    it('should be promise return response of bind address details ', function(done) {

        var notyetBuilt = '1234';
        deliveryPointAPIService.GetPostalAddressByGuid(notyetBuilt).then(function (response) {
              expect(response).toBe(bindAddressDetailsMockData);
              done();
        });

         //task perform to asy cal so jasmine need to time interval so update to digest cycle
          scope.$digest(); //scope is digest life cycle
    });

    it('should be return promise filter Organisation', function() {

        //Set of defer for promise object
        var deferred = $q.defer();
        var result = { "dpUse": null, "selectedDPUse": null };

        function filter(input,output) {
          return $filter('filter')(dpUseType.referenceDatas, {
                referenceDataValue: "Organisation"
            });
            
        }

        var addressDetails = {postcodeType: "S", organisationName: undefined, departmentName: undefined, buildingName: undefined, buildingNumber: undefined};
        var dpUseType = [{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"},{"id":"178edcad-9431-e711-83ec-28d244aef9ed","name":null,"value":"Residential","displayText":null,"description":"Residential"}];
        dpUseType.referenceDatas = dpUseType;
        var a = {"dpUse":[{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"}],"selectedDPUse":{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"}};
        //call to our fun
        deliveryPointService.setOrganisation(addressDetails,dpUseType);
        
        result.dpUse = filter(addressDetails,dpUseType);
        result.selectedDPUse = result.dpUse[0];
        deferred.resolve(result);
        deferred.promise;

        expect(result).toBeDefined();
        expect(JSON.stringify(result)).toBe(JSON.stringify({"dpUse":[{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"}],"selectedDPUse":{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"}}))


    });

    it('should be return promise filter Residential', function() {

        //Set of defer for promise object
        var deferred = $q.defer();
        var result = { "dpUse": null, "selectedDPUse": null };

        function filter(input,output) {
          return $filter('filter')(dpUseType.referenceDatas, {
                referenceDataValue: "Residential"
            });
            
        }

        var addressDetails = {postcodeType: "S", departmentName: undefined, buildingName: undefined, buildingNumber: undefined};
        var dpUseType = [{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Residential","displayText":null,"description":"Organisation"},{"id":"178edcad-9431-e711-83ec-28d244aef9ed","name":null,"value":"Residential","displayText":null,"description":"Residential"}];
        dpUseType.referenceDatas = dpUseType;
        
        //call to our fun
        deliveryPointService.setOrganisation(addressDetails,dpUseType);
        

        result.dpUse = filter(addressDetails,dpUseType);
        result.selectedDPUse = result.dpUse[0];
        deferred.resolve(result);
        deferred.promise;

        expect(result).toBeDefined();
        expect(JSON.stringify(result)).toBe(JSON.stringify({"dpUse":[{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Residential","displayText":null,"description":"Organisation"}],"selectedDPUse":{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Residential","displayText":null,"description":"Organisation"}}));

    });

    it('should be return isUndefinedOrNull', function() {
        
        expect(deliveryPointService.isUndefinedOrNull(JSON.stringify({'value':true}))).toBe(JSON.stringify({'value':true}));
        expect(deliveryPointService.isUndefinedOrNull()).toBe('');
    });

    it('should be promise UpdateDeliverypoint', function() {
        
        var positionedDeliveryPointList = [{"udprn":"","locality":"45 test001    Abbotts Close BN11 1JB","addressGuid":"00000000-0000-0000-0000-000000000000","id":"4124f93d-e679-498e-b454-c82768a4732e","xCoordinate":512073.18010136025,"yCoordinate":107209.5124445403,"latitude":50.853553614182566,"longitude":-0.40920540821937557,"rowversion":"AAAAAAAFCSk=","$$hashKey":"object:309"}];

        //Cal to api for UpdateDeliveryPoint
        deliveryPointAPIService.UpdateDeliverypoint(positionedDeliveryPointList[0]).then(function (result) {

            //Once recive response call to mapfactroy setAccessLink setDeliveryPoint setguid
            spyOn(mapFactory,'setAccessLink');
            spyOn(mapFactory,'setDeliveryPoint');
            spyOn(guidService,'setGuid');
            //spyOn($state,'go'); 

            mapFactory.setAccessLink();
            mapFactory.setDeliveryPoint(result.xCoordinate, result.yCoordinate);
            guidService.setGuid(result.id);


            expect(JSON.stringify(result)).toEqual(JSON.stringify(UpdateDeliverypointMockData));
            expect(mapFactory.setAccessLink).toHaveBeenCalled();
            expect(mapFactory.setDeliveryPoint).toHaveBeenCalled();
            expect(mapFactory.setDeliveryPoint).toHaveBeenCalledWith(result.xCoordinate, result.yCoordinate);
            //$state.expectTransitionTo('deliveryPoint');
        });

        scope.$digest();

        //cal to updateDelivery
        deliveryPointService.UpdateDeliverypoint(positionedDeliveryPointList);
    });

});

