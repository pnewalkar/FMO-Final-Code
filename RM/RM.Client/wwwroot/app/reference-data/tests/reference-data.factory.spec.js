'use strict';
describe('Reference Data : Factory', function() {
    var $httpBackend;
    var GlobalSettings;
    var referencedataApiService;
    var MockGlobalSettings = {
        referenceDataApiUrl:"http://172.18.5.7/ReferenceData/api",
        readJson: "/UI-string.json"
    };

    beforeEach(function(){
        module('referencedata');
        module(function ($provide) {
            $provide.value('GlobalSettings', MockGlobalSettings);
        });
        inject(function (_referencedataApiService_,_$httpBackend_,_GlobalSettings_) {
            referencedataApiService = _referencedataApiService_;
            $httpBackend = _$httpBackend_;
            GlobalSettings = _GlobalSettings_;
        });
    });

    it('should resolve the promise when readJson method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.readJson;
        var readJsonMockData = {"DELIVERY_POINT":"Delivery Point","ACCESS_LINK":"Access Link"};

        $httpBackend.when('GET', expectedUrl)
            .respond(200, readJsonMockData);

        referencedataApiService.readJson()
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(readJsonMockData);
    });

    it('should reject the promise when readJson method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.readJson;
        var readJsonMockData = {"DELIVERY_POINT":"Delivery Point","ACCESS_LINK":"Access Link"};

        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        referencedataApiService.readJson()
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

    it('should resolve the promise when getSimpleListsReferenceData method is called', function() {
        var response;
        var params = '/ReferenceDataManager/simpleLists?listName=DeliveryPointColor';
        var expectedUrl = GlobalSettings.referenceDataApiUrl+params;
        var getSimpleListsReferenceDataMockData = {item2:[{"id":"ef7176d0-120c-4fc3-b67f-e7f5b879ecb6","name":"Object Transparency"}]};

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getSimpleListsReferenceDataMockData);

        referencedataApiService.getSimpleListsReferenceData('DeliveryPointColor')
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual([{"id":"ef7176d0-120c-4fc3-b67f-e7f5b879ecb6","name":"Object Transparency"}]);
    });

    it('should reject the promise when getSimpleListsReferenceData method is called', function() {
        var response;
        var params = '/ReferenceDataManager/simpleLists?listName=DeliveryPointColor';
        var expectedUrl = GlobalSettings.referenceDataApiUrl+params;
        var getSimpleListsReferenceDataMockData = {item2:[{"id":"ef7176d0-120c-4fc3-b67f-e7f5b879ecb6","name":"Object Transparency"}]};

        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        referencedataApiService.getSimpleListsReferenceData('DeliveryPointColor')
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

    it('should resolve the promise when getNameValueReferenceData method is called', function() {
        var response;
        var params = '/ReferenceDataManager/nameValuePairs?appGroupName=Map_License_Information';
        var expectedUrl = GlobalSettings.referenceDataApiUrl+params;
        var getNameValueReferenceDataMockData = {item2:[{"id":"b5e7ed0d-c744-4e86-b0c0-0d967f595e93","group":"Map_License_Information"}]};

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getNameValueReferenceDataMockData);

        referencedataApiService.getNameValueReferenceData('Map_License_Information')
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual([{"id":"b5e7ed0d-c744-4e86-b0c0-0d967f595e93","group":"Map_License_Information"}]);
    });

    it('should reject the promise when getNameValueReferenceData method is called', function() {
        var response;
        var params = '/ReferenceDataManager/nameValuePairs?appGroupName=Map_License_Information';
        var expectedUrl = GlobalSettings.referenceDataApiUrl+params;
        var getNameValueReferenceDataMockData = {item2:[{"id":"b5e7ed0d-c744-4e86-b0c0-0d967f595e93","group":"Map_License_Information"}]};

        $httpBackend.when('GET', expectedUrl)
            .respond(500);

        referencedataApiService.getNameValueReferenceData('Map_License_Information')
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });

});