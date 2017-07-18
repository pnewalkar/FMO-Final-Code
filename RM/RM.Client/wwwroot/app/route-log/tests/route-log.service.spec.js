'use strict';
describe('Route-Log: Service', function () {
    var $scope;
    var $rootScope;
    var $q;    
    var $mdDialog;
    var routeLogService;
    var routeLogAPIService;
    var CommonConstants;    
    var referencedataApiService;
    var referenceDataConstants;
	 
    beforeEach(function () {
        module('routeLog');
        module(function ($provide) {
            $provide.factory('routeLogAPIService', function ($q) {
                function getSelectionType() {}                  
                function getStatus() {}                    
                function getScenario(selectedRouteStatusObj, selectedDeliveryUnitObj){
                  var MockGetScenarioData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"},
                    {"scenarioName":"Worthing Delivery Office - Baseline weekend","id":"8b212f80-53c0-437a-8aa5-a81f610e85b6"}];
                  var deferred = $q.defer();
                  deferred.resolve(MockGetScenarioData);                    
                  return deferred.promise;
                }                                    
                function getRoutes(operationStateID, deliveryScenarioID,selectionType) {
                  var deferred = $q.defer();                  
                  deferred.resolve({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
                  return deferred.promise;
                }
                function getRouteDetailsByGUID(selectedRouteId) {
                  var deferred = $q.defer();
                  deferred.resolve({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
                  return deferred.promise;
                }
                function generateRouteLogSummary(deliveryRoute) {
                  var deferred = $q.defer();
                  deferred.resolve({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
                  return deferred.promise;
                }                    
                function generatePdf(pdfname) {
                  var deferred = $q.defer();
                  deferred.resolve({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
                  return deferred.promise;
                }

                return {
                    getSelectionType: getSelectionType,
      			        getStatus: getStatus,
      			        getScenario: getScenario,
      			        getRoutes: getRoutes,
      			        getRouteDetailsByGUID: getRouteDetailsByGUID,
                    generateRouteLogSummary: generateRouteLogSummary,
                    generatePdf: generatePdf
                };
            });

            $provide.factory('referencedataApiService', function ($q) {
              function getSimpleListsReferenceData(listname) {                    
                    var MockData = {"listItems":[
                        {"id":"a47f6791-d766-4d38-80df-809390d81e8a","name":null,"value":"Single","displayText":null,"description":"Live"},
                        {"id":"13cc8c8d-bfed-4aff-939d-d3988f0cc79d","name":null,"value":"Multiple","displayText":null,"description":"Planned"}]};
                    var deferred = $q.defer();
                    deferred.resolve(MockData);
                    return deferred.promise;
                }
              return {
                    getSimpleListsReferenceData: getSimpleListsReferenceData
                };
            });

            $provide.value('referenceDataConstants',{
                UI_RouteLogSearch_SelectionType: { DBCategoryName: "UI_RouteLogSearch_SelectionType", AppCategoryName: "UI_RouteLogSearch_SelectionType", ReferenceDataNames: [] },
                SenarioOperationState: { AppCategoryName: "SenarioOperationState", DBCategoryName: "Senario Operation State", ReferenceDataNames: [] }
            });                  

       		  $provide.constant('CommonConstants', {RouteLogSelectionType:{Multiple:"Multiple",Single:"Single"}});       		  
    		    $provide.factory('$mdDialog', function() {
    		        return {cancel: jasmine.createSpy()};
    		    });
        });
        
        inject(function (
          _$rootScope_,
          _routeLogAPIService_,
          _routeLogService_,
          _$q_,
          _$mdDialog_,
          _referencedataApiService_,
          _referenceDataConstants_) {
          
        	$scope = _$rootScope_.$new(); 
          $rootScope = _$rootScope_;
          routeLogService = _routeLogService_;
          routeLogAPIService = _routeLogAPIService_;
          $q = _$q_;
          $mdDialog = _$mdDialog_;
          referenceDataConstants = _referenceDataConstants_;
          referencedataApiService = _referencedataApiService_;
        });
    });


    it('should be window close when dialog button cancel', function() {
    	routeLogService.closeWindow();
    	expect($mdDialog.cancel).toHaveBeenCalled();
    });

    it('should promise to return a success response once loadSelectionType method is called', function () {
      var response;
      var MockLoadSelectionTypeData = [{"RouteselectionTypeObj":
                              [{"id":"a47f6791-d766-4d38-80df-809390d81e8a","name":null,"value":"Single","displayText":null,"description":"Live"},
                              {"id":"13cc8c8d-bfed-4aff-939d-d3988f0cc79d","name":null,"value":"Multiple","displayText":null,"description":"Planned"}],
                      "selectedRouteSelectionObj": {"id":"a47f6791-d766-4d38-80df-809390d81e8a","name":null,"value":"Single","displayText":null,"description":"Live"}}
                      ];

      routeLogService.loadSelectionType('UI_RouteLogSearch_SelectionType').then(function(result){
        response = result;
      });
      $rootScope.$apply();
      expect(response).toEqual(MockLoadSelectionTypeData);
    });

    it('should promise to return a success response once loadRouteLogStatus method is called', function () {
        var MockLoadRouteLogStatusData = [
                        {"id":"a47f6791-d766-4d38-80df-809390d81e8a","name":null,"value":"Single","displayText":null,"description":"Live"},
                        {"id":"13cc8c8d-bfed-4aff-939d-d3988f0cc79d","name":null,"value":"Multiple","displayText":null,"description":"Planned"}];
        var response;         
        routeLogService.loadRouteLogStatus('SenarioOperationState').then(function(result){
          response = result;
        });        
        $rootScope.$apply();        
        expect(response).toEqual(MockLoadRouteLogStatusData);
    });    

    it('should be return `isDeliveryRouteDisabled` and `isShowMultiSelectionRoute` as true when scenarioChange method called ', function() {    	    	
    	expect(routeLogService.scenarioChange("Multiple")).toEqual({isDeliveryRouteDisabled: true,isShowMultiSelectionRoute: true});      
    });

    it('should be return `isDeliveryRouteDisabled` and `isShowMultiSelectionRoute` as false when scenarioChange method called ', function() {            
      expect(routeLogService.scenarioChange("Single")).toEqual({isDeliveryRouteDisabled: false,isShowMultiSelectionRoute: false});      
    });
    
    it('should promise to return a success response once loadScenario method is called', function () {
      var response;
      var selectedRouteStatusObj = "a47f6791-d766-4d38-80df-809390d81e8a";
      var selectedDeliveryUnitObj = "38fd2404-d65b-e711-80e2-000d3a22173b";

      routeLogService.loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function(result){
        response = result;
      });      
      $rootScope.$apply();

      expect(response).toEqual([{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"},{"scenarioName":"Worthing Delivery Office - Baseline weekend","id":"8b212f80-53c0-437a-8aa5-a81f610e85b6"}]);                
    });       

    it('should promise to return a object `deliveryRoute` where selectionType is `Single` once loadDeliveryRoute method is called', function () {      
      var operationStateID = 'ffeb0dbc-d12b-e711-8735-28d244aef9ed';
      var deliveryScenarioID = 'ffeb0dbc-d12b-e711-8735-28d244aef9ed';
      var selectionType =  "Single";      
      var response;

      routeLogService.loadDeliveryRoute(operationStateID, deliveryScenarioID, selectionType).then(function(result){
        response = result;
      });      
      $rootScope.$apply();

      expect(response).toEqual([{deliveryRoute:{ "deliveryRoute": true, "multiSelectiondeliveryRoute": true }, multiSelectiondeliveryRoute: [  ] }]);
    });

    it('should promise to return a object `multiSelectiondeliveryRoute` where selectionType is `Multiple` once loadDeliveryRoute method is called', function () {      
      var operationStateID = 'ffeb0dbc-d12b-e711-8735-28d244aef9ed';
      var deliveryScenarioID = 'ffeb0dbc-d12b-e711-8735-28d244aef9ed';
      var selectionType =  "Multiple";      
      var response;

      routeLogService.loadDeliveryRoute(operationStateID, deliveryScenarioID, selectionType).then(function(result){
        response = result;
      });      
      $rootScope.$apply();

      expect(response).toEqual([{deliveryRoute:[], multiSelectiondeliveryRoute: { "deliveryRoute": true, "multiSelectiondeliveryRoute": true }}]);
    });

    it('should promise to return a success response once deliveryRouteChange method is called', function () {
      var selectedRouteID = 'ffeb0dbc-d12b-e711-8735-28d244aef9ed';
      var response;

      routeLogService.deliveryRouteChange(selectedRouteID).then(function(result){
        response = result;
      });      
      $rootScope.$apply();      
      expect(response).toEqual({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
    });

    it('should promise to return a success response once generateRouteLogSummary method is called', function () {
      var response;
      routeLogService.generateRouteLogSummary(true).then(function(result){
        response = result;
      });
      $rootScope.$apply();      
      expect(response).toEqual({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
    });

    it('should promise to return a success response once generatePdf method is called', function () {      
      var response;
      routeLogService.generatePdf('selectRoute.pdf').then(function(result){
        response = result;
      });      
      $rootScope.$apply();
      expect(response).toEqual({ "deliveryRoute": true, "multiSelectiondeliveryRoute": true });
    });

});
