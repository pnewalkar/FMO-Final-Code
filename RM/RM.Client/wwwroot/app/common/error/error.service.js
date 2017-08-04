angular.module('error')
    .factory('errorService', ['$mdDialog', '$rootScope', '$translate', '$timeout', errorService])

function errorService($mdDialog, $rootScope, $translate, $timeout) {
    return {
        openAlert: openAlert,
        openInfoPopup: openInfoPopup
    };
    function openAlert(text) {
        var alert = $mdDialog.alert()
            .parent()
            .clickOutsideToClose(false)
            .title($translate.instant('ERROR_DIALOG.TITLE'))
            .textContent(text)
            .ariaLabel('Error Dialog')
            .css('error-dialog')
            .ok($translate.instant('GENERAL.BUTTONS.OK'))

        $mdDialog.show(alert)
            .then(function() {
                $rootScope.$broadcast("errorClosed");
            });
    };

    function openInfoPopup(text,title,css) {
        var alert = $mdDialog.alert()
            .parent()
            .clickOutsideToClose(false)
            .title($translate.instant(title))
            .textContent(text)
            .ariaLabel('Duplicates Found')
            .css(css)
            .ok($translate.instant('GENERAL.BUTTONS.OK'))

        $mdDialog.show(alert)
            .then(function () {
                $rootScope.$broadcast("errorClosed");
            });
    };

  
    return errorService;
};


