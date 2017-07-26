'use strict';
describe('Search: Service', function() {
	var $scope;
	var $rootScope;
	var searchService;
	var $state;
	var mapFactory;
	var mapStylesFactory;
    var popUpSettingService;
    var $mdDialog;
    var $q;    
    var CommonConstants;
    var searchBusinessService;
    var MockCommonConstants = {    
    	SearchLessThanThreeCharactersErrorMessage: "At least three characters must be input for a Search",
   		SearchErrorType: "Warning",
   		EntityType: { DeliveryPoint: "DeliveryPoint"},
        RouteName: "ROUTENAME",
        DpUse: "DPUSE",
	};	    

    beforeEach(function() {
    	module('search');
    	module(function($provide){
	    	$provide.value('$state', { go: function(state, args){}});                      
            $provide.value('GlobalSettings', {deliveryPointApiUrl:'',getDeliveryPointById:''});
		    $provide.value('stringFormatService',{Stringformat: function() {}}); 
		    $provide.value('CommonConstants', MockCommonConstants);		    
		    $provide.value('$stateParams', {});  
		    $provide.factory('$mdDialog', function() {return {show: jasmine.createSpy(true)}; });
		    $provide.factory('mapStylesFactory',function(){return{}});    		    	
	    	$provide.factory('popUpSettingService', function(){return{}});    		    	
	    	$provide.factory('mapFactory', function ($q) {                     
                function GetRouteForDeliveryPoint(deliveryPointId) {                    
                    var deffer = $q.defer();
                    deffer.resolve([{"key":"ROUTENAME","value":"Organisation"},{"key":"DPUSE","value":"Residential"}]);
                    return deffer.promise;
                }
                function setDeliveryPoint(){}
                return {
                    setDeliveryPoint: setDeliveryPoint,
                    GetRouteForDeliveryPoint: GetRouteForDeliveryPoint
                }
            });
            $provide.factory('searchService', function ($q) {                     
                function basicSearch(query) {                    
                    var MockBasicSearchData = {"searchCounts":[{"type":4,"count":2}],"searchResultItems":[{"value":"BT National IN","displayText":"Shown"},{"value":"BT National INDIA","displayText":"Shown"}],"isResultDisplay":true};
                    var deffer = $q.defer();
                    deffer.resolve(MockBasicSearchData);
                    return deffer.promise;
                }
                function GetDeliveryPointByGuid(deliveryPointGuid){
                    var deffer = $q.defer();
                    return deffer.promise;   
                }
                return {
                    basicSearch: basicSearch,
                    GetDeliveryPointByGuid: GetDeliveryPointByGuid
                }
            });
    	});

    	inject(function(
            _$rootScope_,
            _searchService_,
            _$state_,
            _mapFactory_,
            _mapStylesFactory_,
            _popUpSettingService_,
            _$mdDialog_,
            _$q_,
            _CommonConstants_,
            _stringFormatService_,
            _searchBusinessService_){

            $rootScope = _$rootScope_;
    		$scope = _$rootScope_.$new();
    		searchService = _searchService_;
		    $state = _$state_;
		    mapFactory = _mapFactory_;
		    mapStylesFactory = _mapStylesFactory_;
		    popUpSettingService = _popUpSettingService_;
		    $mdDialog = _$mdDialog_;
		    $q = _$q_;
		    CommonConstants = _CommonConstants_;
		    searchBusinessService = _searchBusinessService_;
    	});
    });

    it('should return promise response success when resultSet method query argument length is `greater then 3`', function() {
        var response;
        searchBusinessService.resultSet('BT National IN').then(function(result){
            response = result;
        });
        $rootScope.$apply();  
        expect(response).toEqual([{resultscount: [{"type":4,"count":2}], results: [{"value":"BT National IN","displayText":"Shown"},{"value":"BT National INDIA","displayText":"Shown"}], isResultDisplay: true}]);
    });

    it('should return promise response `empty` object when resultSet method query argument length is `less then 3`', function() {
        var response;        
        searchBusinessService.resultSet('BT').then(function(result){
            response = result;
        });
        $rootScope.$apply();  
        expect(response).toEqual([{resultscount:{ 0:{count: 0}},results:{},isResultDisplay: false}]);
    });

    it('should return contextTitle results as undefined when onEnterKeypress method argument searchText is empty', function() {
    	var searchText;
    	var results;        
    	var response = searchBusinessService.onEnterKeypress(searchText,results);
    	expect(response).toEqual({results:[{ displayText:"At least three characters must be input for a Search", type:"Warning"}],contextTitle: undefined});
    });

    it('should return contextTitle results as undefined when onEnterKeypress method argument searchText length `less then 3`', function() {
    	var searchText="BT";
    	var results;        
    	var response = searchBusinessService.onEnterKeypress(searchText,results);
    	expect(response).toEqual({results:[{ displayText:"At least three characters must be input for a Search", type:"Warning"}],contextTitle: undefined});
    });

    it('should return contextTitle results when onEnterKeypress method argument searchText length `greater then 3`', function() {
    	var searchText="BTN1";
    	var results = [{displayText:"BTN1 National IN", type:'DeliveryPoint'}];
    	var response = searchBusinessService.onEnterKeypress(searchText,results);
    	expect(response).toEqual({results:[{displayText:"BTN1 National IN", type:'DeliveryPoint'}],contextTitle:undefined});
    });

    it('should promise to return a success response once OnChangeItem method is called', function() {
        var MockOnChangeItemData = {"type":"FeatureCollection","features":[
                            {"type":"Feature","id":"06f7a44d-ad06-449f-97eb-d1d38288e47b","properties":
                            {"deliveryPointId":"06f7a44d-ad06-449f-97eb-d1d38288e47b","street":"Chandos Road","subBuildingName":null},
                        "geometry":{"type":"Point","coordinates":[514641.36000000034,102514.08999999985]}
                    }]};
        var deferred = $q.defer();
        var selectedItem = {"udprn":2335303,"displayText":" 15, Chandos Road, ","type":"DeliveryPoint","deliveryPointGuid":"06f7a44d-ad06-449f-97eb-d1d38288e47b","$$hashKey":"object:150"};
        spyOn(mapFactory,'setDeliveryPoint');                
        spyOn(searchService,'GetDeliveryPointByGuid').and.returnValue(deferred.promise);
        spyOn($state,'go');

        searchBusinessService.OnChangeItem(selectedItem);
        deferred.resolve(MockOnChangeItemData);
        $rootScope.$apply();

        expect(searchService.GetDeliveryPointByGuid).toHaveBeenCalledWith('06f7a44d-ad06-449f-97eb-d1d38288e47b');        
        expect(mapFactory.setDeliveryPoint).toHaveBeenCalledWith(514641.36000000034, 102514.08999999985);        
        expect($state.go).toHaveBeenCalledWith('deliveryPointDetails',{selectedDeliveryPoint:{ deliveryPointId: '06f7a44d-ad06-449f-97eb-d1d38288e47b', street: 'Chandos Road', subBuildingName: null, routeName: [ 'Organisation' ], dpUse: 'Residential' }},{reload:true});            
    });

    it('should promise to return a success response once showDeliveryPointDetails method is called', function() {
        var MockShowDeliveryPointDetailsData = [{"key":"ROUTENAME","value":""},{"key":"DPUSE","value":"Residential"}];
        var deferred = $q.defer();
        var deliveryPointDetails = {"deliveryPointId":"06f7a44d-ad06-449f-97eb-d1d38288e47b","street":"Chandos Road","subBuildingName":null}
        spyOn(mapFactory,'GetRouteForDeliveryPoint').and.returnValue(deferred.promise);
        spyOn($state,'go');

        searchBusinessService.showDeliveryPointDetails(deliveryPointDetails);
        deferred.resolve(MockShowDeliveryPointDetailsData);
        $rootScope.$apply();

        expect(mapFactory.GetRouteForDeliveryPoint).toHaveBeenCalledWith('06f7a44d-ad06-449f-97eb-d1d38288e47b');        
        expect($state.go).toHaveBeenCalledWith('deliveryPointDetails',{ selectedDeliveryPoint:{ deliveryPointId: '06f7a44d-ad06-449f-97eb-d1d38288e47b', street: 'Chandos Road', subBuildingName: null, routeName: [ '' ], dpUse: 'Residential' }},{reload:true});            
    });
});
