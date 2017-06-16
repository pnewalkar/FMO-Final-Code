'use strict';
angular.module('RMApp')
.factory('errorInterceptorService', ['$rootScope', '$q', errorInterceptorService]);


function errorInterceptorService($rootScope, $q) {

    var errorInterceptorServiceFactory = {};



    var _responseError = function (rejection) {


        if (rejection.status === 401 || rejection.status === 404 || rejection.status === 500) {

            var message = "There was an error processing your request";
            $rootScope.$broadcast("showError", message);

        }
        else {
            if (rejection.data && rejection.data.message) {
                message = rejection.data.message;
                $rootScope.$broadcast("showError", message);
            }
         
        }
        return $q.reject(rejection);
    }

    errorInterceptorServiceFactory.responseError = _responseError;

    return errorInterceptorServiceFactory;
}