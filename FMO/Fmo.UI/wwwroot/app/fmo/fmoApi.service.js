
// Just for reference
angular.module('fmoCommonHome')
    .factory('fmoService', ['$http', 'GlobalSettings', function ($http, GlobalSettings) {
    var fmoService = {};
  
    var getUsers = function (objEmployee) {
        return $http.post(GlobalSettings.apiUrl + '/Values', objEmployee);
    };

    fmoService.getUsers = getUsers;

    return fmoService;
  
}]);