angular.module('referencedata')
    .factory('referencedataApiService', referencedataApiService);
referencedataApiService.$inject = ['$http','$q', 'GlobalSettings'];

function referencedataApiService($http,$q, GlobalSettings) {

    return {
        getReferenceData: getReferenceData
  
    };

    function getReferenceData() {
        return $http.get('./reference-data/ReferenceData.js');
    }

    }