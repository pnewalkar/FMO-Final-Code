
angular.module('layers')
    .factory('layersApiService', layersApiService);
layersApiService.$inject = ['$http', 'GlobalSettings'];

function layersApiService($http, GlobalSettings) {
    var layersApiService = {};

    layersApiService.fetchDeliveryPoints = function () {
        return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/fetchDeliveryPoint');
    };

    layersApiService.fetchAccessLinks = function () {
        return $http.get(GlobalSettings.apiUrl + '/accessLinks/fetchAccessLink');
    };
    return layersApiService;

}