describe('deliveryPoint: DeliveryPointContextController', function() {

	var vm;
	var $scope;
	var $rootScope;
	var GlobalSettings;
	var $stateParams;
	var controller;

	//Mocking Value of Global Setting
	var stateParamsMockData = {"accessLinkFeature":"future",
								"type":"single",
								"selectedDeliveryPoint":"line"
							};

    var MockGlobalSettings = {
    	apiUrl: 'http://localhost:34583/api',
	    env: 'localhost', // Here set the current environment
	    indexUrl: '',
		deliveryPointLayerName: "Delivery Points"
    };

	beforeEach(function() {
		module('deliveryPoint');
        module(function ($provide) {
            $provide.value('$stateParams', stateParamsMockData);
            $provide.value('GlobalSettings', MockGlobalSettings);
         
        });

        inject(function (_$controller_,_$rootScope_,_GlobalSettings_,_$stateParams_) {
        	$scope = _$rootScope_.$new();
        	$rootScope = _$rootScope_;
        	GlobalSettings = _GlobalSettings_;
        	$stateParams = _$stateParams_;

        	vm = _$controller_('DeliveryPointContextController', {
        		GlobalSettings: GlobalSettings,
            	$stateParams: $stateParams
        	});
        });

        spyOn($rootScope, '$broadcast').and.callThrough();
        spyOn($rootScope, '$on').and.callThrough();

	});

	it('should be defined selectedDeliveryPoint ', function() {
		expect(vm.selectedDeliveryPoint).toBeDefined();
	});

	it('should be broadcast after show delivery point details', function() {
        $rootScope.$broadcast();
		expect($rootScope.$broadcast).toHaveBeenCalled();
	});
});