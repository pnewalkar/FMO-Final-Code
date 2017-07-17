'use strict';
describe('Reference Data : Factory', function() {
	var $q;
    var defrred;
    var $rootScope;
    var $httpBackend;
    var expectedUrl;
    var GlobalSettings;
    var referencedataApiService;

    var MockGlobalSettings = {
        apiUrl: undefined
    };

    beforeEach(function(){
        module('referencedata');
        module(function ($provide) {
            $provide.value('GlobalSettings', MockGlobalSettings);
        });
        inject(function (_$rootScope_,_referencedataApiService_,_$httpBackend_, _$q_,_GlobalSettings_) {
            referencedataApiService = _referencedataApiService_;
            $httpBackend = _$httpBackend_;
            $q = _$q_;
            GlobalSettings = _GlobalSettings_;
            $rootScope: _$rootScope_;
        });
    });

    it('should promise to return a success response once getReferenceData method is called', function() {
        expect(getReferenceData).toHaveBeenCalled();
    });

    it('should promise to return a success response once readJson method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.apiUrl+'readJsonData';
        var getReferenceDataMockData = [{}]; 

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getReferenceDataMockData);

        referencedataApiService.readJson('readJsonData')
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();

        expect(response).toEqual(readJsonMockData);
    });

    it('should promise to return a success response once getSimpleListsReferenceData method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.apiUrl+'/ReferenceDataManager/simpleLists?listName=';
        var getSimpleListsReferenceDataMockData = [{}]; 

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getSimpleListsReferenceDataMockData);

        referencedataApiService.getSimpleListsReferenceData('listName')
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();

        expect(response).toEqual(getSimpleListsReferenceDataMockData);
    });

    it('should promise to return a success response once getNameValueReferenceData method is called', function() {
        var response;
        var expectedUrl = GlobalSettings.apiUrl+'/ReferenceDataManager/nameValuePairs?appGroupName=';
        var getNameValueReferenceDataMockData = [{}]; 

        $httpBackend.when('GET', expectedUrl)
            .respond(200, getNameValueReferenceDataMockData);

        referencedataApiService.getNameValueReferenceData('appGroupName')
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();

        expect(response).toEqual(getNameValueReferenceDataMockData);
    });

});