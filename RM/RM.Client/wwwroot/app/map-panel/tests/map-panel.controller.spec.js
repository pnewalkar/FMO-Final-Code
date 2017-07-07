describe('Map Panel: Controller', function() {

    var $rootScope;
    var $scope;
    var $timeout;
    var $interval;
    var ctrl;

    beforeEach(module('mapPanel'));

    beforeEach(inject(function ($rootScope, $controller,_$timeout_) {
        ctrl = $controller('MapPanelController', {
            $timeout: _$timeout_,
        });
    }));

    it('should collapsed `false` when togglePanel set `true` ', function() {
        ctrl.collapsed = false;
        ctrl.togglePanel();        
        expect(ctrl.collapsed).toBe(true);
    });

    it('should collapsed `true` when togglePanel set `false` ', function() {
        ctrl.collapsed = true;
        ctrl.togglePanel();        
        expect(ctrl.collapsed).toBe(false);
    });

});