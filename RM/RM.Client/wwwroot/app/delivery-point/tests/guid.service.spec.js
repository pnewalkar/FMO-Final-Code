'use strict';
describe('Delivery Point : Guid Service', function() {
	var guidService;    
    beforeEach(function() {
    	module('deliveryPoint');
        inject(function(_guidService_) {
            guidService = _guidService_;
        });
    });    

    it("should be get updated value when getGuid method called", function() {
    	guidService.setGuid('47717a64-7bfe-4c86-a25c-39f84c8a069c');
    	expect(guidService.getGuid()).toBe('47717a64-7bfe-4c86-a25c-39f84c8a069c');
	});	
});