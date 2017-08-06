'use strict';
describe('Delivery Point: Service', function () {
    var referencedataApiService;
    var $filter;
    var $q;
    var deliveryPointAPIService;
    var guidService;
    var $mdDialog;
    var mapFactory;
    var $rootScope;    
    var $scope;
    var $state;
    

    beforeEach(function () {
        module('deliveryPoint'); 
        module(function ($provide) {
            $provide.factory('deliveryPointAPIService', function ($q) {                     
                function GetDeliveryPointsResultSet(searchText) {                    
                    var deffer = $q.defer();
                    deffer.resolve([{"value":"null","displayText":"Not Shown"}]);
                    return deffer.promise;
                }
                function GetAddressByPostCode(selectedItem) {                    
                    var deffer = $q.defer();
                    deffer.resolve({"nybAddressDetails":[{"value":"00000000-0000-0000-0000-000000000000"}]});
                    return deffer.promise;
                }                  
                function GetAddressLocation(udprn){                    
                    var deffer = $q.defer();
                    deffer.resolve({AddressDetails:'Abbey Road,BN11 3RW saffron hills'});
                    return deffer.promise;
                }                  
                function GetPostalAddressByGuid(addressGuid) {                    
                    var deffer = $q.defer();
                    deffer.resolve({AddressDetails:'Abbey Road,BN11 3RW saffron hills'});
                    return deffer.promise;
                }                  
                               
                function UpdateDeliverypoint(deliveryPointModelDTO) {                                                        
                    var deffer = $q.defer();
                    deffer.resolve([{"udprn":"","locality":"45 test001    Abbotts Close BN11 1JB","addressGuid":"00000000-0000-0000-0000-000000000000","id":"4124f93d-e679-498e-b454-c82768a4732e","xCoordinate":512073.18010136025,"yCoordinate":107209.5124445403,"latitude":50.853553614182566,"longitude":-0.40920540821937557,"rowversion":"AAAAAAAFCSk=","$$hashKey":"object:309"}]);
                    return deffer.promise;
                }                  
                return {
                    GetDeliveryPointsResultSet: GetDeliveryPointsResultSet,
                    GetAddressByPostCode: GetAddressByPostCode,
                    GetAddressLocation: GetAddressLocation,
                    GetPostalAddressByGuid: GetPostalAddressByGuid,
                    UpdateDeliverypoint: UpdateDeliverypoint
                };
            });

            $provide.factory('referencedataApiService', function ($q) {
                function getSimpleListsReferenceData(searchText) {                                                
                    var MockGetSimpleListData = {"listItems":[{"id":"ec1b3a60-f22f-e711-8735-28d244aef9ed","name":null,"value":"Single","displayText":null,"description":"Single Delivery Point"}]};
                    var deffer = $q.defer();
                    deffer.resolve(MockGetSimpleListData);
                    return deffer.promise;
                }                 
                return {
                    getSimpleListsReferenceData: getSimpleListsReferenceData
                };
            });

            $provide.value('referenceDataConstants',{
                UI_DeliveryPoint_Type: { DBCategoryName: "UI_DeliveryPoint_Type"},
                DeliveryPointUseIndicator: { DBCategoryName: "DeliveryPoint Use Indicator"}
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
            function setDeliveryPointOnLoad() { }
            return {
              initialiseMap: initialiseMap,
                getMap: getMap,             
                setAccessLink : setAccessLink,
                setDeliveryPointOnLoad: setDeliveryPointOnLoad          
              }
          });

          $provide.value('$state', { 
            go: function(state, args){}
          });

          $provide.value('GlobalSettings', {});
            $provide.factory('$mdDialog', function() {
                return {
                        hide: jasmine.createSpy(),
                        show: jasmine.createSpy()
                      };
            });
        });

        inject(function (
            _$rootScope_,
            _deliveryPointService_,
            _deliveryPointAPIService_,
            _referencedataApiService_,
            _$filter_,
            _$q_,
            _$mdDialog_,
            _$state_,
            _mapFactory_,
            _guidService_) {

            $rootScope = _$rootScope_;
            $scope = _$rootScope_.$new();
            deliveryPointService = _deliveryPointService_;
            deliveryPointAPIService = _deliveryPointAPIService_;
            referencedataApiService = _referencedataApiService_;
            $filter = _$filter_;
            $q = _$q_;
            $mdDialog = _$mdDialog_;
            $state = _$state_;
            mapFactory = _mapFactory_;
            guidService = _guidService_;
        });
    });            
    
    it('should promise to return a success response once deliveryPointTypes method is called', function() { 
        var response;       
        deliveryPointService.deliveryPointTypes("UI_DeliveryPoint_Type").then(function(result){
            response = result;
        });
        $rootScope.$apply();
        expect(response).toEqual([{"id":"ec1b3a60-f22f-e711-8735-28d244aef9ed","name":null,"value":"Single","displayText":null,"description":"Single Delivery Point"}]);       
    });

    it('should promise to return a success response once deliveryPointUseType method is called', function() {
        var response;        
        deliveryPointService.deliveryPointUseType("DeliveryPoint Use Indicator").then(function(result){
            response = result;
        });
        $rootScope.$apply();
        expect(response).toEqual([{"id":"ec1b3a60-f22f-e711-8735-28d244aef9ed","name":null,"value":"Single","displayText":null,"description":"Single Delivery Point"}]);       
    });

    it('should return promise response success when resultSet method query argument length is `greater then 3`', function() { 
        var response;
        deliveryPointService.resultSet('BTN1').then(function(result){
            response = result;
        });
        $rootScope.$apply();
        expect(response).toEqual([{"value":"null","displayText":"Not Shown"}]);
    });

    it('should return promise response success when resultSet method query argument length is `less then 3`', function() {        
        var response;
        deliveryPointService.resultSet('BT').then(function(result){
            response = result;
        });
        $rootScope.$apply();
        expect(response).toEqual([]);
    });

    it('should be dialog show when popup model open', function() {
        var popupSetting = {"templateUrl":"./delivery-point/delivery-point.template.html","clickOutsideToClose":false,"controllerAs":"vm","preserveScope":true};
        deliveryPointService.openModalPopup(popupSetting);
        expect($mdDialog.show).toHaveBeenCalledWith(popupSetting);
    });

    it('should be dialog close when popup model close', function() {
        deliveryPointService.closeModalPopup();
        expect($mdDialog.hide).toHaveBeenCalled();
    });
 
    it('should promise to return a success response once getPostalAddress method is called', function() {
        var response;
        deliveryPointService.getPostalAddress('SA62 5DL').then(function(result){
            response = result;
        });        
        $rootScope.$apply();        
        expect(response).toEqual({"postalAddressData":{"nybAddressDetails":[{"value":"00000000-0000-0000-0000-000000000000"}]}, selectedValue: '00000000-0000-0000-0000-000000000000', display: true });        
    });   

    it('should promise to return a success response once bindAddressDetails method is called', function() {
        var response;
        deliveryPointService.bindAddressDetails("Abbey Road,BN11 3RW").then(function(result){
            response = result;
        });
        $rootScope.$apply();
        expect(response).toEqual({AddressDetails:'Abbey Road,BN11 3RW saffron hills'});       
    });

    it('should return promise filter Organisation when setOrganisation method called', function() {
        var addressDetails = {postcodeType: "S", organisationName: "Royal", departmentName: undefined, buildingName: undefined, buildingNumber: undefined};
        var dpUseType = [{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"value":"Organisation","displayText":null,"description":"Organisation"}];
        var response;

        deliveryPointService.setOrganisation(addressDetails,dpUseType).then(function(result){
            response = result;
        });
        $rootScope.$apply();  
        expect(response).toEqual({"dpUse":[{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"value":"Organisation","displayText":null,"description":"Organisation"}],"selectedDPUse":{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"value":"Organisation","displayText":null,"description":"Organisation"}});
    });

    it('should return promise filter Residential when setOrganisation method called', function() {
        var addressDetails = {};
        var dpUseType = [{"value":"Residential","displayText":null,"description":"Organisation"}];
        var response;

        deliveryPointService.setOrganisation(addressDetails,dpUseType).then(function(result){
            response = result;
        });
        $rootScope.$apply();          
        expect(response).toEqual({ dpUse: [{ value: 'Residential', displayText: null, description: 'Organisation' }], selectedDPUse:{ value: 'Residential', displayText: null, description: 'Organisation' }});
    });

    it('should be return object when isUndefinedOrNull method called and passed argument object', function() {        
        expect(deliveryPointService.isUndefinedOrNull({'value':true})).toEqual({'value':true});
    });

    it('should be return empty when isUndefinedOrNull method called', function() {        
        expect(deliveryPointService.isUndefinedOrNull()).toBe('');
    });

    it('should promise to return a success response once UpdateDeliverypoint method is called', function() {
        spyOn(mapFactory,'setAccessLink');
        spyOn(mapFactory,'setDeliveryPointOnLoad');
        spyOn(guidService,'setGuid');
        spyOn($state,'go');
        spyOn($rootScope,'$broadcast');
        var positionedDeliveryPointList = [{"udprn":"","locality":"12 asdf asdf asdf  Abbotts Close BN11 1JB","addressGuid":"00000000-0000-0000-0000-000000000000","id":"a6f948de-27e3-4a8c-9abb-7a72785b825d","xCoordinate":511771.6194982389,"yCoordinate":107359.03274358087,"latitude":50.8549559488007,"longitude":-0.41344215990658806,"rowversion":"AAAAAAAG3dE=","$$hashKey":"object:374"}]
        var response;

        deliveryPointService.UpdateDeliverypoint(positionedDeliveryPointList).then(function(result){
            response = result;
        });
        $rootScope.$apply();

        expect($rootScope.$broadcast).toHaveBeenCalledWith('disablePrintMap',{disable:false});
        expect(mapFactory.setAccessLink).toHaveBeenCalled();
        expect(mapFactory.setDeliveryPointOnLoad).toHaveBeenCalled();
        expect(guidService.setGuid).toHaveBeenCalled();
        expect($state.go).toHaveBeenCalledWith('deliveryPoint', { positionedDeliveryPointList: vm.positionedDeliveryPointList });        
    });
});

