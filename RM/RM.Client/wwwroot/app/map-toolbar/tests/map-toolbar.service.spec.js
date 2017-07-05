'use strict';

describe('MapToolbar: Service', function() {
    var mapToolbarService;
    var mapService;
    var CommonConstants;
    

    //Mock MockMapViewService properties define
    var MockMapViewService = {
        getMapButtons: function() { return ["select", "point", "line", "accesslink","area","modify","delete"]; }
    };

    //Mock CommonConstants value define
    var MockCommonConstants = {
        GetSessionStorageItemType:"roleAccessData",
        EntityType: { DeliveryPoint: "DeliveryPoint", StreetNetwork: "StreetNetwork", Route: "Route", Postcode: "Postcode" },
        ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select", area: "area", del: "delete"},
        GeometryType: { Point: "Point", LineString: "LineString", Polygon: "Polygon" },
        pointTypes: { DeliveryPoint: { text: "Delivery Point", value: 'deliverypoint', style: "deliverypoint" }, AcessLink: { text: "Access Link", value: 'accesslink', style: "accesslink" }, Road: { text: "Road", value: 'roadlink', style: "roadlink" }, Selected: { text: "Selected", value: '', style: "deliverypoint" } },
    };

    //Load Module
    beforeEach(module('mapToolbar'));
   
    beforeEach(function(){
        module(function ($provide) {
        $provide.value('mapService', MockMapViewService);
        $provide.value('CommonConstants', MockCommonConstants);

        });
    });

    beforeEach(function() {
        inject(function(_mapToolbarService_,_mapService_,_CommonConstants_) {
            mapToolbarService = _mapToolbarService_;
            mapService = _mapService_;
            CommonConstants = _CommonConstants_;               
        });
    });


    it('should have initialize mapButtons', function(){

        spyOn(mapToolbarService,'getMapButtons');
        mapToolbarService.getMapButtons(true);

        expect(mapToolbarService.getMapButtons).toHaveBeenCalled();
        expect(mapToolbarService.getMapButtons).toBeDefined();            

    });

    it('should have initialize mapButtons except delete', function () {
        spyOn(mapService, 'getMapButtons').and.callFake(function () {
            return ["select", "point", "line", "accesslink", "area", "modify","delete"]});
        var mapButtons =  mapToolbarService.getMapButtons(false);

        expect(mapService.getMapButtons).toHaveBeenCalled();
        expect(mapButtons).toBeDefined();
        expect(mapButtons.length).toEqual(6);

    });

    it('should be return list of mapButton value', function(){

        var mapButtons = MockMapViewService.getMapButtons();
        expect(mapButtons).toContain('select');
        expect(mapButtons).toContain('point');
        expect(mapButtons).toContain('line');
        expect(mapButtons).toContain('accesslink');
        expect(mapButtons).toEqual(["select", "point", "line", "accesslink", "area", "modify", "delete"]);
        expect(mapButtons.length).toEqual(7);

    });

    it('should be initialize autoSelect button', function(){
        
        spyOn(mapToolbarService,'autoSelect').and.callThrough();
        mapToolbarService.autoSelect();

        expect(mapToolbarService.autoSelect).toHaveBeenCalled();
    });

        
    it('should be return false if seleccted Button already select ', function(){

        spyOn(mapToolbarService,'setSelectedButton').and.callThrough();
        
        mapToolbarService.selectedButton = 'select';
        mapToolbarService.setSelectedButton('select','select');

        //expect false
        expect(mapToolbarService.setSelectedButton).toHaveBeenCalled();
        expect(mapToolbarService.setSelectedButton('select','select')).toBe(false);
    });

    it('should return undefined for a non-shape button', function() {
        var shape = mapToolbarService.getShapeForButton('point');
        expect(shape).toBe('Point');
    });

    it('should return LineString for a shapeButton line or accesslink button', function() {
        var shape = mapToolbarService.getShapeForButton('line');
        expect(shape).toBe('LineString');

        var shape = mapToolbarService.getShapeForButton('accesslink');
        expect(shape).toBe('LineString');
    });

    it('should return Polygon for area button', function () {
        var shape = mapToolbarService.getShapeForButton('area');
        expect(shape).toBe('Polygon');
    });

    it('Should be showButton return true', function(){
        var myVal = mapToolbarService.showButton('select');
        expect(myVal).toBe(true);
    });

    it('showButton should return true for modify', function () {
        var myVal = mapToolbarService.showButton('modify');
        expect(myVal).toBe(true);
    });

    it('showButton should return true for delete', function () {
        var myVal = mapToolbarService.showButton('delete');
        expect(myVal).toBe(true);
    });
});
