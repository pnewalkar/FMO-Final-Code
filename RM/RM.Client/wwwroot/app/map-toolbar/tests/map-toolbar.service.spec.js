'use strict';
describe('MapToolbar: Service', function() {
    var mapToolbarService;
    var CommonConstants;
    
    var MockCommonConstants = {
        GetSessionStorageItemType:"roleAccessData",
        EntityType: { DeliveryPoint: "DeliveryPoint", StreetNetwork: "StreetNetwork", Route: "Route", Postcode: "Postcode" },
        ButtonShapeType: { point: "point", line: "line", accesslink: "accesslink", select: "select", area: "area", del: "delete"},
        GeometryType: { Point: "Point", LineString: "LineString", Polygon: "Polygon" },
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
            CommonConstants = _CommonConstants_;               
        });
    });


    it('should have initialize mapButtons', function(){

        spyOn(mapToolbarService,'getMapButtons');
        mapToolbarService.getMapButtons(true);

        expect(mapToolbarService.getMapButtons).toHaveBeenCalled();
        expect(mapToolbarService.getMapButtons).toBeDefined();            

    it('should call mapService of getMapButtons when getMapButtons method is called', function(){
        spyOn(mapService,'getMapButtons');
        var mapButtons = mapToolbarService.getMapButtons();
        expect(mapService.getMapButtons).toHaveBeenCalled();
    });

    it('should have initialize mapButtons except delete', function () {
        var mapButtons =  mapToolbarService.getMapButtons(false);

        expect(mapButtons).toEqual(["select", "point", "line", "accesslink", "area", "modify"]);
        expect(mapButtons.length).toEqual(6);

    });

    it('should return the entire list of toolbar buttons', function () {

        var mapButtons = mapToolbarService.getMapButtonsList();
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
        var isButtonExists = mapToolbarService.showButton('select');
        expect(isButtonExists).toBe(true);
    });

    it('showButton should return true for modify', function () {
        var isButtonExists = mapToolbarService.showButton('modify');
        expect(isButtonExists).toBe(true);
    });

    it('showButton should return true for delete', function () {
        var isButtonExists = mapToolbarService.showButton('delete');
        expect(isButtonExists).toBe(true);
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
