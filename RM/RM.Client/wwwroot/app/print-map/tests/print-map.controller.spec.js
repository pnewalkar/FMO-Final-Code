describe('Print Map: Controller', function () {
    var PrintMapController;
    var mapService;
    var printMapService;

    var mapimageInfoMock = { "MapDpi": 70.5, "ImageHeight": [{}], "ImageWidth": [{}], "pdfSize": [{}] };
  
    beforeEach(function () {
        module('printMap');

        //inject with mockdata routeLogService
        angular.mock.module(function ($provide) {
            $provide.service('mapService', function () {
                function getResolution() {
                    return;
                }
                return {
                    getResolution: getResolution
                };
            });
        });

        //inject with mockdata printMapService
        angular.mock.module(function ($provide) {
            $provide.service('printMapService', function ($q) {

                function initialize() {
                    deferred = $q.defer();
                    deferred.resolve(mapimageInfoMock);
                    return deferred.promise;
                }

                function closeWindow() {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                function generateMapPDF(/*param*/) {
                    deferred = $q.defer();
                    return deferred.promise;
                }

                return {
                    initialize: initialize,
                    closeWindow: closeWindow,
                    generateMapPDF: generateMapPDF
                };
            });
        });

    });

    beforeEach(inject(function ($controller, _mapService_, _printMapService_) {
        mapService = _mapService_;
        printMapService = _printMapService_;
        PrintMapController = $controller('PrintMapController', {
            printMapService: _printMapService_,
            mapService: mapService
        });
    }));

    describe('Should close the dialog-window', function () {
        it('Should close window', function () {
            spyOn(printMapService, 'closeWindow').and.callThrough();
            PrintMapController.closeWindow();
            expect(printMapService.closeWindow).not.toBeUndefined();
            expect(printMapService.closeWindow).toHaveBeenCalled();
            expect(printMapService.closeWindow).toHaveBeenCalledTimes(1);
        });
    });

    describe('Should initialize the controller', function () {
        it('should be initialize the function', function () {
            spyOn(printMapService, 'initialize').and.callThrough();
            PrintMapController.initialize();
            expect(printMapService.initialize).not.toBeUndefined();
            expect(printMapService.initialize).toHaveBeenCalled();
            expect(printMapService.initialize).toHaveBeenCalledTimes(1);
        });
    });

    describe('Should generate print map to pdf', function () {
        it('should be call Print map function', function () {
            spyOn(printMapService, 'generateMapPDF').and.callThrough();
            PrintMapController.printMap();
            expect(printMapService.generateMapPDF).not.toBeUndefined();
            expect(printMapService.generateMapPDF).toHaveBeenCalled();
            expect(printMapService.generateMapPDF).toHaveBeenCalledTimes(1);
        });
    });
});