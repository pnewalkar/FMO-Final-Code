describe('Reference Data: Controller', function () {    
    var vm;
    var $rootScope;
    var $scope;
    var referencedataApiService;
    var $filter;
    var referenceDataConstants;
    var filtereditems;
    var $q;
    var deferred;
    var $httpBackend;
    var GlobalSettings;
    var $http;

    var MockReferencedataApiService = {
        getReferenceData: function () { return []; }
    };
    var MockReferenceDataConstants = {
        DeliveryPointOperationalStatus: { DBCategoryName: "Delivery Point Operational Status", AppCategoryName: "DeliveryPointOperationalStatus", ReferenceDataNames: [] }
    };
      
    var MockGlobalSettings = {        
        getReferenceData : "./reference-data/ReferenceData.js"        
    };

    var mockFilter = function () {
        return 'whatyouwantittoreturn';
    };

    beforeEach(function () {
        module('referencedata');       

        module(function ($provide) {
            $provide.value('GlobalSettings', MockGlobalSettings);                       

            $provide.value('referenceDataConstants', MockReferenceDataConstants);

            $provide.value('filterFilter', mockFilter);            
        })
    });

    beforeEach(inject(function (_$controller_,
                                _$rootScope_,
                                _$http_,
                                _$httpBackend_,
                                _$filter_,
                                _referencedataApiService_,
                                _referenceDataConstants_,
                                _$q_,
                                _GlobalSettings_) {
        $rootScope = _$rootScope_;
        $scope = _$rootScope_.$new();        
        $q = _$q_;        
        deferred = _$q_.defer();
        referencedataApiService = _referencedataApiService_;        
        referenceDataConstants = _referenceDataConstants_;
        $http = _$http_;
        $httpBackend = _$httpBackend_;
        GlobalSettings = _GlobalSettings_;
        $filter = _$filter_;

        vm = _$controller_('ReferenceDataController', {
            $scope: $scope,
            referencedataApiService: referencedataApiService,
            $filter: $filter,
            referenceDataConstants: referenceDataConstants
        });
    }));      

    it('should call referenceData method when initialize method is called', function () {
        spyOn(vm, 'referenceData');
        vm.initialize();
        expect(vm.referenceData).toHaveBeenCalled();
    });

    it('should promise to return a success response once referenceData method is called', inject(function ($http) {

        var $scope = {};
        var response;
        var expectedUrl = GlobalSettings.getReferenceData;              

        referencedataApiService.getReferenceData()
          .success(function (data, status, headers, config) {              
              response = data;
              filtereditems = $filter('filter')(response, { result: "success" });
          });

        $httpBackend
         .when('GET', expectedUrl)
         .respond(200, { result: "success" });

        $httpBackend.flush();
        expect(response).toEqual({ result: "success" });
    }));    
});