'use strict';
describe('Unit Selector: Service', function() {
	var sessionStorage;
	var $rootScope;
	var $q;
	var GlobalSettings;
	var $filter;
	var manageAccessService;
	var mapFactory;
	var unitSelectorAPIService;
	var licensingInfoService;
	var unitSelectorService;
	var selectedDeliveryUnit = {
			ID:"b51aa229-c984-4ca6-9c12-510187b81050",
			boundingBox:[505058.162109375,100281.56298828125,114158.74194335938,518986.837890625],
			boundingBoxCenter:[512022.5,107220.15246582034],
			unitBoundryPolygon:{coordinates:[508595.6009999998,111763.69700000063],crs:{"type":"name","properties":{"name":"EPSG:27700"}}}
		};   

	beforeEach(function() {
		module('unitSelector');
		module(function($provide){
			$provide.value('GlobalSettings',{});
			$provide.factory('manageAccessService',function(){
				return {
					activate: function(){}
				}
			});
			$provide.factory('mapFactory',function(){
				return {
					setUnitBoundaries: function(){}
				}
			});
			$provide.factory('licensingInfoService',function(){
				return {
					licensingInfoService: function(){}
				}
			});
			$provide.factory('unitSelectorAPIService',function($q){
				function getDeliveryUnit(){
					var MockGetDeliveryUnitData = [{area:"BT",boundingBox:[-1609.23828125,464560.66015625,464560.66015625,611820.83984375],boundingBoxCenter:[88881.95117187509,538190.7500000001],id:"38fd2404-d65b-e711-80e2-000d3a22173b",unitAddressUDPRN:0,unitBoundryPolygon:{coordinates:[1458.0029999995604,1458.0029999995604],crs:{properties:{name:"EPSG:27700"}},type:'MultiPloygon',unitName:'National IN'}}];
					var deffer = $q.defer();
					deffer.resolve(MockGetDeliveryUnitData);
					return deffer.promise;
				}
				return{
					getDeliveryUnit: getDeliveryUnit
				}
			});		
			$provide.factory('sessionStorage',function(){
				var store = {};
				return {
					getItem: function(key){
				      return store[key];
				    },
				    setItem: function(key,value){
				      store[key] = `${value}`;
				    },
				    removeItem: function(key) {
				      delete store[key];
				    },
				    clear: function() {
				      store = {};
				    }
				}
			});
		});
		inject(function(
			_sessionStorage_,		
			_$rootScope_,
			_$q_,
			_GlobalSettings_,
			_$filter_,
			_manageAccessService_,
			_mapFactory_,
			_unitSelectorAPIService_,
			_licensingInfoService_,
			_unitSelectorService_){

			$rootScope = _$rootScope_;
			$q = _$q_;
			GlobalSettings = _GlobalSettings_;
			$filter = _$filter_;
			manageAccessService = _manageAccessService_;
			mapFactory = _mapFactory_;
			unitSelectorAPIService = _unitSelectorAPIService_;
			licensingInfoService = _licensingInfoService_;
			unitSelectorService =_unitSelectorService_;
			sessionStorage: _sessionStorage_;
		});		       
	});

	it('should call updateMapAfterUnitChange method when DeliveryUnit method called', function() {		
		spyOn(manageAccessService,'activate');
		spyOn(mapFactory,'setUnitBoundaries');

		unitSelectorService.DeliveryUnit(selectedDeliveryUnit);

		expect(manageAccessService.activate).toHaveBeenCalledWith('b51aa229-c984-4ca6-9c12-510187b81050');
		expect(mapFactory.setUnitBoundaries).toHaveBeenCalledWith(selectedDeliveryUnit.boundingBox,selectedDeliveryUnit.boundingBoxCenter,selectedDeliveryUnit.unitBoundryPolygon);
	});

    xit('should promise to return a success response when `authData.unitGuid` is true once queryAdvanceSearch method is called', function() {
		var response;
		var MockDeliveryRouteUnitData = [{area:"BT",boundingBox:[-1609.23828125,464560.66015625,464560.66015625,611820.83984375],boundingBoxCenter:[88881.95117187509,538190.7500000001],id:"38fd2404-d65b-e711-80e2-000d3a22173b",unitAddressUDPRN:0,unitBoundryPolygon:{coordinates:[1458.0029999995604,1458.0029999995604],crs:{properties:{name:"EPSG:27700"}},type:'MultiPloygon',unitName:'National IN'}}];
		var sessionStorageData = {token:"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzaG9iaGFyYW0ua2F0aXlhIiwianRpIjoiMzNlYjU3NTAtYjI0YS00OTI2LTkwOGYtNzBkN2Q1MjI1MmVkIiwiaWF0IjoxNTAwOTcwMjgxLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoiMzhmZDI0MDQtZDY1Yi1lNzExLTgwZTItMDAwZDNhMjIxNzNiIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InNob2JoYXJhbS5rYXRpeWEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ByaW1hcnlzaWQiOiJhODY3MDY1Yi1iOTFlLWU3MTEtOWY4Yy0yOGQyNDRhZWY5ZWQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiUHJpbnQgTWFwIiwiVmlldyBEZWxpdmVyeSBQb2ludHMiLCJNYWludGFpbiBEZWxpdmVyeSBQb2ludHMiLCJWaWV3IExpdmUgUm91dGUgTG9ncyIsIlZpZXcgRnV0dXJlIFJvdXRlIExvZ3MiLCJTaW11bGF0ZSBMaXZlIFJvdXRlIiwiU2ltdWxhdGUgRnV0dXJlIFJvdXRlIiwiVmlldyBQTyBCb3hlcyIsIlZpZXcgRGVsaXZlcnkgR3JvdXBzIiwiVmlldyBBY2Nlc3MgTGlua3MiLCJWaWV3IFJvdXRlcyIsIlZpZXcgSGF6YXJkcyIsIlZpZXcgRGVsaXZlcnkgU3BlY2lhbCBJbnN0cnVjdGlvbnMiLCJQcmludCBPdmVyaGVhZCBMYWJlbCIsIlByaW50IFNvcnRpbmcgRnJhbWUiLCJQcmludCBCYXJjb2RlcyJdLCJuYmYiOjE1MDA5NzAyODEsImV4cCI6MTUwMDk4ODI4MSwiaXNzIjoiUk1HIiwiYXVkIjoiUk1HX0FEX0NsaWVudHMifQ.2vnAZRTRT2ZPNJiCv4765dPw77i38rECX1YsIRWWzqM",userName:"shobharam.katiya",unitGuid:null};
		spyOn(mapFactory,'setUnitBoundaries');
		spyOn(sessionStorage, 'getItem').and.callFake(function(key){
        	var authData = {unitGuid:"b51aa229-c984-4ca6-9c12-510187b81050"};
        	return authData;
      	});
		//spyOn(angular,'fromJson').and.returnValue({username:'shobharam.katiy'});
		
        
		unitSelectorService.BindData([]).then(function(result){
			response = result;
		});
		$rootScope.$apply();

		expect(response).toEqual([{ deliveryRouteUnit: [{ displayText: 'BT    undefined', ID: '38fd2404-d65b-e711-80e2-000d3a22173b', icon: 'fa-map-marker delivery', area: 'BT', boundingBox: [ -1609.23828125, 464560.66015625, 464560.66015625, 611820.83984375 ], unitName: undefined, boundingBoxCenter: [ 88881.95117187509, 538190.7500000001 ], unitBoundryPolygon: { coordinates: [ 1458.0029999995604, 1458.0029999995604 ], crs: { properties: { name: 'EPSG:27700' }}, type: 'MultiPloygon', unitName: 'National IN' }}], 
        	selectedUser: { displayText: 'BT    undefined', ID: '38fd2404-d65b-e711-80e2-000d3a22173b', icon: 'fa-map-marker delivery', area: 'BT', boundingBox: [ -1609.23828125, 464560.66015625, 464560.66015625, 611820.83984375 ], unitName: undefined, boundingBoxCenter: [ 88881.95117187509, 538190.7500000001 ], unitBoundryPolygon: { coordinates: [ 1458.0029999995604, 1458.0029999995604 ], crs: { properties: { name: 'EPSG:27700' }}, type: 'MultiPloygon', unitName: 'National IN' }},
        	selectedDeliveryUnit: { area: 'BT', boundingBox: [ -1609.23828125, 464560.66015625, 464560.66015625, 611820.83984375 ], boundingBoxCenter: [ 88881.95117187509, 538190.7500000001 ], id: '38fd2404-d65b-e711-80e2-000d3a22173b', unitAddressUDPRN: 0, unitBoundryPolygon: { coordinates: [ 1458.0029999995604, 1458.0029999995604 ], crs: { properties: { name: 'EPSG:27700' }}, type: 'MultiPloygon', unitName: 'National IN' }}, isDeliveryUnitDisabled: true }]);

		expect(mapFactory.setUnitBoundaries).toHaveBeenCalledWith(MockDeliveryRouteUnitData[0].boundingBox,MockDeliveryRouteUnitData[0].boundingBoxCenter,MockDeliveryRouteUnitData[0].unitBoundryPolygon,);
	});

	xit('should promise to return a success response when `authData.unitGuid` is false once queryAdvanceSearch method is called', function() {
		var response;
		var MockDeliveryRouteUnitData = [{area:"BT",boundingBox:[-1609.23828125,464560.66015625,464560.66015625,611820.83984375],boundingBoxCenter:[88881.95117187509,538190.7500000001],id:"38fd2404-d65b-e711-80e2-000d3a22173b",unitAddressUDPRN:0,unitBoundryPolygon:{coordinates:[1458.0029999995604,1458.0029999995604],crs:{properties:{name:"EPSG:27700"}},type:'MultiPloygon',unitName:'National IN'}}];
		var sessionStorageData = {token:"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJzaG9iaGFyYW0ua2F0aXlhIiwianRpIjoiMzNlYjU3NTAtYjI0YS00OTI2LTkwOGYtNzBkN2Q1MjI1MmVkIiwiaWF0IjoxNTAwOTcwMjgxLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3VzZXJkYXRhIjoiMzhmZDI0MDQtZDY1Yi1lNzExLTgwZTItMDAwZDNhMjIxNzNiIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InNob2JoYXJhbS5rYXRpeWEiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3ByaW1hcnlzaWQiOiJhODY3MDY1Yi1iOTFlLWU3MTEtOWY4Yy0yOGQyNDRhZWY5ZWQiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOlsiUHJpbnQgTWFwIiwiVmlldyBEZWxpdmVyeSBQb2ludHMiLCJNYWludGFpbiBEZWxpdmVyeSBQb2ludHMiLCJWaWV3IExpdmUgUm91dGUgTG9ncyIsIlZpZXcgRnV0dXJlIFJvdXRlIExvZ3MiLCJTaW11bGF0ZSBMaXZlIFJvdXRlIiwiU2ltdWxhdGUgRnV0dXJlIFJvdXRlIiwiVmlldyBQTyBCb3hlcyIsIlZpZXcgRGVsaXZlcnkgR3JvdXBzIiwiVmlldyBBY2Nlc3MgTGlua3MiLCJWaWV3IFJvdXRlcyIsIlZpZXcgSGF6YXJkcyIsIlZpZXcgRGVsaXZlcnkgU3BlY2lhbCBJbnN0cnVjdGlvbnMiLCJQcmludCBPdmVyaGVhZCBMYWJlbCIsIlByaW50IFNvcnRpbmcgRnJhbWUiLCJQcmludCBCYXJjb2RlcyJdLCJuYmYiOjE1MDA5NzAyODEsImV4cCI6MTUwMDk4ODI4MSwiaXNzIjoiUk1HIiwiYXVkIjoiUk1HX0FEX0NsaWVudHMifQ.2vnAZRTRT2ZPNJiCv4765dPw77i38rECX1YsIRWWzqM",userName:"shobharam.katiya",unitGuid:null};
		spyOn(mapFactory,'setUnitBoundaries');
		/*spyOn(sessionStorage, 'getItem').and.callFake(function(authorizationData) {
            return sessionStorageData;
        });*/
        //var authData = {unitGuid:"b51aa229-c984-4ca6-9c12-510187b81050"};
        
		unitSelectorService.BindData([]).then(function(result){
			response = result;
		});
		$rootScope.$apply();

		expect(response).toEqual([{ deliveryRouteUnit: [{ displayText: 'BT    undefined', ID: '38fd2404-d65b-e711-80e2-000d3a22173b', icon: 'fa-map-marker delivery', area: 'BT', boundingBox: [ -1609.23828125, 464560.66015625, 464560.66015625, 611820.83984375 ], unitName: undefined, boundingBoxCenter: [ 88881.95117187509, 538190.7500000001 ], unitBoundryPolygon: { coordinates: [ 1458.0029999995604, 1458.0029999995604 ], crs: { properties: { name: 'EPSG:27700' }}, type: 'MultiPloygon', unitName: 'National IN' }}], 
        	selectedUser: { displayText: 'BT    undefined', ID: '38fd2404-d65b-e711-80e2-000d3a22173b', icon: 'fa-map-marker delivery', area: 'BT', boundingBox: [ -1609.23828125, 464560.66015625, 464560.66015625, 611820.83984375 ], unitName: undefined, boundingBoxCenter: [ 88881.95117187509, 538190.7500000001 ], unitBoundryPolygon: { coordinates: [ 1458.0029999995604, 1458.0029999995604 ], crs: { properties: { name: 'EPSG:27700' }}, type: 'MultiPloygon', unitName: 'National IN' }},
        	selectedDeliveryUnit: { area: 'BT', boundingBox: [ -1609.23828125, 464560.66015625, 464560.66015625, 611820.83984375 ], boundingBoxCenter: [ 88881.95117187509, 538190.7500000001 ], id: '38fd2404-d65b-e711-80e2-000d3a22173b', unitAddressUDPRN: 0, unitBoundryPolygon: { coordinates: [ 1458.0029999995604, 1458.0029999995604 ], crs: { properties: { name: 'EPSG:27700' }}, type: 'MultiPloygon', unitName: 'National IN' }}, isDeliveryUnitDisabled: false }]);

		expect(mapFactory.setUnitBoundaries).toHaveBeenCalledWith(MockDeliveryRouteUnitData[0].boundingBox,MockDeliveryRouteUnitData[0].boundingBoxCenter,MockDeliveryRouteUnitData[0].unitBoundryPolygon,);
	});
});