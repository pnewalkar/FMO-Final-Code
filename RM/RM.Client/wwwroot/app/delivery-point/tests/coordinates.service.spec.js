'use strict';
describe('deliveryPoint: Service', function() {

	beforeEach(module('deliveryPoint'));

	describe('deliveryPoint: coordinatesService', function() {

		var coordinatesService;
		var getCordData;
		var coordinates = '';
	    
	    beforeEach(function() {
	        inject(function(_coordinatesService_) {
	            coordinatesService = _coordinatesService_;
	        });

	        spyOn(coordinatesService, "getCordinates").and.returnValue(745);
			coordinatesService.setCordinates(123);
		    getCordData = coordinatesService.getCordinates();

	    });

	    it("should be called getCordinates", function() {
	    	expect(coordinatesService.getCordinates).toHaveBeenCalled();
		});

		it("should be set value setCordinates", function() {
			spyOn(coordinatesService,'setCordinates');
			coordinatesService.setCordinates();
			expect(coordinatesService.setCordinates).toHaveBeenCalled();

		});

		it("should be return getCordinates", function() {
		    expect(getCordData).toEqual(745);
		});

	});

	describe('deliveryPoint: guidService', function() {

		var guidService;
		var getCordData;
		var guid = '';
	    
	    beforeEach(function() {
	        inject(function(_guidService_) {
	            guidService = _guidService_;
	        });

	        spyOn(guidService, "getGuid").and.returnValue(745);
			guidService.setGuid(123);
		    getCordData = guidService.getGuid();

	    });

	    it("should be called getGuid", function() {
	    	expect(guidService.getGuid).toHaveBeenCalled();
		});

		it("should be set value setGuid", function() {
			spyOn(guidService,'setGuid');
			guidService.setGuid();
			expect(guidService.setGuid).toHaveBeenCalled();

		});

		it("should be return getGuid", function() {
		    expect(getCordData).toEqual(745);
		});

	});

});