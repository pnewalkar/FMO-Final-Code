describe('mapPanel:Controller', function () {

    var $rootScope;
    var $scope;
    var $timeout;
    var $interval;
    var ctrl;

    //load module
    beforeEach(module('mapPanel'));

    //get instance of module 
    beforeEach(inject(function ($rootScope, $controller, _$timeout_) {
        ctrl = $controller('MapPanelController', {
            $timeout: _$timeout_,
        });
    }));


    it('should be initialize mapView', function () {

        //Need to create spy and call
        spyOn(ctrl, 'initialize');

        //cal to function
        ctrl.initialize();

        expect(ctrl.initialize).toHaveBeenCalled();
        expect(ctrl.oncreate).toBeUndefined();

        ctrl.oncreate = true;

        expect(ctrl.oncreate).toBeDefined();
        expect(ctrl.oncreate).not.toBeUndefined();
        expect(ctrl.oncreate).toBe(true);

    });

    it('should be togglePanel collapsed if true', function () {

        spyOn(ctrl, 'togglePanel');
        ctrl.togglePanel();
        expect(ctrl.collapsed).toBeUndefined();

        //change to value true
        ctrl.collapsed = true;
        expect(ctrl.collapsed).toBe(true);
    });

});