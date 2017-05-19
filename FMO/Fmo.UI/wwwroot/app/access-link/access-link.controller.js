angular
    .module('accessLink')
    .controller("AccessLinkController",
    [
        'accessLinkAPIService',
        '$scope',
        '$mdDialog'
        , AccessLinkController])
function AccessLinkController(
    accessLinkAPIService,
    $scope,
    $mdDialog 
) {
    vm.createAccessLink = createAccessLink;

    function createAccessLink() {
        debugger;
        var accessLink = null;

        accessLinkAPIService.createAccessLink(accessLink).then(function (response) {
            debugger;
            alert.message(response);            
        });
    }
};