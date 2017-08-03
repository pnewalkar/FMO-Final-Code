'use strict';
describe('Security: authInterceptorService', function() {
	var $q;
	var $injector;
	var sessionStorage;
	var authInterceptorService;

	beforeEach(function() {
		module('RMApp');
		module(function($provide){
			$provide.factory('$injector', function(){
				return {
					get: function(){}
				}
			});			
		});

		inject(function(_authInterceptorService_,_$injector_){
			authInterceptorService = _authInterceptorService_;
			$injector = _$injector_;
		});
	});

	it('should return config request ', function() {

		authInterceptorService._responseError({status:401});


	});
});