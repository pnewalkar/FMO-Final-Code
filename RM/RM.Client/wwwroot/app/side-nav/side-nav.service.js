angular
    .module('sideNav')
    .service('sideNavService', sideNavService);

sideNavService.$inject = [
                          'CommonConstants',
                          'roleAccessService'
                           ];
function sideNavService(
                         CommonConstants,
                         roleAccessService) {
    return {
        fetchActionItems: fetchActionItems
    };

    function fetchActionItems() {
        return RolesActionResult = roleAccessService.fetchActionItems();
    }
}