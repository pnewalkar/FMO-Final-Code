'use strict';
describe('Unit Selector: Factory', function() {
	var unitSelectorAPIService;
	var $rootScope;
	var $httpBackend;
	var $q;
	var GlobalSettings;
	var MockGlobalSettings = {
        unitManagerApiUrl: "http://localhost:50239/api",
        getDeliveryUnit: "/UnitManager/Unit"
    };

	beforeEach(function() {
		module('unitSelector');
		module(function($provide){
			$provide.value('GlobalSettings', MockGlobalSettings);
		});
		inject(function(_$rootScope_,_$q_,_$httpBackend_,_GlobalSettings_,_unitSelectorAPIService_){
			$rootScope = _$rootScope_;
			$q = _$q_;
			$httpBackend = _$httpBackend_;
			GlobalSettings = _GlobalSettings_;
			unitSelectorAPIService = _unitSelectorAPIService_;
		});
	});

    it('should promise to return a success response once getDeliveryUnit method is called', function() {        
        var response; 
        var getDeliveryUnitMockData = {"id":"87216073-e731-4b8c-9801-877ea4891f7e","listName":"Operational Status","maintainable":false,"listItems":[{"id":"9c1e56d7-5397-4984-9cf0-cd9ee7093c88","name":null,"value":"Live","displayText":null,"description":"Live"},{"id":"bee6048d-79b3-49a4-ad26-e4f5b988b7ab","name":null,"value":"Not Live","displayText":null,"description":"Not Live"}]};        
        var expectedUrl = GlobalSettings.unitManagerApiUrl+GlobalSettings.getDeliveryUnit;

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getDeliveryUnitMockData);

        unitSelectorAPIService.getDeliveryUnit()
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(getDeliveryUnitMockData);
    });
   
    it('should promise to return a error response once getDeliveryUnit method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.unitManagerApiUrl+GlobalSettings.getDeliveryUnit;

        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        unitSelectorAPIService.getDeliveryUnit()
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });
});