'use strict';
describe('Access Link : Road Link Guid Service', function() {
	var roadLinkGuidService;	    
    beforeEach(function() {
    	module('accessLink');
        inject(function(_roadLinkGuidService_) {
            roadLinkGuidService = _roadLinkGuidService_;
        });
    });   

    it("should be get updated value when getRoadLinkGuid method called", function() {
    	roadLinkGuidService.setRoadLinkGuid('47717a64-7bfe-4c86-a25c-39f84c8a069c');
    	expect(roadLinkGuidService.getRoadLinkGuid()).toBe('47717a64-7bfe-4c86-a25c-39f84c8a069c');
	});	
});