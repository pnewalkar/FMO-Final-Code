angular.module('referencedata')
    .factory('referencedataApiService', referencedataApiService);
referencedataApiService.$inject = ['$http', 'GlobalSettings'];

function referencedataApiService($http, GlobalSettings) {
         var referencedataApiService = {};

         referencedataApiService.getReferenceData = function () {
             return $http.get('./reference-data/ReferenceData.js');

        };

         return referencedataApiService;

    }