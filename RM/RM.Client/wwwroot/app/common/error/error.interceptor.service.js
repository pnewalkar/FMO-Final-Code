'use strict';
angular.module('RMApp')
.factory('errorInterceptorService', ['$rootScope', '$q', errorInterceptorService]);


function errorInterceptorService($rootScope, $q) {

    var errorInterceptorServiceFactory = {};



    var _responseError = function (rejection) {


        if (rejection.status != 401) {
            //update to proper default text from reference service
            var message = "There was an error processing your request";
            if (rejection.data && rejection.data.message) {
                // message = rejection.data.message;
                message = "There was an error processing your request";
            }
            $rootScope.$broadcast("showError", message);

        }
        return $q.reject(rejection);
    }

    errorInterceptorServiceFactory.responseError = _responseError;

    return errorInterceptorServiceFactory;
}