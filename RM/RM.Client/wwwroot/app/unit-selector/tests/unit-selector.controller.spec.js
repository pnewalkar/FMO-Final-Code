'use strict';
describe('Unit Selector: Controller', function() {
	var vm;
	var $scope;
	var $rootScope;
	var unitSelectorService;
	var licensingInfoService;
	var $q;

	beforeEach(function() {
		module('unitSelector');
		module(function($provide){
			$provide.factory('unitSelectorService',function($q){
				function DeliveryUnit(){
					var deffer = $q.defer();
					return deffer.promise;
				}
				function BindData(){
					var deffer = $q.defer();
					return deffer.promise;
				}				
				return {
					DeliveryUnit: DeliveryUnit,
					BindData: BindData,
				}
			});

			$provide.factory('licensingInfoService',function(){
				function getLicensingInfo(){
					var deffer = $q.defer();
					return deffer.promise;
				}
				return {
					getLicensingInfo: getLicensingInfo,
				}
			});
		});

		inject(function(
			_$rootScope_,
			_$controller_,
			_unitSelectorService_,
			_licensingInfoService_,
			_$q_){			

			$rootScope = _$rootScope_;
			$scope = _$rootScope_.$new();
			unitSelectorService = _unitSelectorService_;
			licensingInfoService = _licensingInfoService_;
			$q = _$q_;

			vm = _$controller_('UnitSelectorController',{
				unitSelectorService: unitSelectorService,
				$scope: $scope,
				licensingInfoService: licensingInfoService
			});
		});
	});

	it('shoule set default selectedDeliveryUnit value as `null`', function() {
		expect(vm.selectedDeliveryUnit).toBe(null);
	});

	it('shoule set default deliveryRouteUnit value as `[]` ', function() {
		expect(vm.deliveryRouteUnit).toEqual([]);
	});

	it('shoule set default isDeliveryUnitDisabled value as `false`', function() {
		expect(vm.isDeliveryUnitDisabled).toBe(false);
	});

	it('should be set md-text class is selected', function() {
        expect(vm.selectClass).toContain("routeSearch md-text");
    });

    it('should call `unitSelectorService.DeliveryUnit` method when  DeliveryUnit method called', function () {
    	var selectedUser = {"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","area":"BN"};
    	spyOn(unitSelectorService,'DeliveryUnit');
    	vm.DeliveryUnit(selectedUser);

    	expect(unitSelectorService.DeliveryUnit).toHaveBeenCalledWith({"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","area":"BN"});
    });

    it('should promise to return a success response once BindData method is called', function () {
    	var deffer = $q.defer();    	
    	var MockBindData = [{deliveryRouteUnit:[{"displayText":"BT    National NI","ID":"38fd2404-d65b-e711-80e2-000d3a22173b","icon":"fa-map-marker delivery","area":"BT"}],
    						selectedUser:[{"displayText":"BT    National NI","ID":"38fd2404-d65b-e711-80e2-000d3a22173b","icon":"fa-map-marker delivery","area":"BT"}],
    						isDeliveryUnitDisabled:false}];
        spyOn(unitSelectorService, 'BindData').and.returnValue(deffer.promise);
        spyOn(licensingInfoService,'getLicensingInfo');
    	vm.BindData();

    	deffer.resolve(MockBindData);
        $rootScope.$apply();

        expect(unitSelectorService.BindData).toHaveBeenCalledWith([]);
        expect(vm.deliveryRouteUnit).toEqual([{"displayText":"BT    National NI","ID":"38fd2404-d65b-e711-80e2-000d3a22173b","icon":"fa-map-marker delivery","area":"BT"}]);
        expect(vm.selectedUser).toEqual([{"displayText":"BT    National NI","ID":"38fd2404-d65b-e711-80e2-000d3a22173b","icon":"fa-map-marker delivery","area":"BT"}]);
        expect(vm.selectedDeliveryUnit).toEqual([{"displayText":"BT    National NI","ID":"38fd2404-d65b-e711-80e2-000d3a22173b","icon":"fa-map-marker delivery","area":"BT"}]);               
        expect(licensingInfoService.getLicensingInfo).toHaveBeenCalled();
    });
});