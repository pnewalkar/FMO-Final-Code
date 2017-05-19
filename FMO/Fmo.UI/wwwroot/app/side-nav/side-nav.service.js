angular
    .module('sideNav')
    .service('sideNavService', sideNavService);

function sideNavService() {
    return {
        fetchActionItems: fetchActionItems
    };

    function fetchActionItems() {
        getItem = sessionStorage.getItem('roleAccessData');

        return {
            RolesActionResult: angular.fromJson(getItem)
        }
    }
}