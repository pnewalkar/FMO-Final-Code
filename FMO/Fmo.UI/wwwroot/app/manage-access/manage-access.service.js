
angular.module('manageAccess')
    .factory('manageAccessService', manageAccessService);

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

        manageAccessAPIService.getToken(vm.userdata).then(function (response) {
            var accessData = response.data;
            if (response.access_token) {
                sessionStorage.clear();
                sessionStorage.setItem("authorizationData", angular.toJson({ token: response.access_token, userName: response.username[0], unitGuid: unitGuid }));
                sessionStorage.setItem("roleAccessData", angular.toJson((response.roleActions)));
                if (unitGuid) {
                    $window.location.href = GlobalSettings.indexUrl;
                } else if (response.access_token || angular.isDefined(response.access_token)) {
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