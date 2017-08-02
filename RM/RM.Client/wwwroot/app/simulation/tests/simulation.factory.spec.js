describe('Simulation: Factory', function () {
    var $q;
    var $httpBackend;
    var GlobalSettings;
    var stringFormatService;
    var simulationAPIService;

    var MockGlobalSettings = {
    	apiUrl: '',
        deliveryRouteApiUrl: "http://localhost:50235/api",
        getRouteLogStatus: "/RouteLog/RouteLogsStatus",
        unitManagerApiUrl: "http://localhost:50239/api",
        getDeliveryRouteScenario: "/UnitManager/scenario/{0}/{1}",
        getRouteLogSimulationStatus: "/RouteSimulation/RouteLogsStatus",
        getRouteSimulationScenario: "/UnitManager/scenario/{0}/{1}",
        getDeliveryRoutes: "/DeliveryRouteManager/deliveryroute/{0}/DisplayText"
    };

    var MockStringFormatService = {
        Stringformat: function() { 
        var theString = arguments[0];
           for (var i = 1; i < arguments.length; i++) {
               var regEx = new RegExp("\\{" + (i - 1) + "\\}", "gm");
               theString = theString.replace(regEx, arguments[i]);
           }
           return theString;
        }
    };
    
    beforeEach(function(){
        module('simulation');
        module(function ($provide) {
       	    $provide.value('GlobalSettings', MockGlobalSettings);
            $provide.value('stringFormatService', MockStringFormatService);
        });

        inject(function (_simulationAPIService_,_$httpBackend_, _$q_,_GlobalSettings_,_stringFormatService_) {
            simulationAPIService = _simulationAPIService_;
            $httpBackend = _$httpBackend_;
            $q = _$q_;
            GlobalSettings = _GlobalSettings_;
            stringFormatService = _stringFormatService_;
        });
    });
   
    it('should promise to return a success response once GetAdjPathLength method is called', function() {        
        var response; 
        var getStatusMockData = {"id":"87216073-e731-4b8c-9801-877ea4891f7e","listName":"Operational Status","maintainable":false,"listItems":[{"id":"9c1e56d7-5397-4984-9cf0-cd9ee7093c88","name":null,"value":"Live","displayText":null,"description":"Live"},{"id":"bee6048d-79b3-49a4-ad26-e4f5b988b7ab","name":null,"value":"Not Live","displayText":null,"description":"Not Live"}]};        
        var expectedUrl = GlobalSettings.getRouteLogStatus;

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getStatusMockData);

        simulationAPIService.getStatus()
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(getStatusMockData);
    });
   
    it('should promise to return a error response once GetAdjPathLength method is called', function() {
        var response; 
        var expectedUrl = GlobalSettings.apiUrl+GlobalSettings.getRouteLogStatus;

        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        simulationAPIService.getStatus()
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

    it('should promise to return a success response once getScenario method is called', function() {        
        var response; 
        var getRouteSimulationScenarioParams = '/UnitManager/scenario/9c1e56d7-5397-4984-9cf0-cd9ee7093c88/b51aa229-c984-4ca6-9c12-510187b81050';
        var getScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];        
        var expectedUrl = GlobalSettings.unitManagerApiUrl+getRouteSimulationScenarioParams;
        
        $httpBackend.when('GET', expectedUrl)
            .respond(200, getScenarioMockData);

        simulationAPIService.getScenario("9c1e56d7-5397-4984-9cf0-cd9ee7093c88","b51aa229-c984-4ca6-9c12-510187b81050")
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(getScenarioMockData);
    });

     it('should promise to return a error response once getScenario method is called', function() {        
        var response; 
        var getRouteSimulationScenarioParams = '/UnitManager/scenario/9c1e56d7-5397-4984-9cf0-cd9ee7093c88/b51aa229-c984-4ca6-9c12-510187b81050';
        var expectedUrl = GlobalSettings.unitManagerApiUrl+getRouteSimulationScenarioParams;
        
        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        simulationAPIService.getScenario("9c1e56d7-5397-4984-9cf0-cd9ee7093c88","b51aa229-c984-4ca6-9c12-510187b81050")
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

    it('should promise to return a success response once getRoutes method is called', function() {        
        var response; 
        var getRouteSimulationScenarioParams = '/DeliveryRouteManager/deliveryroute/9c1e56d7-5397-4984-9cf0-cd9ee7093c88/DisplayText';
        var getScenarioMockData = [{"scenarioName":"Worthing Delivery Office - Baseline weekday","id":"b51aa229-c984-4ca6-9c12-510187b81050"}];        
        var expectedUrl = GlobalSettings.deliveryRouteApiUrl+getRouteSimulationScenarioParams;
        
        $httpBackend.when('GET', expectedUrl)
            .respond(200, getScenarioMockData);

        simulationAPIService.getRoutes("9c1e56d7-5397-4984-9cf0-cd9ee7093c88")
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(getScenarioMockData);
    });

    it('should promise to return a error response once getRoutes method is called', function() {        
        var response; 
        var getRouteSimulationScenarioParams = '/DeliveryRouteManager/deliveryroute/9c1e56d7-5397-4984-9cf0-cd9ee7093c88/DisplayText';
        var expectedUrl = GlobalSettings.deliveryRouteApiUrl+getRouteSimulationScenarioParams;
        
        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        simulationAPIService.getRoutes("9c1e56d7-5397-4984-9cf0-cd9ee7093c88")
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

});
