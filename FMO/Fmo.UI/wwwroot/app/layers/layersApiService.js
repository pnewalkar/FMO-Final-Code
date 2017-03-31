
//angular.module('layers')
//    .factory('layersApiService',
//    ['$http', 'GlobalSettings', function ($http, GlobalSettings) {

//        // var urlBase = "http://localhost:34583/api";
//        var layersApiService = {};
//        //var deliveryPoints;

//        layersApiService.fetchDeliveryPoints = function () {
//            //return $http.get('http://localhost:34583/api/deliveryPoints/fetchDeliveryPoint');
//            //var getStatus = function (objEmployee) {
//            //   return $http.get(GlobalSettings.apiUrl + '/Values', objEmployee);
//            //};
//            return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/fetchDeliveryPoint');
//            //.success(function (deliveryPoints, status, headers, config) {
//            //    return deliveryPoints;
//            //})
//            //.error(function (deliveryPoints, status, header, config) {
//            //    return data;
//            //});
//        }
//        // layersApiService.fetchDeliveryPoints = fetchDeliveryPoints;
//        return layersApiService;
//    }]);


// Just for reference



angular.module('layers')
    .factory('layersApiService',
    ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
        var layersApiService = {};

        layersApiService.fetchDeliveryPoints = function () {
            return $http.get(GlobalSettings.apiUrl + '/deliveryPoints/fetchDeliveryPoint');
        };

        layersApiService.fetchAccessLinks = function () {
            return $http.get(GlobalSettings.apiUrl + '/accessLinks/fetchAccessLink');
        };
        return layersApiService;

    }]);