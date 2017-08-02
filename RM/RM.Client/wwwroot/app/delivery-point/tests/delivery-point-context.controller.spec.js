'use strict';
describe('Delivery Point: Context Controller', function() {
	var vm;
	var $rootScope;
	var GlobalSettings;
	var $stateParams;
	var CommonConstants;
	var selectedDeliveryPointService;
	
	beforeEach(function() {
		module('deliveryPoint');
        module(function ($provide) {
        	$provide.value('CommonConstants', {DetailsOfDeliveryPoint: 'Details of Delivery Point'});
        	$provide.value('GlobalSettings', {});
            $provide.factory('selectedDeliveryPointService',function(){
            	return {
            		getSelectedDeliveryPoint: function(){} 
            	}
            });         
        });

        inject(function (_$controller_,_$rootScope_,_selectedDeliveryPointService_,_CommonConstants_,_GlobalSettings_) {
        	$rootScope = _$rootScope_;
        	CommonConstants = _CommonConstants_;
        	selectedDeliveryPointService = _selectedDeliveryPointService_;
        	GlobalSettings = _GlobalSettings_;

        	vm = _$controller_('DeliveryPointContextController', {
            	$stateParams: $stateParams,
            	CommonConstants: CommonConstants,
            	selectedDeliveryPointService: selectedDeliveryPointService,
            	GlobalSettings: GlobalSettings
        	});
        });

        //spyOn($rootScope, '$broadcast').and.callThrough();
        //spyOn($rootScope, '$on').and.callThrough();
        //$rootScope.$broadcast('showDeliveryPointDetails',{ contextTitle:'mapLayer',featureType:'line'});
	});

	it('should set others if selectedDeliveryPoint is null', function() {	
		expect(vm.selectedDeliveryPointType).toEqual('others');
	});		
});