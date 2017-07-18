'use strict';
describe('Simulation: Controller', function () {
    var vm;
    var $scope;
    var $rootScope;
    var $q;
    var $stateParams;
    var simulationService;    
    var referencedataApiService;
    var referenceDataConstants;
    var mockReferenceDataConstants;
    var stateParamsMockData = {"selectedUnit":{"displayText":"BN    Worthing  Office","ID":"b51aa229-c984-4ca6-9c12-510187b81050","icon":"fa-map-marker delivery","$$mdSelectId":1,"$$hashKey":"object:114"}};
    
    beforeEach(function () {
        module('simulation'); 
        module(function ($provide) {
            $provide.value('$stateParams', stateParamsMockData);                     
            $provide.constant('referenceDataConstants', { SenarioOperationState: { DBCategoryName: "Operational Status" }});
            $provide.factory('simulationAPIService', function ($q) {
                function getScenario(operationStateID, deliveryUnitID) {
                    var deferred = $q.defer();
                    deferred.resolve([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);                   
                    return deferred.promise;
                }

                function getRoutes(operationStateID, deliveryScenarioID) {
                    var deferred = $q.defer();
                    deferred.resolve([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);
                    return deferred.promise;
                }
                return {
                    getScenario: getScenario,
                    getRoutes: getRoutes
                };
            });      

            $provide.factory('referencedataApiService', function ($q) {     
                function getSimpleListsReferenceData(){
                    var deferred = $q.defer();
                    return deferred.promise;                    
                }           
                return {
                    getSimpleListsReferenceData: getSimpleListsReferenceData
                };
            });
        });

        inject(function(
            _$rootScope_,
            _$controller_,
            _simulationService_,
            _$stateParams_,
            _$q_,
            _referenceDataConstants_,
            _referencedataApiService_) {
            
            $rootScope = _$rootScope_;
            $scope = _$rootScope_.$new();
            $q = _$q_;            
            simulationService = _simulationService_;
            referenceDataConstants = _referenceDataConstants_;
            referencedataApiService = _referencedataApiService_;

            vm = _$controller_('SimulationController', {
                simulationService: simulationService,
                $stateParams: _$stateParams_,
                $scope: $scope,                
            });
        });
    });


    it('should be set md-text class is selected', function() {
        expect(vm.selectClass).toContain("routeSearch md-text");
    });

    it('should be selectedDeliveryUnitObj is `$stateParams`', function() {
        expect(vm.selectedDeliveryUnitObj).toBe(stateParamsMockData);        
    });

    it('should be set isDeliveryRouteDisabled is `true` ', function() {
        expect(vm.isDeliveryRouteDisabled).toBe(true);
    });

    it('should set by default `undefined` selectedRoute value', function() {
        expect(vm.selectedRoute).toBeUndefined();
    });

    it('should be set searchTerm blank when clearSearchTerm called', function() {
        vm.clearSearchTerm();
        expect(vm.searchTerm).toBe("");
    });

    it('should promise to return a success response once loadRouteLogStatus method is called', function() {
        var deffer = $q.defer();
        var loadRouteLogStatusMockData = [{"id":"9c1e56d7-5397-4984-9cf0-cd9ee7093c88","name":null,"value":"Live","displayText":null,"description":"Live"},{"id":"bee6048d-79b3-49a4-ad26-e4f5b988b7ab","name":null,"value":"Not Live","displayText":null,"description":"Not Live"}];        
        spyOn(simulationService,'loadRouteLogStatus').and.returnValue(deffer.promise);               

        vm.loadRouteLogStatus();
        
        deffer.resolve(loadRouteLogStatusMockData);
        $rootScope.$apply();

        expect(simulationService.loadRouteLogStatus).toHaveBeenCalled();
        expect(vm.RouteStatusObj).toEqual([{"id":"9c1e56d7-5397-4984-9cf0-cd9ee7093c88","name":null,"value":"Live","displayText":null,"description":"Live"},{"id":"bee6048d-79b3-49a4-ad26-e4f5b988b7ab","name":null,"value":"Not Live","displayText":null,"description":"Not Live"}]);
        expect(vm.selectedRouteStatusObj).toEqual('9c1e56d7-5397-4984-9cf0-cd9ee7093c88');        
        expect(vm.RouteScenario).toEqual([{ scenarioName: 'Worthing Delivery Office - Baseline weekday', id: 'b51aa229-c984-4ca6-9c12-510187b81050' }]);
    });   

    it('should call loadDeliveryRoute once scenarioChange method called', function() {
        var deffer = $q.defer();
        vm.selectedRouteScenario = {id:"9c1e56d7-5397-4984-9cf0-cd9ee7093c88"};
        spyOn(simulationService,'loadDeliveryRoute').and.returnValue(deffer.promise);               

        vm.scenarioChange();
        expect(vm.isDeliveryRouteDisabled).toBe(false);
        expect(simulationService.loadDeliveryRoute).toHaveBeenCalled();
    });

    it('should promise to return a success response once loadScenario method is called', function() {
        var selectedRouteStatusObj = "9c1e56d7-5397-4984-9cf0-cd9ee7093c88";
        var selectedDeliveryUnitID = "b51aa229-c984-4ca6-9c12-510187b81050";
        var loadScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];
        var deffer = $q.defer();
        spyOn(simulationService,'loadScenario').and.returnValue(deffer.promise);               

        vm.loadScenario(selectedRouteStatusObj,selectedDeliveryUnitID);
        
        deffer.resolve(loadScenarioMockData);
        $rootScope.$apply();

        expect(simulationService.loadScenario).toHaveBeenCalled();
        expect(vm.RouteScenario).toEqual([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);
    });

    it('should promise to return a empty response once loadScenario method is called', function() {
        var selectedRouteStatusObj = "9c1e56d7-5397-4984-9cf0-cd9ee7093c88";
        var selectedDeliveryUnitID = "b51aa229-c984-4ca6-9c12-510187b81050";
        var loadScenarioMockData = [];
        var deffer = $q.defer();
        spyOn(simulationService,'loadScenario').and.returnValue(deffer.promise);               

        vm.loadScenario(selectedRouteStatusObj,selectedDeliveryUnitID);
        
        deffer.resolve(loadScenarioMockData);
        $rootScope.$apply();

        expect(simulationService.loadScenario).toHaveBeenCalled();
        expect(vm.RouteScenario).toEqual([]);
        expect((vm.deliveryRoute)).toBeUndefined();
        expect(vm.selectedRoute).toBe(null);
        expect(vm.selectedRouteScenario).toBe(null);        
    });    
});