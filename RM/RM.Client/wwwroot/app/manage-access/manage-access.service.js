﻿
angular.module('manageAccess')
    .service('manageAccessService', manageAccessService);

manageAccessService.$inject = [
    'manageAccessAPIService',
    'GlobalSettings',
    '$window'];

function manageAccessService(
    manageAccessAPIService,
    GlobalSettings,
    $window) {

    var vm = this;
    return {
        activate: activate,
        getParameterValues: getParameterValues
    };

    function activate(unitGuid) {
        var aValue = sessionStorage.getItem('authorizationData');
        if (unitGuid) {

            var jobject = angular.fromJson(aValue)
            vm.userdata = "username=" + jobject.userName + "&unitguid=" + unitGuid;
        }
        else {
            var userName = getParameterValues('username');
            vm.userdata = "username=" + userName + "&unitguid=" + unitGuid;
            if (angular.isUndefined(userName)) {
                if (aValue) {
                    return;
                }
                else {
                    alert("User Name not provided.")
                    return;
                }
            }
            else {
                if (aValue) {
                    var jobject = angular.fromJson(aValue)
                    if (jobject.userName !== userName) {
                        sessionStorage.clear();
                    }
                }
            }
        }

        manageAccessAPIService.getToken(vm.userdata).then(function (accessData) {
            if (accessData.AccessToken) {
                sessionStorage.clear();
                sessionStorage.setItem("authorizationData", angular.toJson({ token: accessData.AccessToken, userName: accessData.UserName, unitGuid: unitGuid }));
                sessionStorage.setItem("roleAccessData", angular.toJson((accessData.RoleActions)));
                if (unitGuid) {
                    $window.location.href = GlobalSettings.indexUrl;
                } else if (accessData.AccessToken || angular.isDefined(accessData.AccessToken)) {
                    $window.location.href = GlobalSettings.indexUrl;
                }
            }

        });
    }

    function getParameterValues(param) {
        var url = $window.location.search.slice($window.location.search.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }
}