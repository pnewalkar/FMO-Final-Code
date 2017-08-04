'use strict';
describe('Advance Search: Service', function() {	
	var $rootScope;
	var advanceSearchAPIService;
    var $q;
	var searchService;
	var mapFactory;
	var CommonConstants;
	var $state;
    var roleAccessService;
    var searchDPSelectedService;
    var mapService;
	var MockCommonConstants = {    
    	EntityType: { DeliveryPoint: "DeliveryPoint", StreetNetwork: "StreetNetwork", Route: "Route", Postcode: "Postcode" },
        UserType: { DeliveryUser: "Delivery User", CollectionUser: "Collection User", ManagerUser: "Manager User" },
        RouteName: "ROUTENAME",
        DpUse: "DPUSE",
	};	

	beforeEach(function() {
		module('advanceSearch');
		module(function($provide){
			$provide.value('$state', { go: function(state, args){}});                      
		    $provide.value('CommonConstants', MockCommonConstants);		    
			$provide.factory('advanceSearchAPIService',function($q){
				return{advanceSearch: function (query){}}											
			});
			$provide.factory('searchService',function($q){
				function GetDeliveryPointByGuid(selectedItem){
					var MockGetDeliveryPointByGuidData = {"type":"FeatureCollection","features":[
                            {"type":"Feature","id":"06f7a44d-ad06-449f-97eb-d1d38288e47b","properties":
                            {"deliveryPointId":"06f7a44d-ad06-449f-97eb-d1d38288e47b","street":"Chandos Road","subBuildingName":null},
                        "geometry":{"type":"Point","coordinates":[514641.36000000034,102514.08999999985]}
                    }]};

					var deffer = $q.defer();
                    deffer.resolve(MockGetDeliveryPointByGuidData);
                    return deffer.promise;
				}				
				return{
					GetDeliveryPointByGuid: GetDeliveryPointByGuid					
				}
			});
			$provide.factory('mapFactory', function ($q) {                     
                function GetRouteForDeliveryPoint(deliveryPointId) {                    
                    var deffer = $q.defer();
                    deffer.resolve([{"key":"ROUTENAME","value":"Organisation"},{"key":"DPUSE","value":"Residential"}]);
                    return deffer.promise;
                }
                function setDeliveryPoint(long, lat){}
                return {
                    setDeliveryPoint: setDeliveryPoint,
                    GetRouteForDeliveryPoint: GetRouteForDeliveryPoint
                }
            });
            $provide.factory('roleAccessService', function($q){
                return {
                    fetchActionItems: function(){
                        var deffer = $q.defer();
                        return deffer.promise;
                    }
                }
            });
            $provide.factory('searchDPSelectedService', function($q){
                return {
                    setSelectedDP: function(){},
                }
            });
            $provide.factory('mapService', function($q){
                return {
                    deselectDP: function(){}
                }
            });
		});
		inject(function(
			_$rootScope_,
			_advanceSearchService_,
			_advanceSearchAPIService_,
			_$q_,
			_searchService_,
			_mapFactory_,
			_CommonConstants_,
			_$state_,
            _roleAccessService_,
            _searchDPSelectedService_,
            _mapService_){

			$rootScope = _$rootScope_;
			advanceSearchService = _advanceSearchService_;
			advanceSearchAPIService = _advanceSearchAPIService_;
			$q = _$q_;
			searchService = _searchService_;
			mapFactory = _mapFactory_;
			CommonConstants = _CommonConstants_;
			$state = _$state_;			
            roleAccessService = roleAccessService;
            searchDPSelectedService = searchDPSelectedService;
            mapService = mapService;
		});
	});

    it('should promise to return a success response when type match with `DeliveryPoint` and `Deliverypoints` array length is 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockDeliveryPointData = {"searchResultItems":[{"udprn":10872591,"displayText":" HIGHFIELD WAY ","type":"DeliveryPoint","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockDeliveryPointData);
        	return deferred.promise;
    	});
                
        /*spyOn(roleAccessService,'fetchActionItems').and.callFake(function(){
            var MockDeliveryPointData = [{"UserName":"shobharam.katiya    ","RoleName":"Delivery User","Unit_GUID":"092c69ae-4382-4183-84ff-ba07543d9c75","FunctionName":"Print Map","ActionName":"Print Map","UserId":"a867065b-b91e-e711-9f8c-28d244aef9ed","UnitType":"Delivery Office","UnitName":"High Wycombe North DO"}];
            return {
                RolesActionResult: 
            }
        });
        */
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply();

        expect(response).toEqual([{ type: 'DeliveryPoint', name: [{ displayText: ' HIGHFIELD WAY ', UDPRN: 10872591, type: 'DeliveryPoint', ID: '00000000-0000-0000-0000-000000000000' }],open: true}]);

    });

    it('should promise to return a success response when type match with `DeliveryPoint` and `Deliverypoints` array length is greater then 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockDeliveryPointData = {"searchResultItems":[
													{"udprn":10872591,"displayText":" HIGHFIELD WAY ","type":"DeliveryPoint","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"},
													{"udprn":10872591,"displayText":" HIGHFIELD WAY ","type":"DeliveryPoint","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockDeliveryPointData);
        	return deferred.promise;
    	});
        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply();

        expect(response).toEqual([{ type: 'DeliveryPoint', name: [{ displayText: ' HIGHFIELD WAY ', UDPRN: 10872591, type: 'DeliveryPoint', ID: '00000000-0000-0000-0000-000000000000' },{ displayText: ' HIGHFIELD WAY ', UDPRN: 10872591, type: 'DeliveryPoint', ID: '00000000-0000-0000-0000-000000000000' }],open: false}]);        
    });

    it('should promise to return a success response when type match with `Postcode` and `Postcodes` array length is 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockDeliveryPointData = {"searchResultItems":[{"udprn":10872591,"displayText":" HIGHFIELD WAY ","type":"Postcode","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockDeliveryPointData);
        	return deferred.promise;
    	});
        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply();

        expect(response).toEqual([{ type: 'Postcode', name: [{ displayText: ' HIGHFIELD WAY '}],open: true}]);
    });

    it('should promise to return a success response when type match with `Postcode` and `Postcodes` array length is greater then 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockDeliveryPointData = {"searchResultItems":[
													{"udprn":10872591,"displayText":" HIGHFIELD WAY ","type":"Postcode","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"},
													{"udprn":10872591,"displayText":" HIGHFIELD WAY ","type":"Postcode","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockDeliveryPointData);
        	return deferred.promise;
    	});
        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply();

        expect(response).toEqual([{ type: 'Postcode', name: [{ displayText: ' HIGHFIELD WAY '},{ displayText: ' HIGHFIELD WAY '}],open: false}]);
    });

    it('should promise to return a success response when type match with `StreetNetwork` and `StreetNames` array length is 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockStreetNetworkData = {"searchResultItems":[{"udprn":null,"displayText":" HIGHFIELD WAY ","type":"StreetNetwork","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockStreetNetworkData);
        	return deferred.promise;
    	});

        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply(); 

        expect(response).toEqual([{ type: 'StreetNetwork', name: [{ displayText: ' HIGHFIELD WAY ' }], open: true }]);
    });

    it('should promise to return a success response when type match with `StreetNetwork` and `StreetNames` array length is greater then 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockStreetNetworkData = {"searchResultItems":[
										{"udprn":null,"displayText":" HIGHFIELD WAY ","type":"StreetNetwork","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"},
										{"udprn":null,"displayText":" HIGHFIELD WAY ","type":"StreetNetwork","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockStreetNetworkData);
        	return deferred.promise;
    	});

        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply(); 

        expect(response).toEqual([{ type: 'StreetNetwork', name: [{ displayText: ' HIGHFIELD WAY ' },{ displayText: ' HIGHFIELD WAY ' }],open:false}]);
    });     

    it('should promise to return a success response when type match with `Route` and `DeliveryRoutes` array length is 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockStreetNetworkData = {"searchResultItems":[{"udprn":null,"displayText":" HIGHFIELD WAY ","type":"Route","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockStreetNetworkData);
        	return deferred.promise;
    	});

        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply(); 

        expect(response).toEqual([{ type: 'Route', name: [{ displayText: ' HIGHFIELD WAY ' }], open: true }]);
    });

    it('should promise to return a success response when type match with `Route` and `DeliveryRoutes` array length is greater then 1 once queryAdvanceSearch method is called', function() {
    	var response;
		spyOn(advanceSearchAPIService, 'advanceSearch').and.callFake(function(){
			var MockStreetNetworkData = {"searchResultItems":[
										{"udprn":null,"displayText":" HIGHFIELD WAY ","type":"Route","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"},
										{"udprn":null,"displayText":" HIGHFIELD WAY ","type":"Route","deliveryPointGuid":"00000000-0000-0000-0000-000000000000"}]};
        	var deferred = $q.defer();
        	deferred.resolve(MockStreetNetworkData);
        	return deferred.promise;
    	});

        
        advanceSearchService.queryAdvanceSearch('high').then(function(result){
            response = result;
        });
        $rootScope.$apply(); 

        expect(response).toEqual([{ type: 'Route', name: [{ displayText: ' HIGHFIELD WAY ' },{ displayText: ' HIGHFIELD WAY ' }],open:false}]);
    });

	it('should promise to return a success response once onChangeItem method is called', function() {
    	var response;		        
        var selectedItem = {ID:'06f7a44d-ad06-449f-97eb-d1d38288e47b'};
        spyOn(mapFactory,'setDeliveryPoint');
        spyOn($state,'go');

        advanceSearchService.onChangeItem(selectedItem).then(function(result){
        	response = result;
        });
        $rootScope.$apply(); 

        expect(response).toEqual({ type: 'FeatureCollection', features: [{ type: 'Feature', id: '06f7a44d-ad06-449f-97eb-d1d38288e47b', properties: { deliveryPointId: '06f7a44d-ad06-449f-97eb-d1d38288e47b', street: 'Chandos Road', subBuildingName: null, routeName: [ 'Organisation' ], dpUse: 'Residential' }, geometry: { type: 'Point', coordinates: [ 514641.36000000034, 102514.08999999985 ]}}]});
        expect(mapFactory.setDeliveryPoint).toHaveBeenCalledWith(514641.36000000034, 102514.08999999985);
        expect($state.go).toHaveBeenCalledWith('deliveryPointDetails',{selectedDeliveryPoint:{ deliveryPointId: '06f7a44d-ad06-449f-97eb-d1d38288e47b', street: 'Chandos Road', subBuildingName: null, routeName: [ 'Organisation' ], dpUse: 'Residential' }},{reload:true});
    });



    it('should promise to return a success response and response match with RouteName once showDeliveryPointDetails method is called', function() {
        var MockShowDeliveryPointDetailsData = [{"key":"ROUTENAME","value":"Organisation"},{"key":"DPUSE","value":"Residential"}];
        var deferred = $q.defer();
        var deliveryPointDetails = {"deliveryPointId":"06f7a44d-ad06-449f-97eb-d1d38288e47b","street":"Chandos Road","subBuildingName":null}
        spyOn(mapFactory,'GetRouteForDeliveryPoint').and.returnValue(deferred.promise);
        spyOn($state,'go');

        advanceSearchService.showDeliveryPointDetails(deliveryPointDetails);
        deferred.resolve(MockShowDeliveryPointDetailsData);
        $rootScope.$apply();

        expect(mapFactory.GetRouteForDeliveryPoint).toHaveBeenCalledWith('06f7a44d-ad06-449f-97eb-d1d38288e47b');        
        expect($state.go).toHaveBeenCalledWith('deliveryPointDetails',{ selectedDeliveryPoint:{ deliveryPointId: '06f7a44d-ad06-449f-97eb-d1d38288e47b', street: 'Chandos Road', subBuildingName: null, routeName: [ 'Organisation' ], dpUse: 'Residential' }},{reload:true});            
    });

    it('should promise to return a success response and response match with DpUse once showDeliveryPointDetails method is called', function() {
        var MockShowDeliveryPointDetailsData = [{"key":"","value":"Organisation"},{"key":"DPUSE","value":"Residential"}];
        var deferred = $q.defer();
        var deliveryPointDetails = {"deliveryPointId":"06f7a44d-ad06-449f-97eb-d1d38288e47b","street":"Chandos Road","subBuildingName":null}
        spyOn(mapFactory,'GetRouteForDeliveryPoint').and.returnValue(deferred.promise);
        spyOn($state,'go');

        advanceSearchService.showDeliveryPointDetails(deliveryPointDetails);
        deferred.resolve(MockShowDeliveryPointDetailsData);
        $rootScope.$apply();

        expect(mapFactory.GetRouteForDeliveryPoint).toHaveBeenCalledWith('06f7a44d-ad06-449f-97eb-d1d38288e47b');        
        expect($state.go).toHaveBeenCalledWith('deliveryPointDetails', {selectedDeliveryPoint:{deliveryPointId:'06f7a44d-ad06-449f-97eb-d1d38288e47b', street: 'Chandos Road',subBuildingName: null, routeName: null }},{reload:true});

    });
});