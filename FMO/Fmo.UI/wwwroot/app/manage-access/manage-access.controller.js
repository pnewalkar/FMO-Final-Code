'use strict';

angular
    .module('manageAccess')
    .controller('manageAccessController', manageAccessController);

manageAccessController.$inject = ['$stateParams', 'manageAccessService', '$location'];

function manageAccessController($stateParams, manageAccessService, $location) {
    /* jshint validthis:true */
    var vm = this;
    var username = $location;
    activate();

    function activate() {
        debugger;
        var userName = getParameterValues('username');
        var unitGuid = getParameterValues('unitguid');

        if (userName) {
            vm.userdata = "username=" + userName + "&unitguid=" + unitGuid;
            manageAccessService.getToken(vm.userdata).then(function (response) {
                var accessData = response.data;
                sessionStorage.setItem("roleAccessData", accessData)
            });
        }
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
