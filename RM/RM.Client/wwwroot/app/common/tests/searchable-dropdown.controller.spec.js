'use strict';
describe('Common: Searchable Dropdown Controller', function() {
	var vm;
	var $scope;
	var $rootScope;
	var $state;

	beforeEach(function() {
		module('reusableComponent');
		module(function($provide){
			$provide.value('$state',{});
		});
		inject(function(_$rootScope_,_$controller_){
			$rootScope = _$rootScope_;
			$scope = _$rootScope_.$new();

			vm = _$controller_('CustomSearchableDropdownController',{
				$scope: $scope
			});
		});	
	});

	it('should clear search text once clearSearchTerm method called', function() {
		vm.clearSearchTerm();
		expect(vm.searchTerm).toBe('');
	});	
});