
angular.module('manageAccess')
    .service('manageAccessBusinessService', ['$stateParams', '$state', 'manageAccessService', '$location', manageAccessBusinessService])
function manageAccessBusinessService($stateParams, $state, manageAccessService, $location) {
    var vm = this;
    return {
        activate: activate,
        getParameterValues: getParameterValues
    };

    function activate(unitGuid) {  
        if (unitGuid) {
            var aValue = sessionStorage.getItem('authorizationData');
            var jobject = JSON.parse(aValue)
            vm.userdata = "username=" + jobject.userName + "&unitguid=" + unitGuid;
        }
        else {
            var userName = getParameterValues('username');
            vm.userdata = "username=" + userName + "&unitguid=" + unitGuid;
            debugger
            if (userName === undefined) {
                return;
            }
        }
       
        manageAccessService.getToken(vm.userdata).then(function (response) {
            var accessData = response.data;
            //accessToken, roleAccessData
            if (response.access_token) {
                sessionStorage.clear();
                sessionStorage.setItem("authorizationData", JSON.stringify({ token: response.access_token, userName: response.username[0] }));
                sessionStorage.setItem("roleAccessData", JSON.stringify((response.roleActions)));
                if (response.access_token || response.access_token !== undefined) {
                    window.location.href = "http://localhost:34559/app/index.html";
                }
            }
            //$state.go('home', { redirect: true });
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