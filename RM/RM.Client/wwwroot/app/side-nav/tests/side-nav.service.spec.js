'use strict';
describe('SideNav: Service', function() {
	var CommonConstants;
    var roleAccessService;
    var store = {};
    var mockLocalStorage = {
        getItem: function(key) { return store[key]; }
    };

	beforeEach(function() {
        module('sideNav');
        module(function ($provide) {
            $provide.value('CommonConstants', {GetSessionStorageItemType:'roleAccessData'}); 
            $provide.factory('roleAccessService',function(){
                return {
                    fetchActionItems: function(){}
                }
            });                      
        });

        inject(function(_sideNavService_,_CommonConstants_){                              
            sideNavService = _sideNavService_;
        });        
    });

	it('should return get item data when fetchActionItems method called', function() {
        spyOn(sideNavService,'fetchActionItems').and.callFake(function(key){
            return {token:'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ',unitGuid:'092c69ae-4382-4183-84ff-ba07543d9c75',username: 'shobharam.katiya'};
        });
		expect(sideNavService.fetchActionItems()).toEqual({token:'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ',unitGuid:'092c69ae-4382-4183-84ff-ba07543d9c75',username: 'shobharam.katiya'});
	});
});