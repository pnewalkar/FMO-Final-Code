'use strict';
describe('Manage Access: Factory', function() {
	var $rootScope;
	var $httpBackend;
	var GlobalSettings;
	var $q;
	var MockGlobalSettings = {
		actionManagerApiUrl: 'http://localhost:43423/ActionManager/api',
		getToken: '/token'
	};

	beforeEach(function() {
		module('manageAccess');
		module(function($provide){
			$provide.value('GlobalSettings', MockGlobalSettings);
		});

		inject(function(_manageAccessAPIService_,_$q_,_$httpBackend_,_GlobalSettings_){
			manageAccessAPIService = _manageAccessAPIService_;
			$q = _$q_;
			$httpBackend = _$httpBackend_;
			GlobalSettings = _GlobalSettings_;
		});
	});

	it('should promise to return a success response once getToken method is called', function() {
		var userdata = 'username=shobharam.katiya&unitguid=null'
        var expectedUrl = GlobalSettings.actionManagerApiUrl + GlobalSettings.getToken;
        var headers = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };
        var getTokenData = {AccessToken:"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJzaG9iaGFyYW0ua2F0aXlhIiwianRpIjoiYmRmMmQ1ZTQtYzgyZS00ZjU1LWE3MjM",Username: "shobharam.katiya"};           
       	var response;        

        $httpBackend.when('POST', expectedUrl, userdata)
            .respond(200, {AccessToken:"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJzaG9iaGFyYW0ua2F0aXlhIiwianRpIjoiYmRmMmQ1ZTQtYzgyZS00ZjU1LWE3MjM",Username: "shobharam.katiya"});

        manageAccessAPIService.getToken(userdata)
            .then(function(data) {
                response = data;
            });

        $httpBackend.flush();
        expect(response).toEqual(getTokenData);
    });
   
	it('should promise to return a error response once getToken method is called', function() {
        var userdata = 'username=shobharam.katiya&unitguid=null'
        var expectedUrl = GlobalSettings.actionManagerApiUrl + GlobalSettings.getToken;
        var headers = { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } };
        var getTokenData = {AccessToken:"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJzdWIiOiJzaG9iaGFyYW0ua2F0aXlhIiwianRpIjoiYmRmMmQ1ZTQtYzgyZS00ZjU1LWE3MjM",Username: "shobharam.katiya"};
       	var response;    

        $httpBackend.when('POST', expectedUrl, userdata)
            .respond(500);

        manageAccessAPIService.getToken(userdata)
            .then(function(data) {
                response = data;
            }).catch(function() {
                response = 'Error!';
            });

        $httpBackend.flush();
        expect(response).toEqual('Error!');
    });	
});