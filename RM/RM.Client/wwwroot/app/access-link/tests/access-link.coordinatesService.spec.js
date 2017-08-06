'use strict';
describe('Access Link : Coordinates Service', function() {
	var accessLinkCoordinatesService;    
    beforeEach(function() {
    	module('accessLink');
        inject(function(_accessLinkCoordinatesService_) {
            accessLinkCoordinatesService = _accessLinkCoordinatesService_;
        });
    });   

    it("should be get updated value when getCordinates method called", function() {
    	accessLinkCoordinatesService.setCordinates('-7,3');
    	expect(accessLinkCoordinatesService.getCordinates()).toBe('-7,3');
	});	
});