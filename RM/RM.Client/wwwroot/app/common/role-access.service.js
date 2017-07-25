angular
    .module('RMApp')
    .service('roleAccessService', roleAccessService);

roleAccessService.$inject = [
'CommonConstants'
];


function roleAccessService(CommonConstants) {
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