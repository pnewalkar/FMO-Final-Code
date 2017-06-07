'use strict';
angular.module('RMApp')
.factory('authInterceptorService', authInterceptorService);
authInterceptorService.$inject = ['$q', '$injector'];

function authInterceptorService($q, $injector) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

        config.headers = config.headers || {};

        var authData = sessionStorage.getItem('authorizationData');
        authData = angular.fromJson(authData);
        if (authData) {
            config.headers.Authorization = 'Bearer ' + authData.token;
        }

        return config;
    }

    var _responseError = function (rejection) {

        if (rejection.status === 401) {
            var authService = $injector.get('authService');
            var authData = sessionStorage.getItem('authorizationData');

            if (authData) {
                if (authData.useRefreshTokens) {
                    return $q.reject(rejection);
                }
            }
        }
        return $q.reject(rejection);
    }

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
}