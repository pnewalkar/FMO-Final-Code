angular.module('referencedata')
    .factory('referencedataApiService',
    ['$http',
     'GlobalSettings',
     function ($http, GlobalSettings) {
         var referencedataApiService = {};

         referencedataApiService.getReferenceData = function () {
             return $http.get('./reference-data/ReferenceData.js');

        };

         return referencedataApiService;

    }]);