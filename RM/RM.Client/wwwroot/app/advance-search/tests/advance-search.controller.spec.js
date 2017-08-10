'use strict';
describe('Advance Search: Controller', function() {
	var $q;
	var vm;
	var $scope;
	var $rootScope;
	var $mdDialog;
	var searchText;
	var advanceSearchService;
	var CommonConstants;

	beforeEach(function() {
		module('advanceSearch');
		module(function($provide){
            $provide.value('CommonConstants',{EntityType: { DeliveryPoint: "DeliveryPoint"}});
            $provide.value('searchText','road');
			$provide.factory('$mdDialog', function() {
                return {cancel:jasmine.createSpy(),
                          hide:jasmine.createSpy()};
            });
            $provide.factory('advanceSearchService', function($q){
                function queryAdvanceSearch(query){
                    var deferred = $q.defer();
                    deferred.resolve([{"displayText":" Flat 2, 123, Brighton Road, ","UDPRN":52161338,"type":"DeliveryPoint","ID":"12b97cf1-01cc-4e1b-9fae-615b93b03551","$$hashKey":"object:254","open":"false"}]);
                    return deferred.promise;
                }
                function onChangeItem(selectedItem){}
                return{
                    queryAdvanceSearch: queryAdvanceSearch,
                    onChangeItem: onChangeItem,
                };
            });
		});
		inject(function(_$rootScope_,_$controller_,_$mdDialog_,_searchText_,_advanceSearchService_,_CommonConstants_,_$q_){
			$rootScope = _$rootScope_;
			$mdDialog = _$mdDialog_;
			searchText = _searchText_;
			advanceSearchService = _advanceSearchService_;
			CommonConstants = _CommonConstants_;			
			$q = _$q_;

			vm = _$controller_('AdvanceSearchController',{
				$mdDialog: $mdDialog,
				searchText: searchText,
				advanceSearchService: advanceSearchService,
				CommonConstants: CommonConstants
			});
		});
	});

	it('should call queryAdvanceSearch when initialize method called ', function() {
		var deferred = $q.defer();
        spyOn(vm,'queryAdvanceSearch').and.returnValue(deferred.promise);
		vm.initialize();
        deferred.resolve();
        $rootScope.$apply();

		expect(vm.queryAdvanceSearch).toHaveBeenCalledWith('road');
        expect(vm.arrRoutes).toEqual([{"displayText":" Flat 2, 123, Brighton Road, ","UDPRN":52161338,"type":"DeliveryPoint","ID":"12b97cf1-01cc-4e1b-9fae-615b93b03551","$$hashKey":"object:254","open":"false"}]);
		expect(vm.chkRoute).toBe(true);
		expect(vm.chkPostCode).toBe(true);
		expect(vm.chkDeliveryPoint).toBe(true);
		expect(vm.chkStreetNetwork).toBe(true);
	});

    it('should window close when dialog button cancel', function() {
        vm.closeWindow();
        expect($mdDialog.cancel).toHaveBeenCalled();
    });

    it('should promise to return a success response once queryAdvanceSearch method is called', function() {
        var deferred = $q.defer();
        var response = [{"displayText":" Flat 2, 123, Brighton Road, ","UDPRN":52161338,"type":"DeliveryPoint","ID":"12b97cf1-01cc-4e1b-9fae-615b93b03551","$$hashKey":"object:254","open":"false"}];

        spyOn(advanceSearchService, 'queryAdvanceSearch').and.returnValue(deferred.promise);
        
        vm.queryAdvanceSearch('road');
        deferred.resolve(response);
        $rootScope.$apply();
        
        expect(advanceSearchService.queryAdvanceSearch).toHaveBeenCalledWith('road');
        expect(vm.arrRoutes).toEqual(response);        
    });

    it('should assign the parameter value to the respective variables when toggleSelection method is called', function() {
    	vm.toggleSelection(true);
    	expect(vm.chkRoute).toEqual(true);
    	expect(vm.chkPostCode).toEqual(true);
    	expect(vm.chkDeliveryPoint).toEqual(true);
    	expect(vm.chkStreetNetwork).toEqual(true);
    });

    it('should have item `open` in toggleList array', function() {
        vm.arrRoutes = [{"displayText":" Flat 2, 123, Brighton Road, ","UDPRN":52161338,"type":"DeliveryPoint","ID":"12b97cf1-01cc-4e1b-9fae-615b93b03551","$$hashKey":"object:254","open":"false"}];
    	vm.toggleList(false);
        expect(vm.arrRoutes[0].open).toBe(false);
    });

    it('should promise to return a success response once queryAdvanceSearch method is called', function() {
        var deferred = $q.defer();
        var selectedItem = {type:'DeliveryPoint'};
        var response = {id:"aff8e618-349c-4839-9968-0266c9b36eff","properties":{"name":null,"number":20,"postcode":"HP15 7UW","street_name":null,"type":"deliverypoint","organisationName":null,"departmentName":null,"mailVolume":null,"multipleOccupancyCount":10,"locality":"Hazlemere","deliveryPointId":"aff8e618-349c-4839-9968-0266c9b36eff","street":"Highfield Way","subBuildingName":null,"routeName":null}};
        spyOn(advanceSearchService, 'onChangeItem').and.returnValue(deferred.promise);
        vm.OnChangeItem(selectedItem);

        deferred.resolve(response);
        $rootScope.$apply();
        
        expect(advanceSearchService.onChangeItem).toHaveBeenCalledWith({type:'DeliveryPoint'});
        expect($mdDialog.hide).toHaveBeenCalled();
    });
});