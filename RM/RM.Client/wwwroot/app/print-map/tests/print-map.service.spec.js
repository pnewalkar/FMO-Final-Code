describe('Print Map: Service', function () {
    var $q,
        $mdDialog,
        printMapAPIService,
        CommonConstants,
        referencedataApiService,
        referenceDataConstants,
        mockReferenceDataConstants
        mapService
        var DBCategoryName='dj';

    var generatePdfMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];
    var mapPdfMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];
    var printMapMockData = [{ "name": null, "value": "Live", "displayText": null, "description": "Live" }, { "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }];

    var mockReferenceDataConstants = {
    PDF_PageSize: { AppCategoryName: "PDF_PageSize", DBCategoryName: "PDF_PageSize", ReferenceDataNames: [{ AppReferenceDataName: "PDF_PageSize_A0", DBReferenceDataName: "PDF_PageSize_A0" }, { AppReferenceDataName: "PDF_PageSize_A1", DBReferenceDataName: "PDF_PageSize_A1" }, { AppReferenceDataName: "PDF_PageSize_A2", DBReferenceDataName: "PDF_PageSize_A2" }, { AppReferenceDataName: "PDF_PageSize_A3", DBReferenceDataName: "PDF_PageSize_A3" }, { AppReferenceDataName: "PDF_PageSize_A4", DBReferenceDataName: "PDF_PageSize_A4" }] }
        };

    beforeEach(function () {
        module('printMap');

        module(function ($provide) {
            $provide.constant('referenceDataConstants', mockReferenceDataConstants);
            MockMdCancel = jasmine.createSpy();
                $provide.factory('$mdDialog', function() {
                    return {cancel: MockMdCancel};
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

                function mapPdf(printMapDTO) {
                    var deferred = $q.defer();
                    deferred.resolve(mapPdfMockData);
                    return deferred.promise;
                }

                function printMap(printMapDTO) {
                    var deferred = $q.defer();
                    deferred.resolve(printMapMockData);
                    return deferred.promise;
                }

                return {
                    generatePdf: generatePdf,
                    mapPdf: mapPdf,
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

        inject(function (_$rootScope_,_licensingInformationAccessorService_,_mapFactory_,_referenceDataConstants_,_mapService_,_printMapAPIService_,_CommonConstants_,_printMapService_,_$q_,_$mdDialog_,_referencedataApiService_) {
            scope = _$rootScope_.$new(); //Generate new scope
            $rootScope = _$rootScope_;
            printMapService = _printMapService_;
            printMapAPIService = _printMapAPIService_;
            CommonConstants = _CommonConstants_;
            $q = _$q_;
            $mdDialog = _$mdDialog_;
            referencedataApiService = _referencedataApiService_;
            referenceDataConstants = _referenceDataConstants_;
            mapService = _mapService_;
            mapFactory = _mapFactory_;
            licensingInformationAccessorService = _licensingInformationAccessorService_;
        });

    });

    it('should be window close when dialog button cancel ', function() {
        spyOn(printMapService,'closeWindow').and.callThrough(); 

        printMapService.closeWindow();

        expect(printMapService.closeWindow).toHaveBeenCalled();
        expect($mdDialog.cancel).toHaveBeenCalled();
        expect($mdDialog.cancel).toHaveBeenCalledTimes(1);

    });

    it('should initialize the function ', function() {
        spyOn(printMapService,'initialize').and.callThrough();
        printMapService.initialize();
        
        expect(printMapService.initialize).toHaveBeenCalled();

    });

    it('should be return promise response when call generatePdf', function (done) {
       var pdfFilename = undefined;
       printMapAPIService.generatePdf(pdfFilename).then(function (response) {
           expect(response).toBeDefined();
           expect(response).toBe(generatePdfMockData);
           done();
       });
       scope.$digest();
    });

    it('should be return promise response when call mapPdf', function (done) {
       var printMapDto = undefined;
       printMapAPIService.mapPdf(printMapDto).then(function (response) {
           expect(response).toBeDefined();
           expect(response).toBe(mapPdfMockData);
           done();
       });
       scope.$digest();
    });

    it('should be return promise response when call printMap', function (done) {
       var printMapDTO = undefined;
       printMapAPIService.printMap(printMapDTO).then(function (response) {
           expect(response).toBeDefined();
           expect(response).toBe(printMapMockData);
           done();
       });
       scope.$digest();
    });

    it('should be return promise response when call loadpdfsize', function (done) {
       var referenceDataConstants = undefined;
       referencedataApiService.getNameValueReferenceData(referenceDataConstants).then(function (response) {
           expect(response).toBeDefined();
           expect(response).toBe(printMapMockData);
           done();
       });
       scope.$digest();
    });

    it('should be return promise response when call getReferencedata', function (done) {
       var categoryName = undefined;
       referencedataApiService.getNameValueReferenceData(categoryName).then(function (response) {
           expect(response).toBeDefined();
           expect(response).toBe(printMapMockData);
           done();
       });
       scope.$digest();
    });

});