angular.module('sideNav')
    .factory('sideNavApiService',
    ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var sideNavApiService = {};


        sideNavApiService.fetchActionItems = function () {
            return $http.get(GlobalSettings.apiUrl + '/AccessAction/fetchAccessLink');

        };

       
        return sideNavApiService;

    }]);