'use strict';
describe('Map Panel: Controller', function() {
    var vm;
       
    beforeEach(function() {
        module('mapPanel');
        inject(function (_$controller_,_$timeout_) {
            vm = _$controller_('MapPanelController', {
                $timeout: _$timeout_,
            });
        });
    });    

    it('should collapsed `false` when togglePanel set `true` ', function() {
        vm.collapsed = false;
        vm.togglePanel();        
        expect(vm.collapsed).toBe(true);
    });

    it('should collapsed `true` when togglePanel set `false` ', function() {
        vm.collapsed = true;
        vm.togglePanel();        
        expect(vm.collapsed).toBe(false);
    });
});