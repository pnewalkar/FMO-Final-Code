'use strict';
describe('MapToolbar: Service', function() {
    var mapToolbarService;
    var CommonConstants;
    var MockCommonConstants = {
        ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select", area: "area", del: "delete" },
        GeometryType: { Point: "Point", LineString: "LineString" , Polygon : "Polygon"},
        pointTypes: { DeliveryPoint: { text: "Delivery Point", value: 'deliverypoint', style: "deliverypoint" }, AcessLink: { text: "Access Link", value: 'accesslink', style: "accesslink" }, Road: { text: "Road", value: 'roadlink', style: "roadlink" }, Selected: { text: "Selected", value: '', style: "deliverypoint" } },
    };
   
    beforeEach(function(){
        module('mapToolbar');
        module(function ($provide) {           
            $provide.value('CommonConstants', MockCommonConstants);
        });
        inject(function(_mapToolbarService_,_CommonConstants_) {
            mapToolbarService = _mapToolbarService_;
            CommonConstants = _CommonConstants_;               
        });
    });   

    it('should return contain a list of array when getMapButtonsList method called', function() {
        var mapButtons = mapToolbarService.getMapButtons();
        expect(mapButtons).toContain('select');
        expect(mapButtons).toContain('point');
        expect(mapButtons).toContain('line');
        expect(mapButtons).toContain('accesslink');
        expect(mapButtons).toContain('area');
        expect(mapButtons).toContain('modify');
        expect(mapButtons).toContain('delete');
        expect(mapButtons.length).toEqual(7);
    });

    it('should delete a Button when getMapButtons argument isObjectSelected is `false`', function() {
        mapToolbarService.getMapButtons(false);
        var mapButtons = mapToolbarService.getMapButtons();        
        expect(mapButtons.length).toEqual(6);
    });

    it('should add a `delete` Button when getMapButtons argument isObjectSelected is `true`', function() {
        mapToolbarService.getMapButtons(true);
        var mapButtons = mapToolbarService.getMapButtons();       
        expect(mapButtons.length).toEqual(7);
    });

    it('should return `true` when called setSelectedButton method and it matched with selected Button', function(){
        expect(mapToolbarService.setSelectedButton()).toBe(true);
    });

    it('should return `false` when called setSelectedButton method and it not matched with selected Button', function(){
        expect(mapToolbarService.setSelectedButton(null,null)).toBe(false);
    });

    it('should return `true` when called deselectButton method and it matched with selected Button', function(){
        expect(mapToolbarService.deselectButton(null)).toBe(true);
    });       

    it('should return point for a point', function() {
        var shape = mapToolbarService.getShapeForButton('point');
        expect(shape).toBe('Point');
    }); 

    it('should return LineString for a line or accesslink', function() {
        var shape = mapToolbarService.getShapeForButton('line');
        expect(shape).toBe('LineString');

        var shape = mapToolbarService.getShapeForButton('accesslink');
        expect(shape).toBe('LineString');
    });   

    it('should return Polygon for a area', function() {
        var shape = mapToolbarService.getShapeForButton('area');
        expect(shape).toBe('Polygon');
    }); 

    it('should return undefined for a non-shape button', function() {
        var shape = mapToolbarService.getShapeForButton('select');
        expect(shape).toBeUndefined();
    });

    it('should call mapService of getMapButtons and return array when showButton method called ', function() {
        expect(mapToolbarService.showButton('select')).toBe(true); 
    });    

    it('should retun a value when selectDp mehod called', function() {
        expect(mapToolbarService.selectDP('select')).toEqual('select');
    });
});
