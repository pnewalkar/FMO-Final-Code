describe('MapToolbar: Controller', function () {

    var ctrl;
    var $scope;
    var mapToolbarService;

    var MockMapToolbarService = {
        getMapButtons: function () { return [] },
        showButton: function () { return [] },
        autoSelect: function () { return [] },
        getShapeForButton: function () { return [] },
        setSelectedButton: function () { return true }
    };

    beforeEach(module('mapToolbar'));

    beforeEach(module(function ($provide) {
        $provide.value('mapToolbarService', MockMapToolbarService);
    }));


    beforeEach(inject(function ($rootScope, $controller) {
        $scope = $rootScope.$new();
        ctrl = $controller('MapToolbarController', {
            $scope: $scope,
            mapToolbarService: MockMapToolbarService
        });

        spyOn($scope, '$emit').and.callThrough();
        spyOn($scope, '$on').and.callThrough();
        $scope.$emit.and.stub()

    }));

    it('should have selectedButton is `select` when initialised', function () {
        expect(ctrl.selectedButton).toBeDefined();
        expect(ctrl.selectedButton).toBe('select');
        expect(ctrl.selectedButton.length).toEqual(6);
    });

    it('should have an selected button of `line` after emit fired', function () {

        spyOn(MockMapToolbarService, 'setSelectedButton').and.returnValue(true);
        spyOn(ctrl, 'setSelectedButton').and.callThrough();

        //Set to define our map toolbar object
        var name = 'line';
        var shape = [];
        var enabled = true;

        var SetMapButtonObj = { name: name, shape: shape, enabled: enabled };

        //Call to ctrl method with mockService function
        ctrl.setSelectedButton(name);

        //emit fire with select maptoolchange
        expect(ctrl.selectedButton).toBe('line');
        expect(ctrl.setSelectedButton).toHaveBeenCalledWith('line');
        expect($scope.$emit).toHaveBeenCalled();
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', SetMapButtonObj);

    });

    it('should have an  selected button of `select` after emit fired', function () {

        spyOn(MockMapToolbarService, 'setSelectedButton').and.returnValue(false);
        spyOn(ctrl, 'setSelectedButton').and.callThrough();

        //Set to define our map toolbar object
        var name = 'select';
        var shape = [];
        var enabled = true;

        var SetMapButtonObj = { name: name, shape: shape, enabled: enabled };

        //Call to ctrl method with mockService function
        ctrl.setSelectedButton(name);

        //emit fire with select maptoolchange//emit fire with select maptoolchange
        expect(ctrl.selectedButton).not.toBe('selectt');
        expect(ctrl.setSelectedButton).toHaveBeenCalledWith('select');
        expect($scope.$emit).toHaveBeenCalled();
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', SetMapButtonObj);

    });


    it('should have initialize showButton', function () {
        spyOn(ctrl, 'showButton');
        ctrl.showButton('point');
        expect(ctrl.showButton).toHaveBeenCalledWith('point');

    });

});

