
// Just for reference
angular.module('fmoCommonHome')
    .factory('fmoService',
    ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
    var fmoService = {};
  
    //var getUsers = function (objEmployee) {
    //    return $http.post(GlobalSettings.apiUrl + '/Values', objEmployee);
    //};

    var getStatus = function (objEmployee) {
        return $http.get(GlobalSettings.apiUrl + '/Values', objEmployee);
    };

    var getScenario = function (objEmployee) {
        return $http.get(GlobalSettings.apiUrl + '/Values', objEmployee);
    };

    var getRoutes = function (objEmployee) {
        return $http.get(GlobalSettings.apiUrl + '/Values', objEmployee);
    };

   // fmoService.getUsers = getUsers;
    fmoService.GetStatus = getStatus;
    fmoService.GetScenario = getScenario;
    fmoService.GetRoutes = getRoutes;

    return fmoService;
  
}]);