angular.module('referencedata')
    .factory('referencedataApiService', referencedataApiService);
referencedataApiService.$inject = ['$http', 'GlobalSettings'];

function referencedataApiService($http, GlobalSettings) {
         var referencedataApiService = {};

         referencedataApiService.getReferenceData = function () {
             return $http.get('./reference-data/ReferenceData.js');

         };

         function readJson() {
             var deferred = $q.defer();

             $http.get('./UI-string.json').success(function (response) {
                 deferred.resolve(response);

             }).error(function (err, status) {
                 alert(err);
                 deferred.reject(err);
             });

             return deferred.promise;
         }

         return referencedataApiService;

    }