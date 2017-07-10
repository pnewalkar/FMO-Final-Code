'use strict';
angular.module('RMApp')
.factory('errorInterceptorService', ['$rootScope', '$q', '$translate', errorInterceptorService]);


function errorInterceptorService($rootScope, $q, $translate) {
    return {
        responseError: error,
        requestError: error
    }

    function error(rejection) {
        var message = $translate.instant('GENERAL.ERRORS.UNKNOWN');
        
        if (rejection.data && rejection.data.message) {
            message = rejection.data.message;
        };

        $rootScope.$broadcast("showError", message);

        return $q.reject(rejection);
    };
};