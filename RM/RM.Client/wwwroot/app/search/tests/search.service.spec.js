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
    var $stateParams;
    var $timeout;
    var $q;
    var deferred;
    var CommonConstants;
    var searchBusinessService;
    var MockCommonConstants = {    
    	SearchLessThanThreeCharactersErrorMessage: "At least three characters must be input for a Search",
   		SearchErrorType: "Warning",
   		EntityType: { DeliveryPoint: "DeliveryPoint"}
	};
	var MockStringFormatService = {
        Stringformat: function() { 
        /*var theString = arguments[0];
           for (var i = 1; i < arguments.length; i++) {               
               var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
               theString = theString.replace(regEx, arguments[i]);
           }
           return theString;*/
        }
    };
    var MockGlobalSettings = {
        deliveryPointApiUrl: "http://172.18.5.7/DeliveryPoint/api",
		getDeliveryPointById: "/DeliveryPointManager/deliverypoint/Guid/{0}"
    };

    beforeEach(function() {
    	module('search');
    	module(function($provide){
	    	$provide.value('$state', { go: function(state, args){}});                      
            $provide.value('GlobalSettings', MockGlobalSettings);
		    $provide.value('stringFormatService',MockStringFormatService); 
		    $provide.value('CommonConstants', MockCommonConstants);
		    $provide.value('$state', {});
		    $provide.value('$stateParams', {});  
		    $provide.factory('$mdDialog', function() {return {show: jasmine.createSpy(true)}; });
		    $provide.factory('mapStylesFactory',function(){return{}});    		    	
	    	$provide.factory('popUpSettingService', function(){return{}});    		    	
	    	$provide.factory('mapFactory', function(){return{}}); 
    	});

    	inject(function(_$rootScope_,_searchService_,_$state_,_mapFactory_,_mapStylesFactory_,_popUpSettingService_,_$mdDialog_,
_$stateParams_,_$timeout_,_$q_,_CommonConstants_,_stringFormatService_,_searchBusinessService_){
            $rootScope = _$rootScope_;
    		$scope = _$rootScope_.$new();
    		searchService = _searchService_;
		    $state = _$state_;
		    mapFactory = _mapFactory_;
		    mapStylesFactory = _mapStylesFactory_;
		    popUpSettingService = _popUpSettingService_;
		    $mdDialog = _$mdDialog_;
		    $stateParams = _$stateParams_;
		    $timeout = _$timeout_;
		    $q = _$q_;
		    CommonConstants = _CommonConstants_;
		    searchBusinessService = _searchBusinessService_;
    	});
    });

    it('should return promise response success when resultSet method query argument length is `greater then 3`', function() {
        var deferredSuccess = $q.defer();
        var response = {"searchCounts":[{"type":4,"count":2}],"searchResultItems":[{"value":"BT National IN","displayText":"Shown"},{"value":"BT National INDIA","displayText":"Shown"}],"isResultDisplay":true};
        spyOn(searchService,'basicSearch').and.returnValue(deferredSuccess.promise);
        
        searchBusinessService.resultSet('BT National IN');

        deferredSuccess.resolve(response); 
        $scope.$digest();  

        expect(searchService.basicSearch).toHaveBeenCalled();
        expect(searchService.basicSearch).toHaveBeenCalledWith('BT National IN');
        expect(response).toEqual({"searchCounts":[{"type":4,"count":2}],"searchResultItems":[{"value":"BT National IN","displayText":"Shown"},{"value":"BT National INDIA","displayText":"Shown"}],"isResultDisplay":true});
        expect(response).not.toBe({});
    });

    it('should return promise response `empty` object when resultSet method query argument length is `less then 3`', function() {
        var deferredSuccess = $q.defer();
        var response = {"searchCounts":{0:{count:0}},"results":{},"isResultDisplay":false};
        spyOn(searchService,'basicSearch').and.returnValue(deferredSuccess.promise);
        
        searchBusinessService.resultSet('BT');

        deferredSuccess.resolve(response); 
        $scope.$digest();  

        expect(searchService.basicSearch).not.toHaveBeenCalled();
        expect(response).toEqual({"searchCounts":{0:{count:0}},"results":{},"isResultDisplay":false});
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
    	expect(response).toEqual({});
    });


});
