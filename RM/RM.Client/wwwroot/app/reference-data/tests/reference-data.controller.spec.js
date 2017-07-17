'use strict';
describe('Reference data: Controller', function () {
	var $scope;
	var $filter;
    var $rootScope;
	var referencedataApiService;
	var referenceDataConstants;

	beforeEach(function () {
		module('referencedata'); 
        module(function ($provide) {});
		$provide.factory('referencedataApiService', function($q){
			function initialize() {};

			function referenceData() {
                    deferred = $q.defer();
                    return deferred.promise;
                };

			return{
				initialize: initialize,
				referenceData: referenceData
			}
		});
	});        

	inject(function (_$controller_,_$rootScope_,_referencedataApiService_,_referenceDataConstants_,_$filter_){
			$rootScope = _$rootScope_;
            $scope = $rootScope.$new();
            referencedataApiService = _referencedataApiService_;
            referenceDataConstants = _referenceDataConstants_;
            $filter = _$filter_;

            vm = _$controller_('ReferenceDataController', {
                $scope : $scope,
                referencedataApiService : referencedataApiService,
                referenceDataConstants : referenceDataConstants,
                $filter : $filter
            });

            spyOn($scope, '$emit').and.callThrough();
            spyOn($scope, '$on').and.callThrough();
            $scope.$emit.and.stub();
        });

	it('should promise to return a success response once referenceData method is called', function() {
        var deferredSuccess = $q.defer(); 
        var response = {};
        spyOn(referencedataApiService, 'referenceData').and.returnValue(deferredSuccess.promise);
        
        vm.referenceData();
        
        deferredSuccess.resolve(response);
        $scope.$digest();

        expect(referencedataApiService.referenceData).toHaveBeenCalled();
    });

    });