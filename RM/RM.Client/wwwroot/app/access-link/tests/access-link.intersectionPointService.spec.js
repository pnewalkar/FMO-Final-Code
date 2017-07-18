'use strict';
describe('Access Link : Intersection Point Service', function() {
	var intersectionPointService;
    beforeEach(function() {
    	module('accessLink');
        inject(function(_intersectionPointService_) {
            intersectionPointService = _intersectionPointService_;
        });
    });

    it("should be get updated value when getIntersectionPoint method called", function() {
    	intersectionPointService.setIntersectionPoint(1.11);
    	expect(intersectionPointService.getIntersectionPoint()).toBe(1.11);
	});	
});