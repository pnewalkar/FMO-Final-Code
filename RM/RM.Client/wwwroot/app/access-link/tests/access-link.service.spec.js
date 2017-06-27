'use strict';
describe('accessLink: Service', function() {

	beforeEach(module('accessLink'));

	describe('accessLink: accessLinkCoordinatesService', function() {

		var accessLinkService;
	    
	    beforeEach(function() {
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

	    	expect(accessLinkService.accessLink()).toEqual(jasmine.any(Object));
	    	expect(accessLinkService.accessLink()).toEqual(obj);
		});

	});

	describe('accessLink: accessLinkCoordinatesService', function() {

		var accessLinkCoordinatesService;
		var getCordData;
		var coordinates = '';
	    
	    beforeEach(function() {
	        inject(function(_accessLinkCoordinatesService_) {
	            accessLinkCoordinatesService = _accessLinkCoordinatesService_;
	        });

	        spyOn(accessLinkCoordinatesService, "getCordinates").and.returnValue(745);
			accessLinkCoordinatesService.setCordinates(123);
		    getCordData = accessLinkCoordinatesService.getCordinates();

	    });

	    it("should be called getCordinates", function() {
	    	expect(accessLinkCoordinatesService.getCordinates).toHaveBeenCalled();
		});

		it("should be set value setCordinates", function() {
			spyOn(accessLinkCoordinatesService,'setCordinates');
			accessLinkCoordinatesService.setCordinates();
			expect(accessLinkCoordinatesService.setCordinates).toHaveBeenCalled();

		});

		it("should be return getCordinates", function() {
		    expect(getCordData).toEqual(745);
		});

	});

	describe('accessLink: roadLinkGuidService', function() {

		var roadLinkGuidService;
		var roadLinkGuid = '';
		var getRoadLinkData;
	    
	    beforeEach(function() {

	        inject(function(_roadLinkGuidService_) {
	            roadLinkGuidService = _roadLinkGuidService_;
	        });

	        spyOn(roadLinkGuidService, "getRoadLinkGuid").and.returnValue(745);
			roadLinkGuidService.setRoadLinkGuid(123);
		    getRoadLinkData = roadLinkGuidService.getRoadLinkGuid();

	    });

	    it("should be called getCordinates", function() {
	    	expect(roadLinkGuidService.getRoadLinkGuid).toHaveBeenCalled();
		});

		it("should be set value setCordinates", function() {
			spyOn(roadLinkGuidService,'setRoadLinkGuid');
			roadLinkGuidService.setRoadLinkGuid();
			expect(roadLinkGuidService.setRoadLinkGuid).toHaveBeenCalled();

		});

		it("should be return getCordinates", function() {
		    expect(getRoadLinkData).toEqual(745);
		});

	});

	describe('accessLink: intersectionPointService', function() {

		var intersectionPointService;
		var getIntData;
		var intersectionPoint = '';
	    
	    beforeEach(function() {
	        inject(function(_intersectionPointService_) {
	            intersectionPointService = _intersectionPointService_;
	        });

	        spyOn(intersectionPointService, "getIntersectionPoint").and.returnValue(745);
			intersectionPointService.setIntersectionPoint(123);
		    getIntData = intersectionPointService.getIntersectionPoint();

	    });

	    it("should be called getCordinates", function() {
	    	expect(intersectionPointService.getIntersectionPoint).toHaveBeenCalled();
		});

		it("should be set value setCordinates", function() {
			spyOn(intersectionPointService,'setIntersectionPoint');
			intersectionPointService.setIntersectionPoint();
			expect(intersectionPointService.setIntersectionPoint).toHaveBeenCalled();

		});

		it("should be return getCordinates", function() {
		    expect(getIntData).toEqual(745);
		});

	});
});