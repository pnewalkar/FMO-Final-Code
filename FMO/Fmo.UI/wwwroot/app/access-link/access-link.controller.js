angular
    .module('accessLink')
    .controller("AccessLinkController",
    [
        'accessLinkApiService',
        '$scope',
        '$mdDialog'
        , AccessLinkController])
function AccessLinkController(
    accessLinkApiService,
    $scope,
    $mdDialog 
) {
    vm.createAccessLink = createAccessLink;

    function createAccessLink() {
        debugger;
        var accessLink = null;

        accessLinkApiService.createAccessLink(accessLink).then(function (response) {
            debugger;
            alert.message(response);            
        });
    }
};