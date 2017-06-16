angular.module('printMap')
    .factory('printMapService', printMapService);
printMapService.$inject = ['$q',
                           '$mdDialog',
                           'printMapAPIService',
                           'CommonConstants',
                            'referencedataApiService',
                            'referenceDataConstants'];

function printMapService(
$q,
$mdDialog,
printMapAPIService,
CommonConstants,
referencedataApiService,
referenceDataConstants) {
    return {
        closeWindow: closeWindow,
        printMap:printMap,
        generatePdf: generatePdf,
        loadPdfSize: loadPdfSize,
        getReferencedata: getReferencedata,
        mapPdf: mapPdf
    };
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
}