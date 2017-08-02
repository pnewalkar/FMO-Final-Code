'use strict';
describe('Delivery Point: Controller', function () {    
    var $rootScope;    
    var $scope;
    var vm;
    var $q; 
    var $mdDialog;
    var mapToolbarService;    
    var popUpSettingService;
    var deliveryPointAPIService;    
    var mapFactory;
    var coordinatesService;
    var guidService;
    var $state;
    var $stateParams;
    var deliveryPointService           
    var stateMockData;
    var stateParamsMockingData;
    var mapService;
    var CommonConstants;
    

    stateMockData = {"selectedUnit":{"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","$$mdSelectId":1,"$$hashKey":"object:114"}};
    stateParamsMockingData = {
      hide:true,
      positionedThirdPartyDeliveryPointList: 'thirdparty',
      positionedDeliveryPointList: 'firstPointList',
      deliveryPointList: '2ndpointlist'
    };
   
    beforeEach(function () {
        module('deliveryPoint'); 
        module(function ($provide) {
            $provide.value('CommonConstants',{});
            $provide.value('$state', stateMockData);
            $provide.value('$stateParams', stateParamsMockingData);  
            $provide.value('GlobalSettings', {});
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
                function initialize() {}                
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
                    UpdateDeliverypoint: UpdateDeliverypoint           
                };
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
            return {
              initialiseMap: initialiseMap,
                getMap: getMap,             
                setAccessLink : setAccessLink             
              }
            });            
        });        

        inject(function (
            _$controller_,
            _$rootScope_,
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
            _CommonConstants_) {
            
            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            mapToolbarService = _mapToolbarService_;
            $mdDialog = _$mdDialog_;
            popUpSettingService = _popUpSettingService_;
            mapFactory = _mapFactory_;
            $state = _$state_;
            $stateParams = _$stateParams_;
            deliveryPointService = _deliveryPointService_;
            $q = _$q_;
            CommonConstants = _CommonConstants_;

            vm = _$controller_('DeliveryPointController', {
                $scope : $scope,
                mapToolbarService : mapToolbarService,
                $mdDialog : $mdDialog,
                popUpSettingService : popUpSettingService,
                mapFactory : mapFactory,
                $state : $state,
                $stateParams : $stateParams,
                deliveryPointService : deliveryPointService,
                mapService: _mapService_,
                CommonConstants: CommonConstants
            });

            spyOn($scope, '$emit').and.callThrough();
            spyOn($scope, '$on').and.callThrough();
            $scope.$emit.and.stub();
        });
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
        expect(vm.defaultNYBValue).toBeDefined();
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

    it('should be set items value as `array`', function() {
        expect(vm.items).toEqual([]);
    });

    it('should set `dpIsChecked` value as `false`', function() {
        expect(vm.dpIsChecked).toBe(false);
    });    

    it('should promise to return a success response once initialize method is called', function() {            
        var deferred = $q.defer();
        var response = {deliveryPointTypes:'single',dpUseType:'circle'};

        spyOn(deliveryPointService, 'deliveryPointTypes').and.returnValue(deferred.promise);        
        spyOn(deliveryPointService, 'deliveryPointUseType').and.returnValue(deferred.promise);

        vm.initialize();    

        deferred.resolve(response);    
        $scope.$digest();

        expect(deliveryPointService.deliveryPointTypes).toHaveBeenCalled();
        expect(deliveryPointService.deliveryPointUseType).toHaveBeenCalled();
        expect(response).toEqual({deliveryPointTypes:'single',dpUseType:'circle'});        
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
        $scope.$digest();
        
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
        $scope.$digest();

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
        $scope.$digest();
       
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
        var deferred = $q.defer(); 
        var response = {dpUse:true,selectedDPUse:true};
        spyOn(deliveryPointService, 'setOrganisation').and.returnValue(deferred.promise);
        
        vm.setOrganisation();
        
        deferred.resolve(response);
        $scope.$digest();

        expect(deliveryPointService.setOrganisation).toHaveBeenCalled();
        expect(vm.dpUse).toBe(response.dpUse);
        expect(vm.selectedDPUse).toBe(response.selectedDPUse);
    });

    it('should be set toggle', function() {
        vm.toggle(true);
        expect(vm.selectedItem).toBe(true);       
    });    

    it('should be call update UpdateDeliverypoint once savePositionedDeliveryPoint method called ', function() {
        spyOn(deliveryPointService,'UpdateDeliverypoint').and.callThrough();

        vm.savePositionedDeliveryPoint();

        expect(vm.isOnceClicked).toBe(true);        
        expect(vm.positionedDeliveryPointList).toBe(null);
        expect(deliveryPointService.UpdateDeliverypoint).toHaveBeenCalled();
        expect(deliveryPointService.UpdateDeliverypoint).toHaveBeenCalledWith('firstPointList');
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

    it('should add an item when `addAlias` method called ', function() {
        vm.addAlias();
        expect(vm.items).toEqual([{ Preferred: false, DPAlias: null }]);
        expect(vm.alias).toBeDefined();
        expect(vm.alias).toBe('');
    });

    it('should remove an item when removeAlias method called', function() {
        vm.items = [1,2,3,4,5];
        vm.removeAlias();
        var lastItem = vm.items.length - 1;
        vm.items.splice(lastItem);
        expect(lastItem).toBe(3);
    });
});

