'use strict';
describe('MapToolbar: Controller', function() {    
    var vm;
    var $scope;
    var $rootScope;
    var mapToolbarService;
    var MockMapToolbarService = {
        getMapButtons: function() {return []; },
        showButton: function() {return []; },
        autoSelect: function() {return []; },
        getShapeForButton: function() { return []},
        setSelectedButton: function () { return; }
     };

     beforeEach(function () {
        module('mapToolbar');
        module(function ($provide) {
            $provide.value('mapToolbarService',MockMapToolbarService);
        });

        inject(function (_$rootScope_, _$controller_,_mapToolbarService_) {
            $scope = _$rootScope_.$new();  
            $rootScope = _$rootScope_;
            mapToolbarService = _mapToolbarService_;  
            vm = _$controller_('MapToolbarController', {
                $scope: $scope,
                $rootScope: $rootScope,
                mapToolbarService: mapToolbarService
            });

            spyOn($scope, '$emit').and.callThrough();
            spyOn($scope, '$on').and.callThrough();
            $scope.$emit.and.stub();
        });
    });    

    it('should have selectedButton is `select` when initialised', function() { 
        expect(vm.selectedButton).toBe('select');
    });

    it('should set isObjectSelected value as `false`', function() {        
        expect(vm.isObjectSelected).toBe(false);
    });

    it('should have selected button of `line` after emit fired when setSelectedButton method called', function() {
        spyOn(mapToolbarService, 'setSelectedButton').and.returnValue(true);        
        vm.setSelectedButton('line');
        expect(vm.selectedButton).toBe('line');
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', { name: 'line', shape: [ ], enabled: true });
    });    

    it('should have selected button of `select` after emit fired when setSelectedButton method called', function() {
        spyOn(mapToolbarService, 'setSelectedButton').and.returnValue(false);
        vm.setSelectedButton('select');
        expect(vm.selectedButton).toBe('select');
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', { name: 'select', shape: [ ], enabled: true });
    });    

    it('should call `mapToolbarService of showButton method` when showButton method called', function() {
        spyOn(mapToolbarService,'showButton');
        vm.showButton('select');
        expect(mapToolbarService.showButton).toHaveBeenCalledWith('select');
    });

    it('should emit fired when autoSelect method called', function() {
        spyOn(mapToolbarService,'autoSelect');
        vm.autoSelect();
        expect(vm.selectedButton).toBe('select');
        expect(mapToolbarService.autoSelect).toHaveBeenCalled();
        expect($scope.$emit).toHaveBeenCalledWith('mapToolChange', { "name": "select", "shape": 'undefined', enabled: true });
    });

    it('should return list of contains buttons when getMapButtons method called', function () {
        spyOn(MockMapToolbarService, 'getMapButtons').and.callFake(function () {
            return ["select", "point", "line", "accesslink", "area", "modify", "delete"];
        });
        vm.getMapButtons();
        expect(vm.mapButtons).toEqual(["select", "point", "line", "accesslink", "area", "modify", "delete"]);
        expect(vm.mapButtons.length).toEqual(7);
    });    
});