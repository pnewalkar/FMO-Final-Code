'use strict';
describe('Common: popUpSettingService', function() {	
	var popUpSettingService;
	beforeEach(function() {		
        module('home');        
		module('RMApp');
		inject(function(_popUpSettingService_){
			popUpSettingService = _popUpSettingService_;
		})
	});

	it("should return advanceSearch object", function() {		
    	expect(popUpSettingService.advanceSearch(false)).toEqual({templateUrl: './advance-search/advance-search.template.html',clickOutsideToClose: false,controller: 'AdvanceSearchController as vm',locals : {searchText : false}});
	});

	it("should return routeLog object", function() {    	
        expect(popUpSettingService.routeLog(false)).toEqual({templateUrl: './route-log/route-log.template.html',clickOutsideToClose: false,locals: {items: false},controller: 'RouteLogController as vm'});
	});

	xit("should return deliveryPoint object", function() {    	
    	expect(popUpSettingService.deliveryPoint(false)).toEqual({ controller: '', templateUrl: './delivery-point/delivery-point.template.html', clickOutsideToClose: false, controllerAs: 'vm', preserveScope: true });
	});

	it("should return printMap object", function() {
    	expect(popUpSettingService.printMap()).toEqual({templateUrl: './print-map/print-map.template.html',clickOutsideToClose: false,controller: 'PrintMapController as vm'});
	});

	it("should return openAlert object", function() {    	
    	expect(popUpSettingService.openAlert('sucessfully')).toEqual({templateUrl: './common/error/error-popup.html',clickOutsideToClose: false,controller: 'ErrorController as vm',locals: {message: 'sucessfully'}});
	});
});