'use strict';
describe('Access LInk : Factory', function() {
    var $q;    
    var $httpBackend;
    var GlobalSettings;
    var stringFormatService;
    var accessLinkAPIService;
    var $rootScope;    
    var MockGlobalSettings = {
        accessLinkApiUrl: "http://172.18.5.7/AccessLink/api",
        createAccessLink: "/accessLink/CreateManual/",
        getAdjustedPathLength: "/accessLink/GetWorkloadLength/",
        checkAccessLinkIsValid: "/accessLink/CheckAccessLinkIsValid/"
    };

   beforeEach(function(){
        module('accessLink');
        module(function ($provide) {
            $provide.value('GlobalSettings', MockGlobalSettings);
        });

        inject(function (_$rootScope_,_accessLinkAPIService_,_$httpBackend_, _$q_,_GlobalSettings_) {
            accessLinkAPIService = _accessLinkAPIService_;
            $httpBackend = _$httpBackend_;
            $q = _$q_;
            GlobalSettings = _GlobalSettings_;
            $rootScope: _$rootScope_;
        });
    });
  
    it('should promise to return a success response once GetAdjPathLength method is called', function() {        
        var response;
        var expectedUrl = GlobalSettings.accessLinkApiUrl+GlobalSettings.getAdjustedPathLength;
        $httpBackend.when('POST', expectedUrl, {result: "success"})
            .respond(200, {result:"success"});

        accessLinkAPIService.GetAdjPathLength({result: "success"})
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual({result: "success"});
    });
   
    it('should promise to return a error response once GetAdjPathLength method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.accessLinkApiUrl+GlobalSettings.getAdjustedPathLength;
        $httpBackend.when('POST', expectedUrl)
            .respond(500);

        accessLinkAPIService.GetAdjPathLength({result: "success"})
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });
    
    it('should promise to return a success response once CreateAccessLink method is called', function() {        
        var response;
        var expectedUrl = GlobalSettings.accessLinkApiUrl+GlobalSettings.createAccessLink;
        $httpBackend.when('POST', expectedUrl , {result: "success"})
            .respond(200, {result:"success"});

        accessLinkAPIService.CreateAccessLink({result: "success"})
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual({result: "success"});
    });
    
    it('should promise to return a error response once CreateAccessLink method is called', function() {        
        var response;
        var expectedUrl = GlobalSettings.accessLinkApiUrl+GlobalSettings.createAccessLink;
        $httpBackend.when('POST', expectedUrl)
            .respond(500);

        accessLinkAPIService.CreateAccessLink({result: "success"})
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });
    
    it('should promise to return a success response once CheckAccessLinkIsValid method is called', function() {        
        var response;
        var expectedUrl = GlobalSettings.accessLinkApiUrl+GlobalSettings.checkAccessLinkIsValid;
        $httpBackend.when('POST', expectedUrl, {result: "success"})
            .respond(200, {result:"success"});

        accessLinkAPIService.CheckAccessLinkIsValid({result: "success"})
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual({result: "success"});
    });
    
    it('should promise to return a error response once CheckAccessLinkIsValid method is called', function() {        
        var response;
        var expectedUrl = GlobalSettings.accessLinkApiUrl+GlobalSettings.checkAccessLinkIsValid;        
        $httpBackend.when('POST', expectedUrl)
            .respond(500);

        accessLinkAPIService.CheckAccessLinkIsValid({result: "success"})
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });
});
