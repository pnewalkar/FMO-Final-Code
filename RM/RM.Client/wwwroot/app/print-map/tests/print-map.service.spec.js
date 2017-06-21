describe('Print Map: Service', function () {
    //var rootScope;
    //var scope;
    //var $q;
    //var $mdDialog
    //var printMapService;

    var mockPrintMapService, mockMdCancel;
    //var mockPrintMapAPIService;

    //var referencedataApiService;
    //var referenceDataConstants;
    //var CommonConstants;

    var generatePdfMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];
    var printMapMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];

    beforeEach(function () {
        module('printMap');

        module(function ($provide) {
            mockMdCancel = jasmine.createSpy();
            $provide.factory('$mdDialog', function () {
                return function cancel() {
                    return { cancel: mockMdCancel };
                };
            });
        });
      
        //inject with mockdata printMapAPIService
        module(function ($provide) {
            $provide.service('printMapAPIService', function ($q) {

                function generatePdf(pdfFilename) {
                    var deferred = $q.defer();
                    deferred.resolve(generatePdfMockData);
                    return deferred.promise;
                }

                function printMap(printMapDTO) {
                    var deferred = $q.defer();
                    deferred.resolve(printMapMockData);
                    return deferred.promise;
                }

                return {
                    generatePdf: generatePdf,
                    printMap: printMap
                };
            });

            $provide.service('CommonConstants', function () {
                return '';
            });

            $provide.service('referencedataApiService', function ($q) {

                function getNameValueReferenceData(referenceDataConstants) {
                    var deferred = $q.defer();
                    deferred.resolve(printMapMockData);
                    return deferred.promise;
                }

                function getReferencedata(categoryName) {
                    var deferred = $q.defer();
                    deferred.resolve(printMapMockData);
                    return deferred.promise;
                }

                return {
                    getNameValueReferenceData: getNameValueReferenceData,
                    getReferencedata: getReferencedata
                };
            });

            $provide.service('referenceDataConstants', function ($q) {
                return '';
            });

            $provide.service('mapService', function ($q) {
                return '';
            });

            $provide.service('mapFactory', function ($q) {
                return '';
            });

            $provide.service('licensingInformationAccessorService', function ($q) {
                return '';
            });
        });

    });

    beforeEach(function () {
        inject(function ($injector) {
            mockPrintMapService = $injector.get('printMapService');
        });
    });


   /* beforeEach(inject(function (
        $service,
        $q,
        $mdDialog,
        printMapAPIService,
        CommonConstants,
        referencedataApiService,
        referenceDataConstants,
        mapService,
        $rootScope,
        mapFactory,
        $timeout,
        licensingInformationAccessorService) {

        mockmdDialog = $mdDialog;
        mockPrintMapAPIService = printMapAPIService;
        mockCommonConstants = CommonConstants;
        mockReferencedataApiService = referencedataApiService;
        mockReferenceDataConstants = referenceDataConstants;
        mockMapService = mapService;
        mockMapFactory = mapFactory;
        mockLicensingInformationAccessorService = licensingInformationAccessorService;

        mockPrintMapService = $service('printMapService', {
            $q: $q,
            $mdDialog: mockmdDialog,
            printMapAPIService: mockPrintMapAPIService,
            CommonConstants: mockCommonConstants,
            referencedataApiService: mockReferencedataApiService,
            referenceDataConstants: mockReferenceDataConstants,
            mapService: mockMapService,
            $rootScope: $rootScope,
            mapFactory: mockMapFactory,
            $timeout: $timeout,
            licensingInformationAccessorService: mockLicensingInformationAccessorService
        });
    }));
    */

    describe('Should close the dialog-window', function () {
        it('Should close window', function () {
            mockPrintMapService.closeWindow();
            expect(mockMdCancel).toHaveBeenCalled();
            expect(mockMdCancel).toHaveBeenCalledTimes(1);
        });
    });

    //describe('Should initalize the service', function () {
    //    it('Should initailize', function () {
    //        spyOn(printMapService, 'initialize').and.callThrough();
    //        mockPrintMapService.initialize();
    //        expect(printMapService.initialize).not.toBeUndefined();
    //        expect(printMapService.initialize).toHaveBeenCalled();
    //        expect(printMapService.initialize).toHaveBeenCalledTimes(1);
    //    });
    //});

    //it('should be return promise response when call generatePdf', function (done) {
    //    var pdfFilename = undefined;
    //    printMapAPIService.generatePdf(pdfFilename).then(function (response) {
    //        expect(response).toBeDefined();
    //        expect(response).toBe(generatePdfMockData);
    //        done();
    //    });
    //    scope.$digest();
    //});

    //it('should be return promise response when call printMap', function (done) {
    //    var printMapDTO = undefined;
    //    printMapAPIService.printMap(printMapDTO).then(function (response) {
    //        expect(response).toBeDefined();
    //        expect(response).toBe(printMapMockData);
    //        done();
    //    });
    //    scope.$digest();
    //});

    //it('should be return promise response when call loadpdfsize', function (done) {
    //    var referenceDataConstants = undefined;
    //    referencedataApiService.getNameValueReferenceData(referenceDataConstants).then(function (response) {
    //        expect(response).toBeDefined();
    //        expect(response).toBe(printMapMockData);
    //        done();
    //    });
    //    scope.$digest();
    //});

    //it('should be return promise response when call getReferencedata', function (done) {
    //    var categoryName = undefined;
    //    referencedataApiService.getNameValueReferenceData(categoryName).then(function (response) {
    //        expect(response).toBeDefined();
    //        expect(response).toBe(printMapMockData);
    //        done();
    //    });
    //    scope.$digest();
    //});

});