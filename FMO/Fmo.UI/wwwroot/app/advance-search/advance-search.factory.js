
angular.module('advanceSearch')
    .factory('advanceSearchService',
    ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var advanceSearchApiService = {};
        debugger;
        //layersApiService.fetchDeliveryPoints = function () {
        //    return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/fetchDeliveryPoint');
        //};

        //layersApiService.fetchAccessLinks = function () {
        //    return $http.get(GlobalSettings.apiUrl + '/accessLinks/fetchAccessLink');
        //};
        return advanceSearchApiService;

    }]);