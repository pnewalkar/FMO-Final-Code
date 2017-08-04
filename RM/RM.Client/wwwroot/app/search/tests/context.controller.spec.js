'use strict';
describe('Search: Context Controller', function() {
	var $scope;
	var vm;
	var $rootScope;
	var $state;
	var $stateParams;

	beforeEach(function() {
		module('search');
		module(function($provide){
			$provide.value('$stateParams',{selectedItem:true});
			$provide.value('$state',{});
		});

		inject(function(_$controller_,_$rootScope_,_$state_,_$stateParams_){
			$rootScope = _$rootScope_;
			$scope = _$rootScope_.$new();
			$state = _$state_;
			$stateParams = _$stateParams_;

			vm = _$controller_('ContextController',{
				$scope: $scope,
				$state: $state,
				$stateParams: $stateParams
			});
		});
	});

	it('should set `selectedItems` value as `$stateParams`', function() {
		expect(vm.selectedItem).toEqual(true);
	});

	it('should set showDeliveryPoint value as empty ', function() {
		expect(vm.showDeliveryPoint).toEqual('');
	});

});