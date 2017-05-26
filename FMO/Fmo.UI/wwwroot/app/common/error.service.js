angular.module('error')
    .factory('errorService', errorService)
errorService.$inject = [
'$mdDialog', 'popUpSettingService'];

function errorService($mdDialog, popUpSettingService) {
    var errorService = {};
    return {
        openAlert: openAlert
        };

   
function openAlert(text) {
    $mdDialog.show(popUpSettingService.openAlert(text));       
    }
  
    return errorService;
};
