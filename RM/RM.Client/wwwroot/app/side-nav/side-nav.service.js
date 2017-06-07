angular
    .module('sideNav')
    .service('sideNavService', sideNavService);

sideNavService.$inject = [
'CommonConstants'
                           ];
function sideNavService(CommonConstants) {
    return {
        fetchActionItems: fetchActionItems
    };

    function fetchActionItems() {
        getItem = sessionStorage.getItem(CommonConstants.GetSessionStorageItemType);

        return {
            RolesActionResult: angular.fromJson(getItem)
        }
    }
}