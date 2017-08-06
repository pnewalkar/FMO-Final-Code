'use strict';
describe('Advance Search: Factory', function() {
	var $httpBackend;
	var GlobalSettings;
	var $q;	
    var MockGlobalSettings = {
    	searchManagerApiUrl: "http://localhost:43423/SearchManager/api",
        fetchAdvanceSearchResults: "/searchmanager/advance/",
    };    

	beforeEach(function() {
		module('advanceSearch');
		module(function($provide){
            $provide.value('GlobalSettings', MockGlobalSettings);
		});
		inject(function(_advanceSearchAPIService_,_$httpBackend_,_$q_,_GlobalSettings_){   
            advanceSearchAPIService = _advanceSearchAPIService_;         
			$httpBackend = _$httpBackend_;
			$q = _$q_;
			GlobalSettings = _GlobalSettings_;
		});
	});

	it('should promise to return a success response once advanceSearch method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.searchManagerApiUrl+GlobalSettings.fetchAdvanceSearchResults + 'road';
        var AdvanceSearchMockData = {"id":"d8053df2-1aad-40c8-bbf6-de0ae783e772","externalId":null,"routeName":"1101 NEWLAND ROAD             ","routeNumber":"101       ","operationalStatus_Id":0,"routeMethodType_Id":0,"travelOutTransportType_Id":null,"travelInTransportType_Id":null,"travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"displayText":"(101)1101 NEWLAND ROAD             ","methodReferenceGuid":"c168f46e-561b-e711-9f8c-28d244aef9ed","method":"High Capacity Trolley","deliveryOffice":null,"aliases":0,"blocks":2,"scenarioName":"Worthing Delivery Office - Baseline weekday","dPs":677,"businessDPs":1,"residentialDPs":676,"travelOutTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","travelInTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","accelarationIn":"High Capacity Trolley","accelarationOut":"High Capacity Trolley","pairedRoute":"","totaltime":"0:04 mins"};

        $httpBackend.when('GET', expectedUrl).respond(200, AdvanceSearchMockData);            
        advanceSearchAPIService.advanceSearch('road').then(function(data) { response = data; });   

        $httpBackend.flush();
        expect(response).toEqual(AdvanceSearchMockData);
    });
   
    it('should promise to return a error response once advanceSearch method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.searchManagerApiUrl+GlobalSettings.fetchAdvanceSearchResults + 'road';

        $httpBackend.when('GET', expectedUrl).respond(500);            

        advanceSearchAPIService.advanceSearch('road')
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });   
});