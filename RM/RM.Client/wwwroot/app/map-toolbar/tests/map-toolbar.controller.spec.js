describe('MapToolbar: Controller', function() {
    
    var ctrl;
    var $scope;
    var mapToolbarService;

    var MockMapToolbarService = {
        getMapButtons: function() {return []},
        showButton: function() {return []},
        autoSelect: function() {return []},
        getShapeForButton: function() { return []},
        setSelectedButton:  function() { return true }
     };

     beforeEach(function () {
        module('mapToolbar');
        module(function ($provide) {
            $provide.value('mapToolbarService',MockMapToolbarService);
        });

        inject(function (_$rootScope_, _$controller_,_mapToolbarService_) {
            $scope = _$rootScope_.$new();  
            mapToolbarService = _mapToolbarService_;  
            ctrl = _$controller_('MapToolbarController', {
                $scope: $scope,
                mapToolbarService: mapToolbarService
            });

            spyOn($scope, '$emit').and.callThrough();
            spyOn($scope, '$on').and.callThrough();
            $scope.$emit.and.stub();
        });
    });
   
    it('should have selectedButton is `select` when initialised', function() {        
        expect(ctrl.selectedButton).toBeDefined();  
        expect(ctrl.selectedButton).toBe('select');
    });

    it('should have an selected button of `line` after emit fired', function() {
        spyOn(MockMapToolbarService, 'setSelectedButton').and.returnValue(true);
        spyOn(ctrl,'setSelectedButton').and.callThrough();

        ctrl.setSelectedButton('line');

        expect(ctrl.selectedButton).toBe('line');
        expect(ctrl.setSelectedButton).toHaveBeenCalledWith('line');
        expect($scope.$emit).toHaveBeenCalled();
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', { name: 'line', shape: [ ], enabled: true });

    });    

    it('should have an  selected button of `select` after emit fired', function() {

        spyOn(MockMapToolbarService, 'setSelectedButton').and.returnValue(false);
        spyOn(ctrl,'setSelectedButton').and.callThrough();

        ctrl.setSelectedButton('select');

        expect(ctrl.selectedButton).toBe('select');
        expect(ctrl.setSelectedButton).toHaveBeenCalledWith('select');
        expect($scope.$emit).toHaveBeenCalled();
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', { name: 'select', shape: [ ], enabled: true });

    });       
});