'use strict';
describe('Search: Factory', function() {
	var $httpBackend;
	var GlobalSettings;
	var $q;	
	var stringFormatService;
	var searchService; 
    var MockGlobalSettings = {
    	searchManagerApiUrl: "http://localhost:43423/SearchManager/api",
        fetchBasicSearchResults: "/searchmanager/basic/",
        fetchAdvanceSearchResults: "/searchmanager/advance/",
        deliveryPointApiUrl: "http://localhost:43423/DeliveryPoint/api",
		getDeliveryPointById: "/DeliveryPointManager/deliverypoint/Guid/{0}"
    };

    var MockStringFormatService = {
        Stringformat: function() { 
        var theString = arguments[0];
           for (var i = 1; i < arguments.length; i++) {               
               var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
               theString = theString.replace(regEx, arguments[i]);
           }
           return theString;
        }
    };

	beforeEach(function() {
		module('search');
		module(function($provide){
            $provide.value('GlobalSettings', MockGlobalSettings);
            $provide.value('stringFormatService', MockStringFormatService);
		});

		inject(function(
            _$httpBackend_,
            _$q_,
            _GlobalSettings_,
            _stringFormatService_,
            _searchService_){

			$httpBackend = _$httpBackend_;
			$q = _$q_;
			GlobalSettings = _GlobalSettings_;
			stringFormatService = _stringFormatService_;
			searchService = _searchService_;
		});
	});

	it('should promise to return a success response once basicSearch method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.searchManagerApiUrl+GlobalSettings.fetchBasicSearchResults+'basicSearch';
        var basicSearchMockData = {"id":"d8053df2-1aad-40c8-bbf6-de0ae783e772","externalId":null,"routeName":"1101 NEWLAND ROAD             ","routeNumber":"101       ","operationalStatus_Id":0,"routeMethodType_Id":0,"travelOutTransportType_Id":null,"travelInTransportType_Id":null,"travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"displayText":"(101)1101 NEWLAND ROAD             ","methodReferenceGuid":"c168f46e-561b-e711-9f8c-28d244aef9ed","method":"High Capacity Trolley","deliveryOffice":null,"aliases":0,"blocks":2,"scenarioName":"Worthing Delivery Office - Baseline weekday","dPs":677,"businessDPs":1,"residentialDPs":676,"travelOutTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","travelInTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","accelarationIn":"High Capacity Trolley","accelarationOut":"High Capacity Trolley","pairedRoute":"","totaltime":"0:04 mins"};

        $httpBackend.when('GET', expectedUrl).respond(200, basicSearchMockData);            
        searchService.basicSearch('basicSearch').then(function(data) { response = data; });   

        $httpBackend.flush();
        expect(response).toEqual(basicSearchMockData);
    });
   
    it('should promise to return a error response once basicSearch method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.searchManagerApiUrl+GlobalSettings.fetchBasicSearchResults+'basicSearch';

        $httpBackend.when('GET', expectedUrl).respond(500);            

        searchService.basicSearch('basicSearch')
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

    it('should promise to return a success response once advanceSearch method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.searchManagerApiUrl+GlobalSettings.fetchAdvanceSearchResults+'advanceSearch';
        var advanceSearchMockData = {"id":"d8053df2-1aad-40c8-bbf6-de0ae783e772","externalId":null,"routeName":"1101 NEWLAND ROAD             ","routeNumber":"101       ","operationalStatus_Id":0,"routeMethodType_Id":0,"travelOutTransportType_Id":null,"travelInTransportType_Id":null,"travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"displayText":"(101)1101 NEWLAND ROAD             ","methodReferenceGuid":"c168f46e-561b-e711-9f8c-28d244aef9ed","method":"High Capacity Trolley","deliveryOffice":null,"aliases":0,"blocks":2,"scenarioName":"Worthing Delivery Office - Baseline weekday","dPs":677,"businessDPs":1,"residentialDPs":676,"travelOutTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","travelInTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","accelarationIn":"High Capacity Trolley","accelarationOut":"High Capacity Trolley","pairedRoute":"","totaltime":"0:04 mins"};

        $httpBackend.when('GET', expectedUrl).respond(200, advanceSearchMockData);            
        searchService.advanceSearch('advanceSearch').then(function(data) { response = data; });   

        $httpBackend.flush();
        expect(response).toEqual(advanceSearchMockData);
    });
   
    it('should promise to return a error response once advanceSearch method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.searchManagerApiUrl+GlobalSettings.fetchAdvanceSearchResults+'advanceSearch';

        $httpBackend.when('GET', expectedUrl).respond(500);            

        searchService.advanceSearch('advanceSearch')
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

    it('should promise to return a success response once GetDeliveryPointByGuid method is called', function() {
        var response; 
        var getDeliveryPointGuidParams = '/DeliveryPointManager/deliverypoint/Guid/d8053df2-1aad-40c8-bbf6-de0ae783e772';
        var expectedUrl = GlobalSettings.deliveryPointApiUrl+getDeliveryPointGuidParams;
        var GetDeliveryPointByGuidMockData = {"id":"d8053df2-1aad-40c8-bbf6-de0ae783e772","externalId":null,"routeName":"1101 NEWLAND ROAD             ","routeNumber":"101       ","operationalStatus_Id":0,"routeMethodType_Id":0,"travelOutTransportType_Id":null,"travelInTransportType_Id":null,"travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"displayText":"(101)1101 NEWLAND ROAD             ","methodReferenceGuid":"c168f46e-561b-e711-9f8c-28d244aef9ed","method":"High Capacity Trolley","deliveryOffice":null,"aliases":0,"blocks":2,"scenarioName":"Worthing Delivery Office - Baseline weekday","dPs":677,"businessDPs":1,"residentialDPs":676,"travelOutTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","travelInTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","accelarationIn":"High Capacity Trolley","accelarationOut":"High Capacity Trolley","pairedRoute":"","totaltime":"0:04 mins"};

        $httpBackend.when('GET', expectedUrl).respond(200, GetDeliveryPointByGuidMockData);            
        searchService.GetDeliveryPointByGuid('d8053df2-1aad-40c8-bbf6-de0ae783e772').then(function(data) { response = data; });   

        $httpBackend.flush();
        expect(response).toEqual(GetDeliveryPointByGuidMockData);
    });
   
    it('should promise to return a error response once GetDeliveryPointByGuid method is called', function() {
        var response; 
        var getDeliveryPointGuidParams = '/DeliveryPointManager/deliverypoint/Guid/d8053df2-1aad-40c8-bbf6-de0ae783e772';
        var expectedUrl = GlobalSettings.deliveryPointApiUrl+getDeliveryPointGuidParams;

        $httpBackend.when('GET', expectedUrl).respond(500);            

        searchService.GetDeliveryPointByGuid('d8053df2-1aad-40c8-bbf6-de0ae783e772')
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });
});