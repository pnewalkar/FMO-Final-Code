'use strict';
describe('Delivery Point : Coordinates Service', function() {
	var coordinatesService;    
    beforeEach(function() {
    	module('deliveryPoint');
        inject(function(_coordinatesService_) {
            coordinatesService = _coordinatesService_;
        });
    });
    
    it("should be get updated value when getCordinates method called", function() {
    	coordinatesService.setCordinates('3,4');
    	expect(coordinatesService.getCordinates()).toBe('3,4');
	});	
});