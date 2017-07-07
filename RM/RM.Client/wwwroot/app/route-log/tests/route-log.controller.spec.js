'use strict';
describe('Route-Log: Controller', function () {

    var routeLogService;
    var controller;
    var items;
    var vm;
    var $q;
    var deferred;
    var $httpBackend;
    var scope;
    var $rootScope;

    beforeEach(function () {
        module('routeLog'); 
        module(function ($provide) {
            $provide.factory('routeLogService', function ($q) {
                function loadSelectionType() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function loadRouteLogStatus() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function closeWindow() { return [] }
                function loadScenario(arg1,arg2) {
                    var loadScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];
                    deferred = $q.defer();                    
                    deferred.resolve(loadScenarioMockData);                    
                    return deferred.promise;
                }
                function loadDeliveryRoute() {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function deliveryRouteChange() {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function scenarioChange() {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function generateRouteLogSummary(){
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function generatePdf(){
                    deferred = $q.defer();
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
                    deferred = $q.defer();
                    deferred.resolve(['Single','Multiple']);
                    return deferred.promise;
                }
                function getStatus() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function getScenario() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function getRoutes() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function getRouteDetailsByGUID() {
                    deferred = $q.defer();
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
        
            $provide.value('items', { externalId: "0", unitName: "Worthing  Office", unitAddressUDPRN: 2333402, area: "BN", ID: "b51aa229-c984-4ca6-9c12-510187b81050",id: "b51aa229-c984-4ca6-9c12-510187b81050"});         
        });

        inject(function (_$q_,_$controller_, _routeLogService_, _$rootScope_,items) {
            $rootScope = _$rootScope_;
            vm = $rootScope.$new();
            routeLogService = _routeLogService_;
            $q = _$q_;
            deferred = _$q_.defer();

            controller = _$controller_('RouteLogController', {
                routeLogService: routeLogService,
                items: items,
                $scope: vm
                
            })
        });       
    });


    it('should be set md-text class is selected', function() {
        expect(controller.selectClass).toContain("routeSearch md-text");
    });
 
    it('Should be call method selectionType and routelogstatus once initialize method called', function () {
        spyOn(controller, 'loadSelectionType').and.callThrough();
        spyOn(controller, 'loadRouteLogStatus').and.callThrough();

        controller.initialize();

        expect(controller.loadSelectionType).toHaveBeenCalled();
        expect(controller.loadRouteLogStatus).toHaveBeenCalled();
    });


    it('Should be close dialog-window once closeWindow method called', function () {
        spyOn(routeLogService, 'closeWindow').and.callThrough();
        controller.closeWindow();
        expect(routeLogService.closeWindow).toHaveBeenCalled();
    });

    it('should be empty searchTerm value once clearSearchTerm method called', function() {      
        controller.clearSearchTerm();       
        expect(controller.searchTerm).toBe('');
    });

    it('should be set selected delivery unit is a object of items service', function() {
        expect(controller.selectedDeliveryUnitObj).toEqual({ externalId: "0", unitName: "Worthing  Office", unitAddressUDPRN: 2333402, area: "BN", ID: "b51aa229-c984-4ca6-9c12-510187b81050",id: "b51aa229-c984-4ca6-9c12-510187b81050"});
    });

    it('should be clear fields on changing selection type', function () {
        controller.selectionTypeChange();
        expect(controller.selectedRouteStatusObj).toBe(null);
        expect(controller.selectedRouteScenario).toBe(null);
        expect(controller.selectedRoute).toBe(null);
        expect(controller.isSelectionType).toBe(false);
        expect(controller.isRouteScenarioDisabled).toBe(true);
        expect(controller.isDeliveryRouteDisabled).toBe(true);
        expect(controller.isShowMultiSelectionRoute).toBe(false);
        expect(controller.deliveryRoute).toBe(null);
        expect(controller.routeDetails).toBe(false);
    });

    it('should be select a route status and clear the delivery route ', function() {
        controller.selectedRouteStatusObj = {id: "b51aa229-c984-4ca6-9c12-510187b81050"};
        controller.selectedRouteStatus();
        $rootScope.$apply();
        
        expect(controller.RouteScenario).toEqual([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);
        expect(controller.isRouteScenarioDisabled).toBe(false);
        expect(controller.deliveryRoute).toBe(null);
        expect(controller.routeDetails).toBe(false);
    });

    it('should promise to return a success response true once loadSelectionType method is called', function () {
        var deferred = $q.defer();
        var loadSelectionTypeMockData = [{"RouteselectionTypeObj":[
                                                            {"referenceDataName":null,"referenceDataValue":"Single","dataDescription":"Single","displayText":null,"dataParentId":null,"id":"ffeb0dbc-d12b-e711-8735-28d244aef9ed","referenceDataCategory_GUID":"5c64cd73-d12b-e711-8735-28d244aef9ed","dataParent_GUID":null},
                                                            {"referenceDataName":null,"referenceDataValue":"Multiple","dataDescription":"Multiple","displayText":null,"dataParentId":null,"id":"01ae2cc6-d12b-e711-8735-28d244aef9ed","referenceDataCategory_GUID":"5c64cd73-d12b-e711-8735-28d244aef9ed","dataParent_GUID":null}],
                                    "selectedRouteSelectionObj":{"referenceDataName":null,"referenceDataValue":"Single","dataDescription":"Single","displayText":null,"dataParentId":null,"id":"ffeb0dbc-d12b-e711-8735-28d244aef9ed","referenceDataCategory_GUID":"5c64cd73-d12b-e711-8735-28d244aef9ed","dataParent_GUID":null}
                                }];

        spyOn(routeLogService,'loadSelectionType').and.returnValue(deferred.promise);
        controller.loadSelectionType();

        deferred.resolve(loadSelectionTypeMockData);       
        $rootScope.$apply();

        expect(routeLogService.loadSelectionType).toHaveBeenCalled();
        expect(controller.RouteselectionTypeObj).toBe(loadSelectionTypeMockData[0].RouteselectionTypeObj);
        expect(controller.selectedRouteSelectionObj.referenceDataValue).toBe(loadSelectionTypeMockData[0].selectedRouteSelectionObj.referenceDataValue);
    });

     it('should promise to return a success response true once selectedRouteStatus method is called', function () {
        controller.selectedRouteStatusObj = {id: "b51aa229-c984-4ca6-9c12-510187b81050"};

        controller.selectedRouteStatus();
        vm.$digest();
        expect(controller.isRouteScenarioDisabled).toBe(false);
        expect(controller.deliveryRoute).toBe(null);
        expect(controller.routeDetails).toBe(false);
        expect(controller.generateSummaryReport).toBe(false);
        expect(controller.RouteScenario).toEqual([{scenarioName: 'Worthing Delivery Office - Baseline weekday', id: 'b51aa229-c984-4ca6-9c12-510187b81050'}]);

    });

    it('should promise to return a success response once loadRouteLogStatus method is called', function () {
        
        var deffer = $q.defer();
        var loadRouteLogStatusMockData = [
                                        {"id":"9c1e56d7-5397-4984-9cf0-cd9ee7093c88","name":null,"value":"Live","displayText":null,"description":"Live"},
                                        {"id":"bee6048d-79b3-49a4-ad26-e4f5b988b7ab","name":null,"value":"Not Live","displayText":null,"description":"Not Live"}
                                    ];

        spyOn(routeLogService, 'loadRouteLogStatus').and.returnValue(deffer.promise);

        controller.loadRouteLogStatus();

        deffer.resolve(loadRouteLogStatusMockData);
        $rootScope.$apply();

        expect(controller.RouteStatusObj).toBe(loadRouteLogStatusMockData);
        expect(controller.selectedRouteStatusObj).toBe(loadRouteLogStatusMockData[0]);
        expect(controller.RouteScenario).toEqual([{scenarioName: 'Worthing Delivery Office - Baseline weekday', id: 'b51aa229-c984-4ca6-9c12-510187b81050'}]);
    });

    it('should be load delivery route and after clear delivery route once scenarioChange method called', function() {
        controller.selectedRouteSelectionObj = {value:true};
        controller.selectedRouteStatusObj = { id:'b51aa229-c984-4ca6-9c12-510187b81050'};
        controller.selectedRouteScenario = { id:'9c1e56d7-5397-4984-9cf0-cd9ee7093c88'};

        spyOn(routeLogService,'loadScenario');
        controller.scenarioChange();
            
        expect(controller.isDeliveryRouteDisabled).toBe(false);
        expect(controller.multiSelectiondeliveryRoute).toBeUndefined();        
        expect(controller.deliveryRoute).toBe(null);
        expect(controller.routeDetails).toBe(false);
        expect(controller.generateSummaryReport).toBe(false);
    });

    it('should promise to return a success response once loadScenario method is called', function () {
        
        var deffer = $q.defer();
        var loadScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];        

        spyOn(routeLogService, 'loadScenario').and.returnValue(deffer.promise);

        controller.loadScenario();

        deffer.resolve(loadScenarioMockData);
        $rootScope.$apply();

        expect(routeLogService.loadScenario).toHaveBeenCalled();
        expect(controller.RouteScenario).toEqual([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}]);
    });

    it('should promise to return a empty response once loadScenario method is called', function () {
        
        var deffer = $q.defer();
        var response = [];
        spyOn(routeLogService, 'loadScenario').and.returnValue(deffer.promise);

        controller.loadScenario();

        deffer.resolve(response);
        $rootScope.$apply();

        expect(routeLogService.loadScenario).toHaveBeenCalled();
        expect(controller.RouteScenario).toBeUndefined();
        expect(controller.selectedRouteScenario).toBe(null);
        expect(controller.isSelectionType).toBe(true);
        expect(controller.selectedRoute).toBe(null);
        expect(controller.isDeliveryRouteDisabled).toBe(true);
        expect(controller.isShowMultiSelectionRoute).toBe(false);
    });
    

    it('should promise to return a success response once deliveryRouteChange method is called', function () {
        
        var deffer = $q.defer();
        var deliveryRouteChangeMockData = {"id":"33bc42ed-42bd-4456-a3f6-33a10efc8bd3","externalId":null,"routeName":"1102 UPPER HIGH STREET        ","routeNumber":"102       ","operationalStatus_Id":0,"routeMethodType_Id":0,"travelOutTransportType_Id":null,"travelInTransportType_Id":null,"travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"displayText":"(102)1102 UPPER HIGH STREET        ","methodReferenceGuid":"c168f46e-561b-e711-9f8c-28d244aef9ed","method":"High Capacity Trolley","deliveryOffice":null,"aliases":0,"blocks":2,"scenarioName":"Worthing Delivery Office - Baseline weekday","dPs":524,"businessDPs":8,"residentialDPs":516,"travelOutTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","travelInTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","accelarationIn":"High Capacity Trolley","accelarationOut":"High Capacity Trolley","pairedRoute":"","totaltime":"0:07 mins"};
        var selectedRouteValue = {id:'33bc42ed-42bd-4456-a3f6-33a10efc8bd3'};

        spyOn(routeLogService, 'deliveryRouteChange').and.returnValue(deffer.promise);

        controller.deliveryRouteChange(selectedRouteValue);

        deffer.resolve(deliveryRouteChangeMockData);
        $rootScope.$apply();

        expect(routeLogService.deliveryRouteChange).toHaveBeenCalled();
        expect(routeLogService.deliveryRouteChange).toHaveBeenCalledWith('33bc42ed-42bd-4456-a3f6-33a10efc8bd3');
        expect(controller.routeDetails).toEqual({"id":"33bc42ed-42bd-4456-a3f6-33a10efc8bd3","externalId":null,"routeName":"1102 UPPER HIGH STREET        ","routeNumber":"102       ","operationalStatus_Id":0,"routeMethodType_Id":0,"travelOutTransportType_Id":null,"travelInTransportType_Id":null,"travelOutTimeMin":null,"travelInTimeMin":null,"spanTimeMin":null,"deliveryScenario_Id":null,"deliveryRouteBarcode":null,"displayText":"(102)1102 UPPER HIGH STREET        ","methodReferenceGuid":"c168f46e-561b-e711-9f8c-28d244aef9ed","method":"High Capacity Trolley","deliveryOffice":null,"aliases":0,"blocks":2,"scenarioName":"Worthing Delivery Office - Baseline weekday","dPs":524,"businessDPs":8,"residentialDPs":516,"travelOutTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","travelInTransportType_GUID":"c168f46e-561b-e711-9f8c-28d244aef9ed","accelarationIn":"High Capacity Trolley","accelarationOut":"High Capacity Trolley","pairedRoute":"","totaltime":"0:07 mins"});
        expect(controller.generateSummaryReport).toEqual(false);
    }); 

    it('should promise to return a empty response once generateRouteLogSummary method is called', function () {
        controller.generateSummaryReport = true;
        var deffer = $q.defer();   
        var response;     
        var deliveryRouteDTO = { "DeliveryRouteDTO": true };
        spyOn(routeLogService, 'generatePdf').and.returnValue(deffer.promise);
        spyOn(routeLogService, 'generateRouteLogSummary').and.returnValue(deffer.promise);

        controller.generateRouteLogSummary(deliveryRouteDTO);

        deffer.resolve('mainRoutePath.pdf');
        $rootScope.$apply();

        expect(routeLogService.generateRouteLogSummary).toHaveBeenCalled();
        expect(routeLogService.generatePdf).toHaveBeenCalled();    
    });
     
});
