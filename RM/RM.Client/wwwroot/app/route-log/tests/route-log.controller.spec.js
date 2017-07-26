'use strict';
describe('Route-Log: Controller', function () {
    var routeLogService;
    var controller;
    var items;
    var vm;
    var $q;
    var $scope;
    var $rootScope;

    beforeEach(function () {
        module('routeLog'); 
        module(function ($provide) {
            $provide.value('items', { externalId: "0", unitName: "Worthing  Office", unitAddressUDPRN: 2333402, area: "BN", ID: "b51aa229-c984-4ca6-9c12-510187b81050",id: "b51aa229-c984-4ca6-9c12-510187b81050"});             
            $provide.factory('routeLogService', function ($q) {
                function loadSelectionType() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }
                function loadRouteLogStatus() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }
                function closeWindow() { return [] }
                function loadScenario(arg1,arg2) {
                    var loadScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];
                    var deferred = $q.defer();                    
                    deferred.resolve(loadScenarioMockData);                    
                    return deferred.promise;
                }
                function loadDeliveryRoute() {
                    var deferred = $q.defer();

                    return deferred.promise;
                }

                function deliveryRouteChange() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }

                function scenarioChange() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }

                function generateRouteLogSummary(){                    
                    var deferred = $q.defer();
                    deferred.resolve('mainRoutePath.pdf');
                    return deferred.promise;
                }

                function generatePdf(){
                    var deferred = $q.defer();
                    return deferred.promise;
                }

                return {
                    loadSelectionType: loadSelectionType,
                    loadRouteLogStatus: loadRouteLogStatus,
                    closeWindow: closeWindow,
                    loadScenario: loadScenario,
                    loadDeliveryRoute: loadDeliveryRoute,
                    deliveryRouteChange : deliveryRouteChange,
                    scenarioChange: scenarioChange,
                    generateRouteLogSummary: generateRouteLogSummary,
                    generatePdf: generatePdf
                };
            });

            $provide.factory('routeLogAPIService', function ($q) {
                function getSelectionType() {
                    var deferred = $q.defer();
                    deferred.resolve(['Single','Multiple']);
                    return deferred.promise;
                }
                function getStatus() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }
                function getScenario() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }
                function getRoutes() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }
                function getRouteDetailsByGUID() {
                    var deferred = $q.defer();
                    return deferred.promise;
                }
                return {
                    getSelectionType: getSelectionType,
                    getStatus: getStatus,
                    getScenario: getScenario,
                    getRoutes: getRoutes,
                    getRouteDetailsByGUID: getRouteDetailsByGUID
                };
            });                    
        });

        inject(function (_$q_,_$controller_, _routeLogService_, _$rootScope_,items) {
            $rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            routeLogService = _routeLogService_;
            $q = _$q_;

            vm = _$controller_('RouteLogController', {
                routeLogService: routeLogService,
                items: items,
                $scope: $scope                
            });
        });       
    });

    it('should be set md-text class is selected', function() {
        expect(vm.selectClass).toContain("routeSearch md-text");
    });
 
    it('Should be close dialog-window once closeWindow method called', function () {
        spyOn(routeLogService, 'closeWindow').and.callThrough();
        vm.closeWindow();
        expect(routeLogService.closeWindow).toHaveBeenCalled();
    });

    it('should be empty searchTerm value once clearSearchTerm method called', function() {      
        vm.clearSearchTerm();       
        expect(vm.searchTerm).toBe('');
    });

    it('should be set selected delivery unit is a object of items service', function() {
        expect(vm.selectedDeliveryUnitObj).toEqual({ externalId: "0", unitName: "Worthing  Office", unitAddressUDPRN: 2333402, area: "BN", ID: "b51aa229-c984-4ca6-9c12-510187b81050",id: "b51aa229-c984-4ca6-9c12-510187b81050"});
    });

    it('should be clear fields on changing selection type', function () {
        vm.selectionTypeChange();
        expect(vm.selectedRouteStatusObj).toBe(null);
        expect(vm.selectedRouteScenario).toBe(null);
        expect(vm.selectedRoute).toBe(null);
        expect(vm.isSelectionType).toBe(false);
        expect(vm.isRouteScenarioDisabled).toBe(true);
        expect(vm.isDeliveryRouteDisabled).toBe(true);
        expect(vm.isShowMultiSelectionRoute).toBe(false);
        expect(vm.deliveryRoute).toBe(null);
        expect(vm.routeDetails).toBe(false);
    });

    it('should be select a route status and clear the delivery route ', function() {
        vm.selectedRouteStatusObj = {id: "b51aa229-c984-4ca6-9c12-510187b81050"};
        vm.selectedRouteStatus();
        $rootScope.$apply();
        
        expect(vm.RouteScenario).toEqual([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);
        expect(vm.isRouteScenarioDisabled).toBe(false);
        expect(vm.deliveryRoute).toBe(null);
        expect(vm.routeDetails).toBe(false);
    });

    it('should promise to return a success response true once loadSelectionType method is called', function () {
        var deferred = $q.defer();
        var loadSelectionTypeMockData = [{"RouteselectionTypeObj":[
                                                            {"referenceDataName":null,"referenceDataValue":"Single","dataDescription":"Single","displayText":null,"dataParentId":null,"id":"ffeb0dbc-d12b-e711-8735-28d244aef9ed","referenceDataCategory_GUID":"5c64cd73-d12b-e711-8735-28d244aef9ed","dataParent_GUID":null},
                                                            {"referenceDataName":null,"referenceDataValue":"Multiple","dataDescription":"Multiple","displayText":null,"dataParentId":null,"id":"01ae2cc6-d12b-e711-8735-28d244aef9ed","referenceDataCategory_GUID":"5c64cd73-d12b-e711-8735-28d244aef9ed","dataParent_GUID":null}],
                                    "selectedRouteSelectionObj":{"referenceDataName":null,"referenceDataValue":"Single","dataDescription":"Single","displayText":null,"dataParentId":null,"id":"ffeb0dbc-d12b-e711-8735-28d244aef9ed","referenceDataCategory_GUID":"5c64cd73-d12b-e711-8735-28d244aef9ed","dataParent_GUID":null}
                                }];

        spyOn(routeLogService,'loadSelectionType').and.returnValue(deferred.promise);
        vm.loadSelectionType();

        deferred.resolve(loadSelectionTypeMockData);       
        $rootScope.$apply();

        expect(routeLogService.loadSelectionType).toHaveBeenCalled();
        expect(vm.RouteselectionTypeObj).toBe(loadSelectionTypeMockData[0].RouteselectionTypeObj);
        expect(vm.selectedRouteSelectionObj.referenceDataValue).toBe(loadSelectionTypeMockData[0].selectedRouteSelectionObj.referenceDataValue);
    });

    it('should promise to return a success response true once selectedRouteStatus method is called', function () {
        vm.selectedRouteStatusObj = {id: "b51aa229-c984-4ca6-9c12-510187b81050"};

        vm.selectedRouteStatus();
        $rootScope.$apply();

        expect(vm.isRouteScenarioDisabled).toBe(false);
        expect(vm.deliveryRoute).toBe(null);
        expect(vm.routeDetails).toBe(false);
        expect(vm.generateSummaryReport).toBe(false);
        expect(vm.RouteScenario).toEqual([{scenarioName: 'Worthing Delivery Office - Baseline weekday', id: 'b51aa229-c984-4ca6-9c12-510187b81050'}]);

    });

    it('should promise to return a success response once loadRouteLogStatus method is called', function () {        
        var deffer = $q.defer();
        var loadRouteLogStatusMockData = [
                                        {"id":"9c1e56d7-5397-4984-9cf0-cd9ee7093c88","name":null,"value":"Live","displayText":null,"description":"Live"},
                                        {"id":"bee6048d-79b3-49a4-ad26-e4f5b988b7ab","name":null,"value":"Not Live","displayText":null,"description":"Not Live"}]; 
        spyOn(routeLogService, 'loadRouteLogStatus').and.returnValue(deffer.promise);

        vm.loadRouteLogStatus();

        deffer.resolve(loadRouteLogStatusMockData);
        $rootScope.$apply();

        expect(vm.RouteStatusObj).toBe(loadRouteLogStatusMockData);
        expect(vm.selectedRouteStatusObj).toBe(loadRouteLogStatusMockData[0]);
        expect(vm.RouteScenario).toEqual([{scenarioName: 'Worthing Delivery Office - Baseline weekday', id: 'b51aa229-c984-4ca6-9c12-510187b81050'}]);
    });

    it('should call loadDeliveryRoute and after clearDeliveryRoute once scenarioChange method called', function() {
        var deffer = $q.defer();
        vm.selectedRouteSelectionObj = {value:"Multiple"};
        vm.selectedRouteStatusObj = { id:'b51aa229-c984-4ca6-9c12-510187b81050'};
        vm.selectedRouteScenario = { id:'9c1e56d7-5397-4984-9cf0-cd9ee7093c88'};

        spyOn(routeLogService,'loadDeliveryRoute').and.returnValue(deffer.promise);
        spyOn(routeLogService,'scenarioChange').and.returnValue({isDeliveryRouteDisabled:true});
        var result = vm.scenarioChange();
            
        deffer.resolve([{"deliveryRoute":[],"multiSelectiondeliveryRoute":[]}]);
        $rootScope.$apply();

        expect(vm.isDeliveryRouteDisabled).toBe(true);
        expect(routeLogService.loadDeliveryRoute).toHaveBeenCalledWith('b51aa229-c984-4ca6-9c12-510187b81050', '9c1e56d7-5397-4984-9cf0-cd9ee7093c88', 'Multiple');
        expect(vm.multiSelectiondeliveryRoute).toEqual([]);
        expect(vm.deliveryRoute).toEqual([]);   
        expect(vm.isShowMultiSelectionRoute).toBe(false);
        expect(vm.isDeliveryRouteDisabled).toBe(true);
        expect(vm.routeDetails).toBe(false);
        expect(vm.generateSummaryReport).toBe(false);
    });

    it('should call loadDeliveryRoute and check `multiSelectiondeliveryRoute` length greater then 1 once scenarioChange method called', function() {
        var deffer = $q.defer();
        vm.selectedRouteSelectionObj = {value:"Multiple"};
        vm.selectedRouteStatusObj = { id:'b51aa229-c984-4ca6-9c12-510187b81050'};
        vm.selectedRouteScenario = { id:'9c1e56d7-5397-4984-9cf0-cd9ee7093c88'};

        spyOn(routeLogService,'loadDeliveryRoute').and.returnValue(deffer.promise);
        var result = vm.scenarioChange();
            
        deffer.resolve([{"deliveryRoute":[],"multiSelectiondeliveryRoute":['9c1e56d7-5397-4984-9cf0-cd9ee7093c88']}]);
        $rootScope.$apply();

        expect(vm.multiSelectiondeliveryRoute).toEqual(['9c1e56d7-5397-4984-9cf0-cd9ee7093c88']);
        expect(vm.deliveryRoute).toBe(null);   
        expect(vm.isShowMultiSelectionRoute).toBe(true);
        expect(vm.isDeliveryRouteDisabled).toBe(true);
    });

    it('should call loadDeliveryRoute and check `deliveryRoute` length 0 once scenarioChange method called', function() {
        var deffer = $q.defer();
        vm.selectedRouteSelectionObj = {value:"Multiple"};
        vm.selectedRouteStatusObj = { id:'b51aa229-c984-4ca6-9c12-510187b81050'};
        vm.selectedRouteScenario = { id:'9c1e56d7-5397-4984-9cf0-cd9ee7093c88'};

        spyOn(routeLogService,'loadDeliveryRoute').and.returnValue(deffer.promise);
        var result = vm.scenarioChange();
            
        deffer.resolve([{"deliveryRoute":[],"multiSelectiondeliveryRoute":[]}]);
        $rootScope.$apply();

        expect(vm.multiSelectiondeliveryRoute).toEqual([]);
        expect(vm.deliveryRoute).toEqual([]);   
        expect(vm.isShowMultiSelectionRoute).toBe(false);
        expect(vm.isDeliveryRouteDisabled).toBe(true);
    });

    it('should promise to return a success response once loadScenario method is called', function () {        
        var deffer = $q.defer();
        var loadScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];        

        spyOn(routeLogService, 'loadScenario').and.returnValue(deffer.promise);

        vm.loadScenario();

        deffer.resolve(loadScenarioMockData);
        $rootScope.$apply();

        expect(routeLogService.loadScenario).toHaveBeenCalled();
        expect(vm.RouteScenario).toEqual([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);
    });

    it('should promise to return a empty response once loadScenario method is called', function () {        
        var deffer = $q.defer();
        var response = [];
        spyOn(routeLogService, 'loadScenario').and.returnValue(deffer.promise);

        vm.loadScenario();

        deffer.resolve(response);
        $rootScope.$apply();

        expect(routeLogService.loadScenario).toHaveBeenCalled();
        expect(vm.RouteScenario).toBeUndefined();
        expect(vm.selectedRouteScenario).toBe(null);
        expect(vm.isSelectionType).toBe(true);
        expect(vm.selectedRoute).toBe(null);
        expect(vm.isDeliveryRouteDisabled).toBe(true);
        expect(vm.isShowMultiSelectionRoute).toBe(false);
    });    

    it('should promise to return a success response once deliveryRouteChange method is called', function () {        
        var deffer = $q.defer();
        var deliveryRouteChangeMockData = {"id":"33bc42ed-42bd-4456-a3f6-33a10efc8bd3","travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"deliveryOffice":undefined};        
        var selectedRouteValue = {id:'33bc42ed-42bd-4456-a3f6-33a10efc8bd3'};

        spyOn(routeLogService, 'deliveryRouteChange').and.returnValue(deffer.promise);

        vm.deliveryRouteChange(selectedRouteValue);

        deffer.resolve(deliveryRouteChangeMockData);
        $rootScope.$apply();

        expect(routeLogService.deliveryRouteChange).toHaveBeenCalledWith('33bc42ed-42bd-4456-a3f6-33a10efc8bd3');
        expect(vm.routeDetails.deliveryOffice).toEqual('');
        expect(vm.generateSummaryReport).toEqual(false);
    }); 

    it('should promise to return a empty response once generateRouteLogSummary method is called', function () {
        vm.generateSummaryReport = true;
        var deffer = $q.defer();           
        spyOn(routeLogService, 'generatePdf').and.returnValue(deffer.promise);
        spyOn(routeLogService, 'generateRouteLogSummary').and.returnValue(deffer.promise);

        vm.generateRouteLogSummary(true);

        deffer.resolve('mainRoutePath.pdf');
        $rootScope.$apply();

        expect(routeLogService.generateRouteLogSummary).toHaveBeenCalled();
        expect(routeLogService.generatePdf).toHaveBeenCalledWith('mainRoutePath.pdf');    
    });
     
});
