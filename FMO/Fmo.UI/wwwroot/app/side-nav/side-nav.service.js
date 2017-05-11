angular.module('sideNav')
    .factory('sideNavApiService', sideNavApiService);
sideNavApiService.$inject = ['$http', 'GlobalSettings'];
function sideNavApiService($http, GlobalSettings) {
    var sideNavApiService = {};

    return sideNavApiService;

}