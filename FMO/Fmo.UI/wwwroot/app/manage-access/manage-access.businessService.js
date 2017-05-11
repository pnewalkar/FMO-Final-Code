
angular.module('manageAccess')
    .service('manageAccessBusinessService', ['$stateParams', '$state', 'manageAccessService', '$location', 'GlobalSettings', manageAccessBusinessService])
function manageAccessBusinessService($stateParams, $state, manageAccessService, $location, GlobalSettings) {
    var vm = this;

    return {
        activate: activate,
        getParameterValues: getParameterValues
    };

    function activate(unitGuid) {
        var aValue = sessionStorage.getItem('authorizationData');
        if (unitGuid) {

            var jobject = JSON.parse(aValue)
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
                    var jobject = JSON.parse(aValue)
                    if (jobject.userName !== userName) {
                        sessionStorage.clear();
                    }
                }
            }
        }

        manageAccessService.getToken(vm.userdata).then(function (response) {
            var accessData = response.data;
            if (response.access_token) {
                sessionStorage.clear();
                sessionStorage.setItem("authorizationData", JSON.stringify({ token: response.access_token, userName: response.username[0], unitGuid: unitGuid }));
                sessionStorage.setItem("roleAccessData", JSON.stringify((response.roleActions)));
                if (unitGuid) {
                    window.location.href = GlobalSettings.indexUrl;
                } else if (response.access_token || angular.isDefined(response.access_token)) {
                    window.location.href = GlobalSettings.indexUrl;
                }
            }

        });
    }

    function getParameterValues(param) {
        var url = window.location.search.slice(window.location.search.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }
}