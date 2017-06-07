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

    beforeEach(module('mapToolbar'));

    beforeEach(module(function($provide){
        $provide.value('mapToolbarService',MockMapToolbarService);
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
    

    describe('should be set byDefault variables ', function()
    {
        it('should have an selectedButton is select', function() {        
            expect(ctrl.selectedButton).toBe('select');
            expect(ctrl.selectedButton).not.toBeUndefined();  
            expect(ctrl.selectedButton).toContain('select');
            expect(ctrl.selectedButton.length).toEqual(6);      
        });
    });

    describe('should be call method setSelectedButton', function()
    {
        it('should set $emit ture and a $emit emitter fired', function() {

            spyOn(MockMapToolbarService, 'setSelectedButton').and.returnValue(true);

            //Set to define our map toolbar object
            var name = ['line'];
            var shape = [];
            var enabled = true;            

            var SetMapButtonObj = { name: name, shape: shape, enabled: enabled };

            //Call to ctrl method with mockService function
            ctrl.setSelectedButton(name);
            MockMapToolbarService.getShapeForButton(name);

            //emit fire with select maptoolchange
            expect($scope.$emit).toHaveBeenCalled();
            expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', SetMapButtonObj);

        });    

        it('should set the $emit is false and $emit emitter fired', function() {

            spyOn(MockMapToolbarService, 'setSelectedButton').and.returnValue(false);

            //Set to define our map toolbar object
            var name = 'select';
            var shape = [];
            var enabled = true;            

            var SetMapButtonObj = { name: name, shape: shape, enabled: enabled };

            //Call to ctrl method with mockService function
            ctrl.setSelectedButton(name);
            MockMapToolbarService.getShapeForButton(name);

            //emit fire with select maptoolchange
            expect($scope.$emit).toHaveBeenCalled();
            expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', SetMapButtonObj);

        });       

    });


    describe('should be call method showButton' ,  function() 
    {
        it('should be initialize the method', function(){            
            spyOn(ctrl, 'showButton').and.callThrough();            
            expect(ctrl).toBeDefined();
            expect(ctrl.showButton.length).toBe(0);            
        });
    });

    describe('should be call method autoSelect', function() 
    {
        it('should be initialize function', function(){            
            expect(ctrl.autoSelect).toBeUndefined();
        });
    });
    
});

