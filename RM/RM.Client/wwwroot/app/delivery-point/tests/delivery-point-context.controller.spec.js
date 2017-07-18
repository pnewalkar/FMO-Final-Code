'use strict';
describe('Delivery Point: Context Controller', function() {
	var vm;
	var $scope;
	var $rootScope;
	var GlobalSettings;
	var $stateParams;
	var controller;
	var stateParamsMockData = {"accessLinkFeature":"future",
								"type":"single",
								"selectedDeliveryPoint":{"type":"line"}
							};    

	beforeEach(function() {
		module('deliveryPoint');
        module(function ($provide) {
            $provide.value('$stateParams', stateParamsMockData);
            $provide.value('GlobalSettings', {deliveryPointDetails: "mapLayer"});         
        });

        inject(function (_$controller_,_$rootScope_,_GlobalSettings_,_$stateParams_) {
        	$scope = _$rootScope_.$new();
        	$rootScope = _$rootScope_;
        	$stateParams = _$stateParams_;

        	vm = _$controller_('DeliveryPointContextController', {
        		GlobalSettings: _GlobalSettings_,
            	$stateParams: $stateParams
        	});
        });

        spyOn($rootScope, '$broadcast').and.callThrough();
        $rootScope.$broadcast('showDeliveryPointDetails',{ contextTitle:'mapLayer',featureType:'line'});
	});

	it('should set `selectedDeliveryPoint` value as `stateparams`', function() {		
		expect(vm.selectedDeliveryPoint).toEqual({type:'line'});
	});

	it('should set selectedDeliveryPointType `line` when stateparams not null', function() {		
		expect(vm.selectedDeliveryPointType).toEqual('line');
	});	

	it('should broadcast event called showDeliveryPointDetails', function() {    		
        expect($rootScope.$broadcast).toHaveBeenCalledWith('showDeliveryPointDetails',{ contextTitle:'mapLayer',featureType:'line'});       
	});
});