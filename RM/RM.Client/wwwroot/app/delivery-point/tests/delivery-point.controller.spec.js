'use strict';
describe('Delivery Point: Controller', function () {    
    
    var $scope;
    var vm;
    var $q; 
    var $mdDialog;
    var mapToolbarService;    
    var popUpSettingService;
    var deliveryPointAPIService;    
    var mapFactory;
    var coordinatesService;    
    var stateMockData;
    var stateParamsMockingData;
    var mapService;
    var guidService;
    var $state;    
    var $stateParams;
    var deliveryPointService           
    var $rootScope;    
    var CommonConstants;
    var $translate;
    var GlobalSettings;
    
    var MockGlobalSettings = {single:'Single',numberInName:'Number in Name',subBuilding:'Sub building',range:'Range',defaultRangeOption:"Odds"};
    stateParamsMockingData = {
      hide:true,
      positionedThirdPartyDeliveryPointList: {},
      positionedDeliveryPointList: 'firstPointList',
      deliveryPointList: {udprn: "484575", locality: "232 232 adf adf adf Abbey Road BN11 3RW", addressGuid: "00000000-0000-0000-0000-000000000000", id: "09c2b40c-684a-405d-86db-6aa054c29453", xCoordinate: null}
    };
   
    beforeEach(function () {
        module('deliveryPoint'); 
        module(function ($provide) {
            $provide.value('$translate',{});
            $provide.value('GlobalSettings',MockGlobalSettings);
            $provide.value('CommonConstants',{DpUseType:{Residential:'Residential',Organisation:'Organisation'}});            
            $provide.value('$state', { go: function(state, args){}});
            $provide.value('$stateParams', stateParamsMockingData);  
            $provide.value('stringFormatService', {});          
            $provide.factory('$mdDialog', function() {
                return {hide:jasmine.createSpy()};
            });

            $provide.factory('mapToolbarService', function ($q) {
              function getShapeForButton(button) {}
                return {
                  getShapeForButton: getShapeForButton
                }
            });

            $provide.factory('mapService', function () {
                function clearDrawingLayer(param){}
                return {
                    clearDrawingLayer : clearDrawingLayer 
                }
            });

            $provide.factory('deliveryPointService', function($q){
                function initialize() {
                    var deferred = $q.defer();                         
                    return deferred.promise;
                }     
                function getSubBuildingTypes(){
                    var deferred = $q.defer();                         
                    return deferred.promise;
                }
                function getRangeOptions(){
                    var deferred = $q.defer();                         
                    return deferred.promise;
                }
                function resultSet() {}                    
                function openModalPopup() {}              
                function closeModalPopup() {}
                function getPostalAddress(postcode) {
                  var getPostalAddressMockData = {display:true,selectedValue:true,postalAddressData:{nybAddressDetails:'testDetails',routeDetails:''}};
                    var deferred = $q.defer();
                    deferred.resolve(getPostalAddressMockData);
                    return deferred.promise;
                }

                function bindAddressDetails(notyetBuilt) {
                    var deferred = $q.defer();
                    return deferred.promise;
                }

                function setOrganisation(addressDetails, dpUseType) {
                    var deferred = $q.defer();
                    return deferred.promise;
                }

                function deliveryPointTypes() {
                    var deferred = $q.defer();                    
                    return deferred.promise;
                }
                 function deliveryPointUseType() {
                    var deferred = $q.defer();                    
                    return deferred.promise;
                }

                function UpdateDeliverypoint(argument) {
                    var deferred = $q.defer(); 
                    return deferred.promise;
                }

                function createDeliveryPointsRange(postalAddressDetails){
                    var deferred = $q.defer();
                    return deferred.promise;   
                }
                
                return {
                    initialize: initialize,
                    resultSet: resultSet,
                    openModalPopup: openModalPopup,
                    closeModalPopup: closeModalPopup,
                    getPostalAddress: getPostalAddress,
                    bindAddressDetails: bindAddressDetails,
                    setOrganisation: setOrganisation,  
                    deliveryPointTypes: deliveryPointTypes, 
                    deliveryPointUseType: deliveryPointUseType,
                    UpdateDeliverypoint: UpdateDeliverypoint,
                    getSubBuildingTypes: getSubBuildingTypes,
                    getRangeOptions: getRangeOptions,
                    createDeliveryPointsRange: createDeliveryPointsRange
                };
            });

            $provide.factory('deliveryPointAPIService', function(){
                function GetAddressLocation(){}
                return {
                    GetAddressLocation: GetAddressLocation
                }
            });

            $provide.factory('popUpSettingService', function(){
                function deliveryPoint() {}
                return {
                  deliveryPoint: deliveryPoint
                }
            });

            $provide.factory('mapFactory', function () {
              var map = {
                getView: function(){
                  return {
                      calculateExtent: function(){}                 
                  }
                },
                getSize: function(){}               
             };

            function initialiseMap() {}               
            function getMap() { return map; }           
            function setAccessLink() { }   
            function locateDeliveryPoint(){}
            return {
              initialiseMap: initialiseMap,
                getMap: getMap,             
                setAccessLink : setAccessLink,
                locateDeliveryPoint: locateDeliveryPoint          
              }
            });            
        });        

        inject(function (
            _$controller_,            
            _mapToolbarService_,
            _$mdDialog_,
            _popUpSettingService_,
            _deliveryPointAPIService_,
            _mapFactory_,
            _$state_,
            _$stateParams_,
            _deliveryPointService_,
            _$q_,
            _mapService_,
            _CommonConstants_,
            _$rootScope_,
            _$translate_,
            _GlobalSettings_) {
            
            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            mapToolbarService = _mapToolbarService_;
            $mdDialog = _$mdDialog_;
            popUpSettingService = _popUpSettingService_;
            deliveryPointAPIService = _deliveryPointAPIService_;
            mapFactory = _mapFactory_;
            $state = _$state_;
            $stateParams = _$stateParams_;
            deliveryPointService = _deliveryPointService_;
            $q = _$q_;
            CommonConstants = _CommonConstants_;
            $translate = _$translate_
            GlobalSettings = _GlobalSettings_;

            vm = _$controller_('DeliveryPointController', {
                $scope : $scope,
                mapToolbarService : mapToolbarService,
                $mdDialog : $mdDialog,
                popUpSettingService : popUpSettingService,
                deliveryPointAPIService: deliveryPointAPIService,
                mapFactory : mapFactory,
                $state : $state,
                $stateParams : $stateParams,
                deliveryPointService : deliveryPointService,
                mapService: _mapService_,
                CommonConstants: CommonConstants,
                $translate: $translate,
                GlobalSettings: GlobalSettings
            });

            spyOn($scope, '$emit').and.callThrough();
            spyOn($scope, '$on').and.callThrough();
            $scope.$emit.and.stub();
        });
    });

    it('should set positionedThirdPartyDeliveryPointList value as `{}`', function() {
        expect(vm.positionedThirdPartyDeliveryPointList).toEqual({});
    });

    it('should set positionedDeliveryPointList value as `{}`', function() {
        expect(vm.positionedDeliveryPointList).toEqual('firstPointList');
    });

    it('should set deliveryPointList value as `object`', function() {
        expect(vm.deliveryPointList).toEqual({udprn: "484575", locality: "232 232 adf adf adf Abbey Road BN11 3RW", addressGuid: "00000000-0000-0000-0000-000000000000", id: "09c2b40c-684a-405d-86db-6aa054c29453", xCoordinate: null});
    });
    
    it('should set positionedThirdPartyDeliveryPoint value as `array`', function() {
        expect(vm.positionedThirdPartyDeliveryPoint).toEqual([ ]);
    });
    
    it('should set isError value as `false`', function() {
        expect(vm.isError).toBe(false);
    });

    it('should set `isDisable` value as `false`', function() {
        expect(vm.isDisable).toBe(false);
    });

    it('should set `positionedSaveDeliveryPointList` value as `array`', function() {
        expect(vm.positionedSaveDeliveryPointList).toEqual([ ]);
    });

    it('should set defaultNYBValue', function() {
        expect(vm.defaultNYBValue).toBe("00000000-0000-0000-0000-000000000000");
    });

    it('should set `errorMessageDisplay` value as `false`', function() {
        expect(vm.errorMessageDisplay).toBe(false);
    });

    it('should set `selectedItem` value as `null`', function() {
        expect(vm.selectedItem).toBe(null);
    });

    it('should set `positionedCoOrdinates` value as `[]`', function() {
        expect(vm.positionedCoOrdinates).toEqual([]);
    });

    it('should set default alias value as `null`', function() {
        expect(vm.alias).toBe(null);
    });

    it('should set display value as `false`', function() {
        expect(vm.display).toBe(false);
    });

    it('should set disable value as `true`', function() {
        expect(vm.disable).toBe(true);
    });   
    
    it('should set `postalAddressAliases` value as `Odds`', function() {
        expect(vm.rangeOptionsSelected).toEqual('Odds');
    });

    it('should set `rangeOptionsSelected` value as `Odds`', function() {
        expect(vm.rangeOptionsSelected).toEqual('Odds');
    });

    it('should set `dpIsChecked` value as `false`', function() {
        expect(vm.dpIsChecked).toBe(false);
    }); 

    it('should set `selectedType` value as `null`', function() {
        expect(vm.selectedType).toBe(null);
    }); 

    it('should set `single` value as `Single`', function() {
        expect(vm.single).toEqual('Single');
    });

    it('should set `range` value as `Range`', function() {
        expect(vm.range).toEqual('Range');
    });

    it('should set `subBuilding` value as `Sub building`', function() {
        expect(vm.subBuilding).toEqual('Sub building');
    });

    it('should set `numberInName` value as `Number in Name`', function() {
        expect(vm.numberInName).toEqual('Number in Name');
    }); 

    it('should set `displayRangeFromMessage` value as `false`', function() {
        expect(vm.displayRangeFromMessage).toBe(false);
    });   

    it('should set `displayRangeToMessage` value as `false`', function() {
        expect(vm.displayRangeToMessage).toBe(false);
    });

    it('should set `maintainState` value as `false`', function() {
        expect(vm.maintainState).toBe(false);
    });

    it('should set `defaultDeliveryType` value as `single`', function() {
        expect(vm.defaultDeliveryType).toEqual('Single');
    });

    it('should promise to return a success response once initialize method is called', function() {            
        var deferred = $q.defer();             
        var response = {DeliveryPointTypes:'single',DpUseType:'circle',SubBuildingTypes:'GigaPlex',RangeOptions:'548759'};

        spyOn(deliveryPointService, 'initialize').and.returnValue(deferred.promise);        

        vm.initialize();    

        deferred.resolve(response);         

        $rootScope.$apply();

        expect(deliveryPointService.initialize).toHaveBeenCalled();        
        expect(vm.deliveryPointTypes).toEqual('single');
        expect(vm.dpUseType).toEqual('circle');
        expect(vm.subBuildingTypes).toEqual('GigaPlex');
        expect(vm.rangeOptions).toEqual('548759');
    });

    it('should be open popup model', function() {
        spyOn(deliveryPointService,'openModalPopup');
        spyOn(popUpSettingService,'deliveryPoint');
        vm.deliveryPoint();        

        expect(deliveryPointService.openModalPopup).toHaveBeenCalled();
        expect(popUpSettingService.deliveryPoint).toHaveBeenCalled();        
        expect($scope.$emit).toHaveBeenCalledWith('dialogOpen','deliveryPoint');

    });

    it('should close dialog window', function() {    
        spyOn(deliveryPointService,'closeModalPopup');
        vm.closeWindow();

        expect(vm.hide).toBe(false);
        expect(vm.display).toBe(false);
        expect(vm.searchText).toEqual("");
        expect(vm.mailvol).toEqual("");
        expect(vm.multiocc).toEqual("");
        expect(deliveryPointService.closeModalPopup).toHaveBeenCalled();
    });

    it('should promise to return a success response once resultSet method is called', function() {
        var deferred = $q.defer();
        var query = {};
        var response = {deliveryPointTypes:'single',dpUseType:'circle'};
        spyOn(deliveryPointService, 'resultSet').and.returnValue(deferred.promise);
        
        vm.resultSet(query);

        deferred.resolve(response);
        $rootScope.$apply();
        
        expect(deliveryPointService.resultSet).toHaveBeenCalledWith({});
        expect(response).toEqual({deliveryPointTypes:'single',dpUseType:'circle'});        
    });

    it('should promise to return a success response once OnChangeItem method is called', function() {
        var deferred = $q.defer();
        var selectedItem = 'mapSearch';
        var response = {display:true,selectedValue:true,postalAddressData:{nybAddressDetails:'testDetails',routeDetails:''}};

        spyOn(deliveryPointService, 'getPostalAddress').and.returnValue(deferred.promise);

        vm.OnChangeItem(selectedItem);

        expect(vm.routeId).toBe("");
        expect(vm.notyetBuilt).toBe("");
        expect(vm.searchText).toBe(selectedItem);
        expect(vm.results).toEqual({});

        deferred.resolve(response);
        $rootScope.$apply();

        expect(deliveryPointService.getPostalAddress).toHaveBeenCalledWith('mapSearch');
        expect(vm.addressDetails).toEqual(response.postalAddressData);
        expect(vm.nybAddressDetails).toBe(response.postalAddressData.nybAddressDetails);
        expect(vm.routeDetails).toBe(response.postalAddressData.routeDetails);
        expect(vm.display).toBe(response.display);
        expect(vm.selectedValue).toBe(response.selectedValue);        
    });

    it('should promise to return a success response once bindAddressDetails method is called', function() {
        var deferred = $q.defer();
        var response = {addressDetails:{id:'1234',udprn:'testudprn',buildingNumber:1,buildingName:'SEZ',subBuildingName:'GigaPlex',organisationName:'CG',departmentName:'DCX'}}

        spyOn(deliveryPointService, 'bindAddressDetails').and.returnValue(deferred.promise);

        vm.bindAddressDetails();

        deferred.resolve(response);
        $rootScope.$apply();
       
        expect(deliveryPointService.bindAddressDetails).toHaveBeenCalled();
        expect(vm.addressDetails).toBe(response);
    });

    it('should be defined address when default NYB value match', function() {
      vm.bindAddressDetails();

      vm.notyetBuilt = true;
      vm.addressDetails = {id:"00000000-0000-0000-0000-000000000000",udprn:"",buildingNumber:"",buildingName:"",subBuildingName:"",organisationName:"",departmentName:""};

      expect(vm.addressDetails.id).toBe(vm.defaultNYBValue);
      expect(vm.addressDetails.udprn).toBe("");
      expect(vm.addressDetails.buildingNumber).toBe("");
      expect(vm.addressDetails.buildingName).toBe("");
      expect(vm.addressDetails.subBuildingName).toBe("");
      expect(vm.addressDetails.organisationName).toBe("");
      expect(vm.addressDetails.departmentName).toBe("");        

    });

    it('should promise to return a success response once setOrganisation method is called', function() {
        vm.selectedType = 'point';
        vm.single = 'point';
        vm.addressDetails = true;

        var deferred = $q.defer(); 
        var response = {dpUse:true,selectedDPUse:true};
        spyOn(deliveryPointService, 'setOrganisation').and.returnValue(deferred.promise);
        
        vm.setOrganisation();
        
        deferred.resolve(response);
        $rootScope.$apply();

        expect(deliveryPointService.setOrganisation).toHaveBeenCalled();
        expect(vm.dpUse).toBe(response.dpUse);
        expect(vm.selectedDPUse).toBe(response.selectedDPUse);
    });

    it('should return dpUse and selectedDPUse object response once setOrganisation method is called', function() {        
        vm.dpUseType = 'selectedDPUse';

        vm.setOrganisation();
        
        expect(vm.dpUse).toEqual('selectedDPUse');
        expect(vm.selectedDPUse).toBe("");
    });

    it('should be set toggle', function() {
        vm.toggle(true);
        expect(vm.selectedItem).toBe(true); 
        expect(vm.dpIsChecked).toBe(false);
        
    });    

    it('should be call update UpdateDeliverypoint once savePositionedDeliveryPoint method called ', function() {
        spyOn(deliveryPointService,'UpdateDeliverypoint').and.callThrough();

        vm.savePositionedDeliveryPoint();

        expect(vm.isOnceClicked).toBe(true);        
        expect(vm.positionedDeliveryPointList).toBe(null);
        expect(deliveryPointService.UpdateDeliverypoint).toHaveBeenCalled();
        expect(deliveryPointService.UpdateDeliverypoint).toHaveBeenCalledWith('firstPointList');
    });       

    it('should promise to return a success response once getAddressLocation method is called', function() {
        var deferred = $q.defer();
        var udprn = '369859';
        var response = {data:{addressDetails:{id:'1234',udprn:'testudprn',buildingNumber:1,buildingName:'SEZ',subBuildingName:'GigaPlex',organisationName:'CG',departmentName:'DCX'}}};

        spyOn(deliveryPointAPIService, 'GetAddressLocation').and.returnValue(deferred.promise);

        vm.getAddressLocation(udprn);

        deferred.resolve(response);
        $rootScope.$apply();
       
        expect(deliveryPointAPIService.GetAddressLocation).toHaveBeenCalledWith(udprn);
        expect(vm.addressLocationData).toEqual({addressDetails: { id: '1234', udprn: 'testudprn', buildingNumber: 1, buildingName: 'SEZ', subBuildingName: 'GigaPlex', organisationName: 'CG', departmentName: 'DCX'}});
    });

    it('should add an item when `addAlias` method called ', function() {
        vm.alias = true;

        vm.addAlias();
        expect(vm.postalAddressAliases).toEqual([{ PreferenceOrderIndex: 0, AliasName: true }]);
        expect(vm.alias).toEqual("");
        expect(vm.isAliasDisabled).toBe(true);
    });

    it('should remove an item when removeAlias method called', function() {
        vm.items = [1,2,3,4,5];
        vm.removeAlias();
        var lastItem = vm.items.length - 1;
        vm.items.splice(lastItem);
        expect(lastItem).toBe(4);
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

    it('should be return isError and isDisable `false` when called Ok method', function() {
        vm.Ok();
        expect(vm.isError).toBe(false);
        expect(vm.isDisable).toBe(false);
    });

    it('should set range validation when `rangeFrom` greaterthen is `rangeTo` and `rangetype` equal to `RangeFrom`', function() {        
        vm.setRangeValidation(32, 30, 'RangeFrom');

        expect(vm.displayRangeFromMessage).toBe(true);
        expect(vm.displayRangeToMessage).toBe(false);
    });

    it('should set range validation when `rangeFrom` greaterthen is `rangeTo` and `rangetype` not equal to `RangeFrom`', function() {        
        vm.setRangeValidation(32, 30, 'RangeTo');

        expect(vm.displayRangeFromMessage).toBe(false);
        expect(vm.displayRangeToMessage).toBe(true);
    });

    it('should set range validation when `rangeFrom` lessthen is `rangeTo` and `rangetype` is equal to `RangeFrom`', function() {        
        vm.setRangeValidation(30, 32, 'RangeFrom');

        expect(vm.displayRangeFromMessage).toBe(false);
        expect(vm.displayRangeToMessage).toBe(false);
    });

    it('should promise to return a success response once createDeliveryPointsRange method is called', function() {
        var postalAddressDetails = {addressDetails:{id:'1234',udprn:'testudprn',buildingNumber:1,buildingName:'SEZ',subBuildingName:'GigaPlex',organisationName:'CG',departmentName:'DCX'}};
        var deferred = $q.defer();

        spyOn(deliveryPointService,'createDeliveryPointsRange').and.returnValue(deferred.promise);
        spyOn(vm,'closeWindow').and.callThrough();
        spyOn(deliveryPointService,'closeModalPopup');

        vm.createDeliveryPointsRange(postalAddressDetails);
        deferred.resolve();
        $rootScope.$apply();
        
        expect(vm.closeWindow).toHaveBeenCalled();
        expect(vm.hide).toEqual(false);
        expect(vm.display).toEqual(false);
        expect(vm.searchText).toEqual("");
        expect(vm.mailvol).toEqual("");
        expect(vm.multiocc).toEqual("");
        expect(vm.rangeOptionsSelected).toEqual(GlobalSettings.defaultRangeOption);
        expect(deliveryPointService.closeModalPopup).toHaveBeenCalled();
        expect(vm.results).toEqual({});       
        expect(vm.postalAddressAliases).toEqual([]);
        expect(vm.alias).toEqual("");
        expect(vm.maintainState).toEqual(false);        
    });
});

