'use strict';
describe('Common: Error Service', function() {	
	var $rootScope;
	var errorService;
	var $mdDialog;
	var $timeout;
	var $translate;
	var $q;

	beforeEach(function() {		
        module('error'); 
        module(function($provide){
        	$provide.factory('$mdDialog', function($q) {        		
                return {          
                	show: function(){
                		var deffer = $q.defer();
                		return deffer.promise;
                	},   	
	                alert: function(){
	                	return {
	                		parent: function(){
	                			return {
	                				clickOutsideToClose: function(){
	                					return {
	                						title: function(){
	                							return {
	                								textContent: function(){
	                									return {
	                										ariaLabel: function(){
	                											return {
	                												css: function(){
	                													return {
	                														ok: function(){}
	                													}
	                												}
	                											}
	                										}
	                									}
	                								}
	                							}
	                						}
	                					}
	                				}
	                			}
	                		}
	                	}
	                }
                }                      
            });
            $provide.factory('$translate', function() {  
            	return {instant: jasmine.createSpy()};              
            });
        });  

		inject(function(_errorService_,_$mdDialog_, _$rootScope_, _$translate_, _$timeout_,_$q_){
			errorService = _errorService_;
			$rootScope = _$rootScope_;
			$mdDialog = _$mdDialog_;
			$translate = _$translate_;
			$timeout = _$timeout_;
			$q = _$q_;
		});

		spyOn($rootScope, '$broadcast').and.callThrough();
	});

	it("should call show alert once openAlert method called", function() {		
		var deferred = $q.defer();
		spyOn($mdDialog,'show').and.returnValue(deferred.promise);

		errorService.openAlert('Error!');
		deferred.resolve();
        $rootScope.$apply();

		expect($rootScope.$broadcast).toHaveBeenCalledWith('errorClosed');
		expect($mdDialog.show).toHaveBeenCalled();		
	});
});