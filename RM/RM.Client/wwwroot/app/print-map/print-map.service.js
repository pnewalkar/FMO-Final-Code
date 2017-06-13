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
        generatePdf: generatePdf
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
    };
    function printMap(printMapDTO) {
        var deferred = $q.defer();
        printMapAPIService.printMap(printMapDTO).then(function (response) {
            deferred.resolve(response);
        });
        return deferred.promise;
    };
}