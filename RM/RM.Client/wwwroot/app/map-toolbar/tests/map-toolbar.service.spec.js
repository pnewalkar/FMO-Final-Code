'use strict';
describe('MapToolbar: Service', function() {
    var mapToolbarService;
    var mapService;
    var CommonConstants;
    
    var MockCommonConstants = {
        ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select" },
        GeometryType: { Point: "Point", LineString: "LineString" } ,
        pointTypes: { DeliveryPoint: { text: "Delivery Point", value: 'deliverypoint', style: "deliverypoint" }, AcessLink: { text: "Access Link", value: 'accesslink', style: "accesslink" }, Road: { text: "Road", value: 'roadlink', style: "roadlink" }, Selected: { text: "Selected", value: '', style: "deliverypoint" } },
    };
   
    beforeEach(function(){
        module('mapToolbar');
        module(function ($provide) {
            $provide.value('mapService', {
                getMapButtons: function() { return ["select", "point", "line", "accesslink"]; }
            });
            $provide.value('CommonConstants', MockCommonConstants);
        });

        inject(function(_mapToolbarService_,_mapService_,_CommonConstants_) {
            mapToolbarService = _mapToolbarService_;
            mapService = _mapService_;
            CommonConstants = _CommonConstants_;               
        });
    });

    it('should call mapService of getMapButtons when getMapButtons method is called', function(){
        spyOn(mapService,'getMapButtons');
        var mapButtons = mapToolbarService.getMapButtons();
        expect(mapService.getMapButtons).toHaveBeenCalled();
    });

    it('should return true if selected Button not selected ', function(){
        expect(mapToolbarService.setSelectedButton('select','point')).toBe(true);
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

    it('should call mapService of getMapButtons and return array when showButton method called ', function() {
        expect(mapToolbarService.showButton('select')).toBe(true);                    
    });

    it('should call setSelectedButton and return `true` when autoSelect method called', function() {
        expect(mapToolbarService.showButton('select')).toBe(true);        
    });

    it('should retun a value when selectDp mehod called', function() {
        expect(mapToolbarService.selectDP('select')).toEqual('select');
    });
});
