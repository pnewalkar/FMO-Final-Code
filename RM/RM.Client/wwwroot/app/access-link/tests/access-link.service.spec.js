'use strict';
describe('Access LInk : Service', function() {
	var accessLinkService;    
    beforeEach(function() {
    	module('accessLink');
        inject(function(_accessLinkService_) {
            accessLinkService = _accessLinkService_;
        });
    });

    it("should be return a object in accessLink", function() {
    	var obj = {
            templateUrl: './access-link/acccess-link.template.html',
            clickOutsideToClose: false,
            controller: "AccessLinkController as vm"
        };

    	expect(accessLinkService.accessLink()).toEqual(obj);
	});
});	