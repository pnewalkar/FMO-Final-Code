'use strict';
describe('Manage Access: Service', function() {
	var vm = {};
	var manageAccessService;
	var manageAccessAPIService;
	var GlobalSettings;
	var sessionStorage;
	var localStorage;
	var $rootScope;
	var $window;
	var $q;
	var MockWindow = {
	    location: {
	        search: {
	        	indexOf: jasmine.createSpy(),
	        	slice: function(){
	        		return {
	        			split: function(){
	        				return {
	        					length: 1
	        				}
	        			}
	        		}
	        	}  	
	        }
	    }
	};


	beforeEach(function() {
		module('manageAccess');
		module(function($provide){
			$provide.value('GlobalSettings', {indexUrl:'http://localhost:51978/app/index.html'});
			$provide.value('$window', MockWindow);
			$provide.value(angular,{fromJson:function(){}});
			$provide.factory('sessionStorage',function(){
				var store = {};
				return {
					getItem: function(key){
				      return store[key];
				    },
				    setItem: function(key,value){
				      store[key] = `${value}`;
				    },
				    removeItem: function(key) {
				      delete store[key];
				    },
				    clear: function() {
				      store = {};
				    }
				}
			});

			$provide.factory('manageAccessAPIService',function($q){
				return {
					getToken: function(userdata){
						var deffer = $q.defer();
						//deffer.resolve();
						return deffer.promise;
					}
				}
			});
		});
		
		inject(function(
			_$rootScope_,
			_sessionStorage_,
			_$window_,			
			_GlobalSettings_,
			_manageAccessService_,
			_manageAccessAPIService_){

			$rootScope = _$rootScope_;
			$window = _$window_;
			GlobalSettings = _GlobalSettings_;
			manageAccessService = _manageAccessService_;
			manageAccessAPIService = _manageAccessAPIService_;
			sessionStorage = _sessionStorage_;
		});


	   

      	/*spyOn(sessionStorage, 'setItem').and.callFake(function(key, value){
        	store[key] = value;
      	});

      	spyOn(sessionStorage, 'removeItem').and.callFake(function (key) {
        	delete store[key];
      	});	*/   
	});

	it('should set userdata object once activate method called', function() {		
		spyOn(sessionStorage, 'getItem').and.callFake(function(key){
        	return {token:'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ',unitGuid:'092c69ae-4382-4183-84ff-ba07543d9c75',username: 'shobharam.katiya'};
      	});
		spyOn(angular,'fromJson').and.returnValue({username:'shobharam.katiy'});
		
		manageAccessService.activate('092c69ae-4382-4183-84ff-ba07543d9c75');	

		expect(vm.userdata).toEqual('');

	});
	
	xit('should return url parameters', function() {
		spyOn($window.location.search.slice(),'split').and.returnValue(["username=shobharam"]);
		expect(manageAccessService.getParameterValues('username')).toEqual('shobharam');
	});
});

