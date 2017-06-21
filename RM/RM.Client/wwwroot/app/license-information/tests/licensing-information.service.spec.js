describe('Licensing Info: Service', function () {
    var rootScope;
    var scope;
    var $q;
    var defrred;
    var mapService
    var referencedataApiService;
    var licensingInfoService;

    var LicensingInfoMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];
    var getLicensingInfoMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];

    beforeEach(function () {
        module('licensingInfo');

        //inject with mockdata printMapAPIService
        module(function ($provide) {
            $provide.service('licensingInfoService', function ($q) {

                function LicensingInfo(pdfFilename) {
                    var deferred = $q.defer();
                    deferred.resolve(LicensingInfoMockData);
                    return deferred.promise;
                }

                function getLicensingInfo(printMapDTO) {
                    var deferred = $q.defer();
                    deferred.resolve(getLicensingInfoMockData);
                    return deferred.promise;
                }

                return {
                    LicensingInfo: LicensingInfo,
                    getLicensingInfo: getLicensingInfo
                };
            });

            $provide.service('mapService', function ($q) {
                return '';
            });

            $provide.service('referencedataApiService', function ($q) {
                return '';
            });

        });

        //get Instance of controller with inject properties
        angular.mock.inject(function (_$rootScope_, _$q_, _mapService_, _licensingInfoService_,_referencedataApiService_) {
            scope = _$rootScope_.$new();
            rootScope = _$rootScope_;
            $q = _$q_;
            mapService = _mapService_;
            licensingInfoService = _licensingInfoService_;
            referencedataApiService = _referencedataApiService_;
        });

    });

    it('should be return promise response when call LicensingInfo', function (done) {
        licensingInfoService.LicensingInfo().then(function (response) {
            expect(response).toBeDefined();
            expect(response).toBe(LicensingInfoMockData);
            done();
        });
        scope.$digest();
    });

    it('should be return promise response when call getLicensingInfo', function (done) {
        licensingInfoService.getLicensingInfo().then(function (response) {
            expect(response).toBeDefined();
            expect(response).toBe(getLicensingInfoMockData);
            done();
        });
        scope.$digest();
    });

});