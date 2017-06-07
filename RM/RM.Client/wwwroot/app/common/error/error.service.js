angular.module('error')
    .factory('errorService', errorService)
errorService.$inject = [
'$mdDialog', 'popUpSettingService','$rootScope'];

function errorService($mdDialog, popUpSettingService, $rootScope) {
    var errorService = {};
    return {
        openAlert: openAlert
        };

   
function openAlert(text) {
    $mdDialog.show(popUpSettingService.openAlert(text)).then(function () {
        $rootScope.$broadcast("errorClosed");
    });
    }
  
    return errorService;
};

