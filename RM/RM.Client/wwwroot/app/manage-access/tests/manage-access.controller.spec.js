'use strict';
describe('Manage Access: Controller', function() {
	var vm;	
	var manageAccessService;

	beforeEach(function() {
		module('manageAccess');
		module(function($provide){
			$provide.factory('manageAccessService',function(){
				return {activate: function(){}}
			});
		});

		inject(function(_$controller_,_manageAccessService_){
			manageAccessService = _manageAccessService_;

			vm = _$controller_('ManageAccessController',{
				manageAccessService: manageAccessService
			});
		});		
	});

	it('should call `manageAccessService.activate` method called once activate method called', function() {
		spyOn(manageAccessService,'activate');

		vm.activate(null);
		expect(manageAccessService.activate).toHaveBeenCalledWith(null);
	});	
});