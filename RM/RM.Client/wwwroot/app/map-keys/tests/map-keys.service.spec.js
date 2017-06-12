describe('mapKey: Service', function () {

    var mapStylesFactory;
    var mapService;
    var CommonConstants;
    var mapKeyService;

    //
    var getSylefun = function activeStyle(feature) {
        return [];
    }

    //MockSet
    var MockMapKeyService = {
        mapLayers: function () { return [] },

    };

    //MockSet
    var MockMapStylesFactory = {
        getStyle: function () { return getSylefun; },
        styleTypes: {}
    };

    //MockSet
    var MockCommonConstants = {
        pointTypes: {
            DeliveryPoint: {
                text: "Delivery Point",
                value: 'deliverypoint',
                style: "deliverypoint"
            },
            AcessLink: {
                text: "Access Link",
                value: 'accesslink',
                style: "accesslink"
            },
            Road: {
                text: "Road",
                value: 'roadlink',
                style: "roadlink"
            },
            Selected: {
                text: "Selected",
                value: '',
                style: "deliverypoint"
            }
        },
    };


    beforeEach(module('mapKey'));

    //load to our provider
    beforeEach(function () {
        module(function ($provide) {
            $provide.value('mapStylesFactory', MockMapStylesFactory);
            $provide.value('mapService', MockMapKeyService);
            $provide.value('CommonConstants', MockCommonConstants);

        });
    });


    //Inject to get instance
    beforeEach(inject(function (_mapKeyService_, _mapStylesFactory_, _mapService_, _CommonConstants_) {
        mapKeyService = _mapKeyService_;
        mapStylesFactory = _mapStylesFactory_;
        mapService = _mapService_;
        CommonConstants = _CommonConstants_;

    }));

    it('should be return showKey true if id null', function () {

        spyOn(mapKeyService, 'showKey').and.callThrough();

        var id = '';
        var val = mapKeyService.showKey(id);
        expect(val).toEqual(true);
    });

    it('should be return showKey false if id not null', function () {

        spyOn(mapKeyService, 'showKey').and.callThrough();

        var id = '2';
        var val = mapKeyService.showKey(id);
        expect(val).toEqual(false);
    });

    it('should be initialize method and return pointTypes Object Values', function () {
        spyOn(mapKeyService, 'initialize').and.callThrough();

        //cal to mapStyleFactory
        var result = mapKeyService.initialize();

        //check with argument based object what we expect
        var arg1 = { text: 'Selected', id: '' };
        var arg2 = { text: 'Delivery Point', id: 'deliverypoint', style: [] };
        var arg3 = { text: 'Access Link', id: 'accesslink', style: [] };
        var arg4 = { text: 'Road', id: 'roadlink', style: [] };

        expect(mapKeyService.initialize).toHaveBeenCalled();
        expect(result[0]).toEqual(jasmine.objectContaining(arg1));
        expect(result[1]).toEqual(jasmine.objectContaining(arg2));
        expect(result[2]).toEqual(jasmine.objectContaining(arg3));
        expect(result[3]).toEqual(jasmine.objectContaining(arg4));

    });
});