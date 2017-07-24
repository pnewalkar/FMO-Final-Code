describe('Layers: Factory', function(){
	var $http;
	var $q;
	var $GlobalSettings;
	var $httpBackend;
	var MockGlobalSettings = {
        deliveryPointApiUrl:"http://172.18.5.7/DeliveryPoint/api",
        fetchDeliveryPointsByBoundingBox: "/deliverypointmanager/deliverypoints?bbox="
    };

    beforeEach(function(){
    	module('layers')
    	module(function ($provide) {
            $provide.value('GlobalSettings', MockGlobalSettings);
        });
        inject(function (_layersAPIService_,_$httpBackend_,_GlobalSettings_,_$q_) {
            layersAPIService = _layersAPIService_;
            $httpBackend = _$httpBackend_;
            GlobalSettings = _GlobalSettings_;
            $q = _$q_;
        });
    });

    it('should resolve the promise when fetchDeliveryPoints method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.deliveryPointApiUrl + GlobalSettings.fetchDeliveryPointsByBoundingBox + extent.join(',');
        var fetchDeliveryPointsMockData = {"fetchDeliveryPoints":"Delivery Point","ACCESS_LINK":"Access Link"};

        $httpBackend.when('GET', expectedUrl)
            .respond(200, fetchDeliveryPointsMockData);

        layersAPIService.fetchDeliveryPoints(extent, authData)
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(fetchDeliveryPointsMockData);
    });
});