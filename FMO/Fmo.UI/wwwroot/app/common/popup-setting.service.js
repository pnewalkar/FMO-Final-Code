angular.module('FMOApp')
    .factory('popUpSettingService', PopUpSettingService);

function PopUpSettingService() {
    return {
        advanceSearch: advanceSearch,
        routeLog: routeLog,
        deliveryPoint: deliveryPoint
    };

    function advanceSearch(query) {
        return {
            templateUrl: './advance-search/advance-search.template.html',
            clickOutsideToClose: false,
            controller: 'AdvanceSearchController as vm',
            params: { searchText: query }
        };
    }

    function routeLog(selectedUnit) {
        return {
            templateUrl: './route-log/route-log.template.html',
            clickOutsideToClose: true,
            locals: {
                items: selectedUnit
            },
            controller: 'RouteLogController as vm'
        };
    }

    function deliveryPoint() {
        return {
            templateUrl: './delivery-point/delivery-point.template.html',
            clickOutsideToClose: false,
            controller: "DeliveryPointController as vm"
        };
    }

};

