'use strict';
describe('Common: Error Interceptor Service', function() {	
	var errorInterceptorService;
	var $rootScope;
	var $q;
	var $translate;

	beforeEach(function() {		
        module('home');        
		module('RMApp');		
		inject(function(_$rootScope_,_errorInterceptorService_,_$q_,_$translate_){
			errorInterceptorService = _errorInterceptorService_;
			$rootScope = _$rootScope_;
			$q = _$q_;
			$translate = _$translate_;
		});		
		spyOn($rootScope, '$broadcast').and.callThrough();
	});

	it("should return promise reject response translate error message once responseError method called", function() {					
		var rejection = {data:{message:'GENERAL.ERRORS.UNKNOWN'}};        
		var response;
    	errorInterceptorService.responseError(rejection);

    	expect($rootScope.$broadcast).toHaveBeenCalledWith('showError', 'GENERAL.ERRORS.UNKNOWN');
	});
	
	it("should return promise reject response translate error message once requestError method called", function() {					
		var rejection = {data:{message:'GENERAL.ERRORS.UNKNOWN'}};        
		var response;
    	errorInterceptorService.requestError(rejection);

    	expect($rootScope.$broadcast).toHaveBeenCalledWith('showError', 'GENERAL.ERRORS.UNKNOWN');
	});
});