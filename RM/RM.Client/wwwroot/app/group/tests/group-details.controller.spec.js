'use strict';
describe('GroupDetails : Controller', function () {
    var vm;
    var $q;
    var groupService;
    
    beforeEach(function () {
        module('group');
        module(function ($provide) {
            $provide.factory('groupService', function ($q) {
                return {
                    initialize: function () {
                        var deferred = $q.defer();
                        return deferred.promise;
                    }
                }
            });
        });

        inject(function (_$controller_, _groupService_) {
            groupService = _groupService_;
            vm = _$controller_('GroupDetailsController', {
                groupService: groupService,
            });

        });
    });

    it("Spec-1 : Get Summarized Addresses", function () {

        spyOn(vm, 'getSummarizedAddresses').and.callThrough();
        vm.getSummarizedAddresses();
        
        expect(vm.getSummarizedAddresses).toHaveBeenCalled();
        expect(vm.summarizedCount[0]).toBe("BN1 1HS");
        expect(vm.summarizedCount[0].postcode).toBe("BN1 1HS");
        expect(vm.summarizedCount[0].deliveyPoints).toBe(2);
    });
});
