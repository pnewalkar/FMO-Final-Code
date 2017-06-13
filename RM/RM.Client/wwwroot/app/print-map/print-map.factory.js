angular.module('printMap')
    .factory('printMapAPIService', printMapAPIService);

printMapAPIService.$inject = ['$http',
                              '$q',
                              'GlobalSettings',
'stringFormatService'];

function printMapAPIService($http,
                            $q,
                            GlobalSettings,
                            stringFormatService) {

    return {
        printMap: printMap,
        generatePdf: generatePdf
    }

    function generatePdf(pdfFilename) {
        var deferred = $q.defer();

        $http.get(GlobalSettings.pdfGeneratorApiUrl + GlobalSettings.getPdfreport + pdfFilename).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
    function printMap(printMapDTO) {
        var deferred = $q.defer();

        $http.post(GlobalSettings.mapManagerApiUrl + GlobalSettings.generateReportWithMap, printMapDTO).success(function (response) {
            deferred.resolve(response);

        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };
}

