describe('MapToolbar: Service', function() {
    var mapToolbarService;
    var $rootScope;
    var mapService;
    var CommonConstants;
    

    //Mock MockMapViewService properties define
    var MockMapViewService = {
        getMapButtons: function() { return ["select", "point", "line", "accesslink"]; }
    };

    //Mock CommonConstants value define
    var MockCommonConstants = {
        getMapButtons: function() {return []}
    };

    //Load Module
    beforeEach(module('mapToolbar','mapView','FMOApp'));

    beforeEach(function(){
        //angular.mock.module('mapView', []);
        //angular.mock.module('FMOApp', []);
        //module('mapToolbar', ['mapView']);
        module(function ($provide) {
        $provide.value('mapService', MockMapViewService);
        $provide.value('CommonConstants', MockCommonConstants);

        });
    });

    beforeEach(function() {
        inject(function($injector) {
            mapToolbarService = $injector.get('mapToolbarService');            
            //mapService = _mapService_;
            //CommonConstants = _CommonConstants_;
            //console.log('servies=='+ mapService);               
        });
    });

    describe('should be cal method getMapButtons', function(){
        beforeEach(function(){
            //Cal bydefault when suite start
            spyOn(mapToolbarService,'getMapButtons');
        });

        xit('should be initialize method and check toBeDefined', function(){
            
            mapToolbarService.getMapButtons();

            expect(mapToolbarService.getMapButtons).toHaveBeenCalled();
            expect(mapToolbarService.getMapButtons).toHaveBeenCalledWith();
            expect(mapToolbarService.getMapButtons).toBeDefined();            

        });

        xit('should be called mapService and return mapButtons', function(){

            var mapButtons = mapService.getMapButtons();
            expect(mapButtons).toContain('select');
            expect(mapButtons).toContain('point');
            expect(mapButtons).toContain('line');
            expect(mapButtons).toContain('accesslink');
            expect(mapButtons).toEqual(['select','point','line','accesslink']);
            expect(mapButtons.length).toEqual(4);

        });

    });

    describe('should be cal method autoSelect()',function(){

        beforeEach(function(){
            //Cal bydefault when suite start
            spyOn(mapToolbarService,'autoSelect');

        });

        it('should be initialize method and check toBeDefined', function(){
            
            mapToolbarService.autoSelect();

            expect(mapToolbarService.autoSelect).toHaveBeenCalled();
            expect(mapToolbarService.autoSelect).toHaveBeenCalledWith();
            expect(mapToolbarService.autoSelect).toBeDefined();            

        });

        it('should be call setSelectedButton and passthrough the argument', function(){
            //Common Constant - ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select" }
            
            //Calthrough the setSelectedButton
            spyOn(mapToolbarService,'setSelectedButton');
            mapToolbarService.setSelectedButton('select','select');

            //We are expect to
            expect(mapToolbarService.setSelectedButton).toHaveBeenCalledWith('select','select');
            expect(mapToolbarService.setSelectedButton).toBeDefined();

            //expect undefined in selected button
            expect(mapToolbarService.selectedButton).toBeUndefined();
        });

    });

    describe('should be cal method setSelectedButton(button,selectedButton)', function(){
        
        it('should be initialize method with params', function(){

            spyOn(mapToolbarService,'setSelectedButton');

            //method call with with out params 
            mapToolbarService.setSelectedButton();

            //method call sucess
            expect(mapToolbarService.setSelectedButton).toHaveBeenCalled(); //passed
            expect(mapToolbarService.setSelectedButton).toHaveBeenCalledWith();
            expect(mapToolbarService.setSelectedButton).toBeDefined();

            //Method call with params
            mapToolbarService.setSelectedButton('select','select');

            //expect sucess point
            expect(mapToolbarService.setSelectedButton).toHaveBeenCalled();
            expect(mapToolbarService.setSelectedButton).toHaveBeenCalledWith('select','select');
            expect(mapToolbarService.setSelectedButton).toBeDefined();

            //expect(selectedButton).toBe(null);

        });
    });

    describe('Should Be cal Method deselectButton(button)', function(){

        beforeEach(function(){
            spyOn(mapToolbarService,'deselectButton');
        })
        it('should be initialize the method with params', function(){

            //Cal to function
            mapToolbarService.deselectButton();

            expect(mapToolbarService.deselectButton).toHaveBeenCalled();
        });

        it('should be cal method and return to true', function(){

        });

        it('should be ccal method and return to false', function(){

        });
    });

    xdescribe('Should Be cal Method getShapeForButton(button)', function(){

        it('should be initialize the method with params', function(){

        });

        it('Should be call method and return to value', function(){

        })
    });

    xdescribe('Should Be cal Method showButton(button)', function(){

        it('should be initialize the method with params', function(){

        });

        it('Should be call method and return to value', function(){

        })
    });
});
