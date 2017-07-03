angular.module('RMApp')
    .factory('popUpSettingService', PopUpSettingService);

function PopUpSettingService() {
    return {
        advanceSearch: advanceSearch,
        routeLog: routeLog,
        deliveryPoint: deliveryPoint,
        openAlert: openAlert,
        printMap: printMap
    };

    function advanceSearch(query) {
        return {
            templateUrl: './advance-search/advance-search.template.html',
            clickOutsideToClose: false,
            controller: 'AdvanceSearchController as vm',
            locals : {
                searchText : query
            }
        };
    }

    function routeLog(selectedUnit) {
        return {
            templateUrl: './route-log/route-log.template.html',
            clickOutsideToClose: false,
            locals: {
                items: selectedUnit
            },
            controller: 'RouteLogController as vm'
        };
    }

    function deliveryPoint(vm) {
        return {
            controller: function() {
                return vm;
            },
            templateUrl: './delivery-point/delivery-point.template.html',
            clickOutsideToClose: false,
            controllerAs: "vm",
            preserveScope: true
        };
    }

    function printMap() {
        return {
            templateUrl: './print-map/print-map.template.html',
            clickOutsideToClose: false,
            controller: 'PrintMapController as vm'
        };
    }

    function openAlert(text) {
        return {
            templateUrl: './common/error/error-popup.html',
            clickOutsideToClose: false,
            controller: 'ErrorController as vm',
            locals: {
                message: text
            }
        };
    }
};

