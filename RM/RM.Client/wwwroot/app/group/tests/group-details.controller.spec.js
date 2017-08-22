'use strict';
describe('GroupDetails : Controller', function () {
    var vm;
    var $q;
    var groupService;
    var mapService;
    
    beforeEach(function () {
        module('group');
        module(function ($provide) {
            $provide.value('GlobalSettings', {});
            $provide.factory('groupAPIService', function ($q) {
                return {}
            });
            $provide.factory('groupService', function ($q) {
                return {
                    initialize: function () {
                        var deferred = $q.defer();
                        return deferred.promise;
                    }
                }
            });

            $provide.factory('mapService', function ($q) {
                function addSelectionListener(){}
                return {
                    addSelectionListener: addSelectionListener,
                }
            });
        });

        inject(function (_$controller_, _groupService_, _mapService_) {
            groupService = _groupService_;
            mapService = _mapService_;

            vm = _$controller_('GroupDetailsController', {
                groupService: groupService,
                mapService: mapService
            });

        });
    });

    it("Should return summarized count of addresses once getSummarizedAddresses() method is called", function () {
        var addedPoints = {values_:[
                {builing: 'Mars', postcode: 'PC1'},
                {builing: 'Mars', postcode: 'PC2'}, 
                {builing: 'Mars', postcode: 'PC1'},
                {builing: 'Mars', postcode: 'PC4'}]};        

        vm.getSummarizedAddresses();
        
        expect(vm.toggle).toBe(true);
        expect(vm.distinctPostcodes).not.toBe(null);
        expect(vm.summarizedCount).not.toBe(null);
        
    });
});
