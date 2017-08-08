'use strict';
describe('Common: stringFormatService', function() {	
	var popUpSettingService;
	beforeEach(function() {		
        module('home');        
		module('RMApp');
		inject(function(_stringFormatService_){
			stringFormatService = _stringFormatService_;
		})
	});

	it("should replace URL containt when Stringformat method called", function() {		
		var url = 'http://localhost:43423/AccessLink/api/UnitManager/scenario/{0}';
		var regValue = 'mysearch';
    	expect(stringFormatService.Stringformat(url,regValue)).toEqual('http://localhost:43423/AccessLink/api/UnitManager/scenario/mysearch');
	});	
});