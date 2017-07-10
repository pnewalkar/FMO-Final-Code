'use strict';
describe('Delivery Point: Service', function () {
    var referencedataApiService;
    var $filter;
    var $q;
    var deliveryPointAPIService;
    var guidService;
    var $mdDialog;
    var state;
    var mapFactory;
    var $filter;
    var $rootScope;
    var deferred;
    var MockMdHide;
    var MockMdShow;
    var scope;
    var $state;

    beforeEach(function () {
        module('deliveryPoint'); 
        module(function ($provide) {
            $provide.factory('deliveryPointAPIService', function ($q) {                
                function GetDeliveryPointsResultSet(searchText) { return; }
                function GetAddressByPostCode(selectedItem) { return; }                  
                function GetAddressLocation(udprn){ return; }                  
                function GetPostalAddressByGuid(addressGuid) { return; }                  
                function CreateDeliveryPoint(addDeliveryPointDTO){ return; }                  
                function UpdateDeliverypoint(deliveryPointModelDTO) { return; }                  
                return {
                    GetDeliveryPointsResultSet: GetDeliveryPointsResultSet,
                    GetAddressByPostCode: GetAddressByPostCode,
                    GetAddressLocation: GetAddressLocation,
                    GetPostalAddressByGuid: GetPostalAddressByGuid,
                    CreateDeliveryPoint: CreateDeliveryPoint,
                    UpdateDeliverypoint: UpdateDeliverypoint
                };
            });

            $provide.factory('referencedataApiService', function ($q) {
                function getReferenceData() {}                
                return {
                    getReferenceData: getReferenceData,
                };
            });

            $provide.value('referenceDataConstants',{});
                           
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
          MockMdHide = jasmine.createSpy();
          MockMdShow = jasmine.createSpy(true);
            $provide.factory('$mdDialog', function() {
                return {
                        hide: MockMdHide,
                        show: MockMdShow
                      };
            });
        });

        inject(function (_$rootScope_,_deliveryPointService_,_deliveryPointAPIService_,_referencedataApiService_,_$filter_,_$q_,_$mdDialog_,_$state_,_mapFactory_,_guidService_) {
            $rootScope = _$rootScope_;
            scope = _$rootScope_.$new();
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
    
    it('should return promise response success when resultSet method query argument length is `greater then 3`', function() {
        var deferredSuccess = $q.defer();
        var query = [{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"}];

        spyOn(deliveryPointAPIService,'GetDeliveryPointsResultSet').and.returnValue(deferredSuccess.promise);
        
        deliveryPointService.resultSet(query);

        deferredSuccess.resolve(query); 
        scope.$digest();  

        expect(deliveryPointAPIService.GetDeliveryPointsResultSet).toHaveBeenCalled();
        expect(query).toEqual([{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"},{"value":"null","displayText":"Not Shown"}]);
        expect(query).not.toBe([]);
    });

    it('should return promise response success when resultSet method query argument length is `less then 3`', function() {
        var deferredSuccess = $q.defer();
        var query = [];

        spyOn(deliveryPointAPIService,'GetDeliveryPointsResultSet').and.returnValue(deferredSuccess.promise);
        
        deliveryPointService.resultSet(query);

        deferredSuccess.resolve(result); 
        scope.$digest();  

        expect(deliveryPointAPIService.GetDeliveryPointsResultSet).not.toHaveBeenCalled();
        expect(query).toEqual([]);
    });

    it('should be dialog show when popup open', function() {
        deliveryPointService.openModalPopup(true);
        expect($mdDialog.show).toHaveBeenCalled();
        expect($mdDialog.show).toHaveBeenCalledWith(true);
    });

    it('should be dialog model close when popup close', function() {
        deliveryPointService.closeModalPopup(false);
        expect($mdDialog.hide).toHaveBeenCalled();
    });
 
    it('should promise to return a success response once getPostalAddress method is called', function() {
        var deferred = $q.defer();
        var response;
        var result = {"postalAddressData": null, "selectedValue": null, "display": null };
        var postcode = 400080;

        spyOn(deliveryPointAPIService, 'GetAddressByPostCode').and.returnValue(deferred.promise);

        deliveryPointService.getPostalAddress(postcode);


        deferred.resolve(result);
        scope.$digest();

        expect(deliveryPointAPIService.GetAddressByPostCode).toHaveBeenCalled();
        expect(deliveryPointAPIService.GetAddressByPostCode).toHaveBeenCalledWith(400080);
        expect(result).toEqual({"postalAddressData": null, "selectedValue": null, "display": null });
    });

    it('should promise to return a success response once bindAddressDetails method is called', function() {        
        var deferred = $q.defer();
        var response = {AddressDetails:'Abbey Road,BN11 3RW saffron hills'};
        var notyetBuilt = "Abbey Road,BN11 3RW";

        spyOn(deliveryPointAPIService, 'GetPostalAddressByGuid').and.returnValue(deferred.promise);

        deliveryPointService.bindAddressDetails(notyetBuilt);

        deferred.resolve(response);
        scope.$digest();  

        expect(deliveryPointAPIService.GetPostalAddressByGuid).toHaveBeenCalled();        
        expect(deliveryPointAPIService.GetPostalAddressByGuid).toHaveBeenCalledWith("Abbey Road,BN11 3RW"); 
        expect(response).toEqual({AddressDetails:'Abbey Road,BN11 3RW saffron hills'});       
    });

    it('should return promise filter Organisation', function() {
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
        
        deliveryPointService.setOrganisation(addressDetails,dpUseType);
        
        result.dpUse = filter(addressDetails,dpUseType);
        result.selectedDPUse = result.dpUse[0];
        deferred.resolve(result);
        deferred.promise;

        expect(result).toBeDefined();
        expect(result).toEqual({"dpUse":[{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"}],"selectedDPUse":{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Organisation","displayText":null,"description":"Organisation"}});
    });

    it('should return promise filter Residential', function() {
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
        
        deliveryPointService.setOrganisation(addressDetails,dpUseType);
        
        result.dpUse = filter(addressDetails,dpUseType);
        result.selectedDPUse = result.dpUse[0];
        deferred.resolve(result);
        deferred.promise;

        expect(result).toBeDefined();
        expect(result).toEqual({"dpUse":[{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Residential","displayText":null,"description":"Organisation"}],"selectedDPUse":{"id":"990b86a2-9431-e711-83ec-28d244aef9ed","name":null,"referenceDataValue":"Residential","displayText":null,"description":"Organisation"}});
    });

    it('should be return object when isUndefinedOrNull method called and passed argument object', function() {        
        expect(deliveryPointService.isUndefinedOrNull({'value':true})).toEqual({'value':true});
    });

    it('should be return empty when isUndefinedOrNull method called', function() {        
        expect(deliveryPointService.isUndefinedOrNull()).toBe('');
    });

    it('should promise to return a success response once UpdateDeliverypoint method is called', function() {
        var deferredSuccess = $q.defer();
        var positionedDeliveryPointList = [{"udprn":"","locality":"45 test001    Abbotts Close BN11 1JB","addressGuid":"00000000-0000-0000-0000-000000000000","id":"4124f93d-e679-498e-b454-c82768a4732e","xCoordinate":512073.18010136025,"yCoordinate":107209.5124445403,"latitude":50.853553614182566,"longitude":-0.40920540821937557,"rowversion":"AAAAAAAFCSk=","$$hashKey":"object:309"}];
        spyOn(deliveryPointAPIService, 'UpdateDeliverypoint').and.returnValue(deferredSuccess.promise);        
        spyOn(mapFactory,'setAccessLink');
        spyOn(mapFactory,'setDeliveryPointOnLoad');
        spyOn(guidService,'setGuid');
        spyOn($state,'go');

        deliveryPointService.UpdateDeliverypoint(121.11);

        deferredSuccess.resolve(positionedDeliveryPointList); 
        scope.$digest();  

        expect(deliveryPointAPIService.UpdateDeliverypoint).toHaveBeenCalled();
        expect(mapFactory.setAccessLink).toHaveBeenCalled();
        expect(mapFactory.setDeliveryPointOnLoad).toHaveBeenCalled();
        expect(guidService.setGuid).toHaveBeenCalled();
        expect($state.go).toHaveBeenCalled();
        expect($state.go).toHaveBeenCalledWith('deliveryPoint', { positionedDeliveryPointList: vm.positionedDeliveryPointList });        
    });
});

