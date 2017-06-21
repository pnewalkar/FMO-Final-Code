﻿angular
    .module('printMap')
    .factory('printMapService', printMapService);
printMapService.$inject =
    [
        '$q',
        '$mdDialog',
        'printMapAPIService',
        'CommonConstants',
        'referencedataApiService',
        'referenceDataConstants',
        'mapService',
        '$rootScope',
        'mapFactory',
        '$timeout'
    ];

function printMapService(
        $q,
        $mdDialog,
        printMapAPIService,
        CommonConstants,
        referencedataApiService,
        referenceDataConstants,
        mapService,
        $rootScope,
        mapFactory,
        $timeout) {
    return {
        initialize: initialize,
        closeWindow: closeWindow,
        generateMapPDF: generateMapPDF
    };

    function initialize() {
        var deferred = $q.defer();

        var result = { "MapDpi": null, "ImageHeight": null, "ImageWidth": null, "pdfSize": null };
        $q.all([
            getMapDpi(),
            getImageHeight(),
            getImageWidth(),
            loadPdfSize()
        ]).then(function (response) {
            result.MapDpi = response[0];
            result.ImageHeight = response[1];
            result.ImageWidth = response[2];
            result.pdfSize = response[3];
            deferred.resolve(result)
        })
        return deferred.promise;
    }

    function closeWindow() {
        $mdDialog.cancel();
    }

    function generatePdf(pdfFilename) {
        var deferred = $q.defer();
        printMapAPIService.generatePdf(pdfFilename).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    function mapPdf(printMapDto) {
        var deferred = $q.defer();
        printMapAPIService.mapPdf(printMapDto).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    function printMap(printMapDTO) {
        var deferred = $q.defer();
        printMapAPIService.printMap(printMapDTO).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    function loadPdfSize() {
        var deferred = $q.defer();
        referencedataApiService.getNameValueReferenceData(referenceDataConstants.PDF_PageSize.DBCategoryName).then(function (response) {
            var pdfSizeResult = [];
            angular.forEach(response, function (value, key) {
                pdfSizeResult.push({ "DisplayText": value.description, "Value": value.value });
                deferred.resolve(pdfSizeResult);

            });
        });
        return deferred.promise;
    }

    function getReferencedata(categoryName) {
        var deferred = $q.defer();
        referencedataApiService.getNameValueReferenceData(categoryName).then(function (response) {
            var refdataResult = [];
            angular.forEach(response, function (value, key) {
                refdataResult.push({ "Name": value.name, "Value": value.value });
                deferred.resolve(refdataResult);

            });
        });
        return deferred.promise;
    }

    function getMapDpi() {
        var deferred = $q.defer();
        getReferencedata('PrintMap_DPI').then(function (response) {
            if (response) {
                deferred.resolve(response[0].Value);
            }
        });
        return deferred.promise;
    }

    function getImageHeight() {
        var deferred = $q.defer();
        getReferencedata('PrintMap_ImageHeightmm').then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    function getImageWidth() {
        var deferred = $q.defer();
        getReferencedata('PrintMap_ImageWidthmm').then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    }

    function getMapImageWidth(printSize, imageWidth) {
        var categoryName = 'PrintMap_ImageWidthmm_' + printSize;
        var result = imageWidth.filter(function (e) {
            return (e.Name === categoryName);
        });
        return result[0].Value;
    }

    function getMapImageHeight(printSize, imageHeight) {
        var categoryName = 'PrintMap_ImageHeightmm_' + printSize;
        var result = imageHeight.filter(function (e) {
            return (e.Name === categoryName);
        });
        return result[0].Value;
    }

    function getMapWidth(imageWidthmm, printMapDPI) {
        return Math.round((imageWidthmm * parseInt(printMapDPI)) / parseInt(CommonConstants.PrintMapmmPerInch));
    }

    function getMapHeight(imageHeightmm, printMapDPI) {
        return Math.round((imageHeightmm * parseInt(printMapDPI)) / parseInt(CommonConstants.PrintMapmmPerInch));
    }

    function generateMapPDF(printMapDPI, size, imageWidth, imageHeight, resolution, printOptions) {
        var imageWidthmm = getMapImageWidth(size, imageWidth);
        var imageHeightmm = getMapImageHeight(size, imageHeight);
        var mapWidth = getMapWidth(imageWidthmm, printMapDPI);
        var mapHeight = getMapHeight(imageHeightmm, printMapDPI);
        mapService.setSize(mapWidth, mapHeight);
        //After setting the map size it takes around 6-7 secs to load all the map tiles.Image should be captured after all the map tiles are loaded 
        $timeout(captureImage, 7000, true, printOptions, resolution);
    }

    function captureImage(printOptions, resolution) {
        mapService.composeMap();

        var printMapDto = {
            "MapTitle": printOptions.title,
            "CurrentScale": '1 : ' + Math.round(mapFactory.getScaleFromResolution(resolution)),
            "PdfOrientation": printOptions.orientation,
            "PdfSize": printOptions.size,
            "MapScale": 25,
            "EncodedString": $rootScope.canvas.toDataURL('image/png'),
            "License": "Test License"
        };

        mapService.setOriginalSize();
        mapService.refreshLayers();
        printMap(printMapDto).then(function (response) {
            mapPdf(response).then(function (response) {
                displayPdf(response);
            });
        });
    }

    function displayPdf(pdfFileName) {
        if (pdfFileName) {
            generatePdf(pdfFileName).then(function (response) {
                if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                    var byteCharacters = atob(response.data);
                    var byteNumbers = new Array(byteCharacters.length);
                    for (var i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    var byteArray = new Uint8Array(byteNumbers);
                    var blob = new Blob([byteArray], {
                        type: 'application/pdf'
                    });
                    window.navigator.msSaveOrOpenBlob(blob, response.fileName);
                } else {
                    var base64EncodedPDF = response.data;
                    var dataURI = "data:application/pdf;base64," + base64EncodedPDF;
                    window.open(dataURI, "_blank");
                }
            });
        }
    }
}