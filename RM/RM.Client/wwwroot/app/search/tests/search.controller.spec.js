'use strict';
describe('Search: Controller', function() {
	var vm;
	var $q;
	var $timeout;
	var searchBusinessService;
	var $scope;
	var $rootScope;

	beforeEach(function() {
		module('search');
		module(function($provide){
			$provide.service('searchBusinessService',function($q){
				function resultSet(query){}
				function onEnterKeypress(searchText,results){}
				function OnChangeItem(selectedItem){}
				function advanceSearch(query){}
				return{
					resultSet: resultSet,
					onEnterKeypress: onEnterKeypress,
					OnChangeItem: OnChangeItem,
					advanceSearch: advanceSearch
				}
			});
		});
		inject(function(_$rootScope_,_$controller_,_$timeout_,_searchBusinessService_,_$q_){
			$rootScope = _$rootScope_;
			$scope = _$rootScope_.$new();
			$timeout = _$timeout_;
			searchBusinessService = _searchBusinessService_;
			$q = _$q_;

			vm = _$controller_('SearchController',{
				searchBusinessService: searchBusinessService,
				$timeout: $timeout
			});
		});
	});

	it('should return promise response success once resultSet method called', function() {        
        var deferred = $q.defer();
        var response = [{resultscount: [{"type":4,"count":2}], results: [{"value":"BT National IN","displayText":"Shown"},{"value":"BT National INDIA","displayText":"Shown"}], isResultDisplay: true}];

        spyOn(searchBusinessService,'resultSet').and.returnValue(deferred.promise);

        vm.resultSet('BT National IN');
        deferred.resolve(response);
        $rootScope.$apply();  

        expect(searchBusinessService.resultSet).toHaveBeenCalledWith('BT National IN');
        expect(vm.resultscount).toEqual(response[0].resultscount);
        expect(vm.results).toEqual(response[0].results);
        expect(vm.isResultDisplay).toEqual(response[0].isResultDisplay);        
    });

    it('should return contextTitle and results objects when onEnterKeypress method called', function() {
        spyOn(searchBusinessService,'onEnterKeypress').and.returnValue({results:[{displayText:"BTN1 National IN", type:'DeliveryPoint'}],contextTitle:'search'});
    	var result = vm.onEnterKeypress('BTN1');
    	expect(vm.results).toEqual([{displayText:"BTN1 National IN", type:'DeliveryPoint'}]);
    	expect(vm.contextTitle).toEqual('search');
    });

    it('should call `searchBusinessService.OnChangeItem` method when OnChangeItem method called', function() {
    	spyOn(searchBusinessService,'OnChangeItem');
    	vm.OnChangeItem('BTN1');
    	expect(searchBusinessService.OnChangeItem).toHaveBeenCalledWith('BTN1');    	
    });

    it('should set `searchText` value empty and `isResultDisplay` value false after timeout 1 second when onBlur method called ', function() {  
	  	vm.onBlur();
	  	$timeout.flush(1000); 

  		expect(vm.searchText).toEqual('');
  		expect(vm.isResultDisplay).toBe(false);
  		$timeout.verifyNoPendingTasks();
	});

	it('should call `searchBusinessService.advanceSearch` method when advanceSearch method called', function() {
    	spyOn(searchBusinessService,'advanceSearch');
    	vm.advanceSearch('BTN1');
    	expect(searchBusinessService.advanceSearch).toHaveBeenCalledWith('BTN1');    	
    });
});