angular
    .module('routeLog')
    .controller('RouteLogController',
                 RouteLogController);
RouteLogController.$inject = ['routeLogService',
                            'items', '$scope'];

function RouteLogController(routeLogService,
                            items, $scope) {
    var vm = this;
    vm.initialize = initialize;
    vm.loadSelectionType = loadSelectionType;
    vm.loadRouteLogStatus = loadRouteLogStatus;
    vm.loadScenario = loadScenario;
    vm.scenarioChange = scenarioChange;
    vm.selectedRouteStatus = selectedRouteStatus;
    vm.selectionTypeChange = selectionTypeChange;
    vm.deliveryRouteChange = deliveryRouteChange;
    vm.clearSearchTerm = clearSearchTerm;
    vm.selectedDeliveryUnitObj = items;
    vm.closeWindow = closeWindow;
    vm.selectClass = "routeSearch md-text";
    vm.generateRouteLogSummary = generateRouteLogSummary;
    vm.emptyID = "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx";
    vm.generateSummaryReport = false;
    vm.initialize();
    function initialize() {
        vm.loadSelectionType();
        vm.loadRouteLogStatus();
    }

    function closeWindow() {
        routeLogService.closeWindow();
    }
    function selectionTypeChange() {
        vm.selectedRouteStatusObj = null;
        vm.selectedRouteScenario = null;
        vm.selectedRoute = null;
        vm.isSelectionType = false;
        vm.isRouteScenarioDisabled = true;
        vm.isDeliveryRouteDisabled = true;
        vm.isShowMultiSelectionRoute = false;
        clearDeliveryRoute();
    }
    function loadSelectionType() {
        routeLogService.loadSelectionType().then(function (response) {
            vm.RouteselectionTypeObj = response[0].RouteselectionTypeObj;
            vm.selectedRouteSelectionObj = response[0].selectedRouteSelectionObj;
        })
    }
    function selectedRouteStatus() {
        loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.ID);
        vm.isRouteScenarioDisabled = false;
        clearDeliveryRoute();
    }
    function loadRouteLogStatus() {
        routeLogService.loadRouteLogStatus().then(function (response) {
            vm.RouteStatusObj = response;
            vm.selectedRouteStatusObj = vm.RouteStatusObj[0];
            loadScenario(vm.selectedRouteStatusObj.id, vm.selectedDeliveryUnitObj.ID);
        });
    }
    function scenarioChange() {
        var result = routeLogService.scenarioChange(vm.selectedRouteSelectionObj.value);
        vm.isDeliveryRouteDisabled = result.isDeliveryRouteDisabled;
        vm.isShowMultiSelectionRoute = result.isShowMultiSelectionRoute;
        loadDeliveryRoute(vm.selectedRouteStatusObj.id, vm.selectedRouteScenario.id);
        clearDeliveryRoute();
    }
    function loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj) {
        routeLogService.loadScenario(selectedRouteStatusObj, selectedDeliveryUnitObj).then(function (response) {
            if (response.length > 0) {
                vm.RouteScenario = response;
            } else {
                vm.RouteScenario = undefined;
                vm.selectedRouteScenario = null;
                vm.isSelectionType = true;
                vm.selectedRoute = null;
                vm.isDeliveryRouteDisabled = true;
                vm.isShowMultiSelectionRoute = false;
            }
        });
    }
    function loadDeliveryRoute(operationStateID, deliveryScenarioID) {
        routeLogService.loadDeliveryRoute(operationStateID, deliveryScenarioID, vm.selectedRouteSelectionObj.value).then(function (response) {
            vm.multiSelectiondeliveryRoute = response[0].multiSelectiondeliveryRoute;
            vm.deliveryRoute = response[0].deliveryRoute;
        });
    }
    function clearSearchTerm() {
        vm.searchTerm = '';
    }
    function deliveryRouteChange(selectedRouteValue) {
        routeLogService.deliveryRouteChange(selectedRouteValue.id).then(function (response) {
            vm.routeDetails = response;
            vm.generateSummaryReport = false;
        });
    }
    function clearDeliveryRoute() {
        vm.deliveryRoute = null;
        vm.routeDetails = false;
        vm.generateSummaryReport = false;
    }
    function generateRouteLogSummary() {
        if (vm.generateSummaryReport) {
            var deliveryRouteDTO = { "DeliveryRouteDTO": vm.routeDetails };
            routeLogService.generateRouteLogSummary(vm.routeDetails).then(function (response) {
                displayRouteLogPdfReport(response);
            });
        }
    }
    function displayRouteLogPdfReport(data) {
        if (data) {
            if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                var fileName = generateGuid() + ".pdf";
                var byteCharacters = atob(data);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                var byteArray = new Uint8Array(byteNumbers);
                var blob = new Blob([byteArray], {
                    type: 'application/pdf'
                });
                window.navigator.msSaveOrOpenBlob(blob, fileName);
            } else {
                var base64EncodedPDF = data;
                var dataURI = "data:application/pdf;base64," + base64EncodedPDF;
                window.open(dataURI, "_blank");
            }
        }
    }
    function generateGuid() {
        return vm.emptyID.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}
