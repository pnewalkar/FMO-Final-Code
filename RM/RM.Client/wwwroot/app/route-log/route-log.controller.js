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
        loadDeliveryRoute(vm.selectedRouteScenario.id);
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
    function loadDeliveryRoute(deliveryScenarioID) {
        routeLogService.loadDeliveryRoute(deliveryScenarioID, vm.selectedRouteSelectionObj.value).then(function (response) {
            vm.multiSelectiondeliveryRoute = response[0].multiSelectiondeliveryRoute;
            vm.deliveryRoute = response[0].deliveryRoute;

            if (vm.multiSelectiondeliveryRoute.length === 0 && vm.deliveryRoute.length === 0) {
                vm.isShowMultiSelectionRoute = false;
                vm.isDeliveryRouteDisabled = true;


            }
            else if (vm.multiSelectiondeliveryRoute.length > 0 && vm.deliveryRoute.length === 0) {
                vm.isDeliveryRouteDisabled = true;
                vm.isShowMultiSelectionRoute = true;
                vm.deliveryRoute = null;
            }
            else if (vm.deliveryRoute.length === 0) {
                vm.isDeliveryRouteDisabled = true;
            }

        });
    }
    function clearSearchTerm() {
        vm.searchTerm = '';
    }
    function deliveryRouteChange(selectedRouteValue) {
        routeLogService.deliveryRouteChange(selectedRouteValue.id).then(function (response) {
            var selectedUnit = sessionStorage.getItem('selectedDeliveryUnit');
            var selectUnitData = JSON.parse(selectedUnit);
            vm.routeDetails = response;
            if (selectUnitData) {
                vm.routeDetails.deliveryOffice = selectUnitData.unitName;
            }
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
    function displayRouteLogPdfReport(pdfFileName) {
        if (pdfFileName) {
            routeLogService.generatePdf(pdfFileName).then(function (response) {
                if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                    var byteCharacters = atob(response.data);
                    var byteNumbers = new Array(byteCharacters.length);
                    for (var i = 0; i < byteCharacters.length; i++) {
                        byteNumbers[i] = byteCharacters.charCodeAt(i);
                    }
                    var byteArray = new Uint8Array(byteNumbers);
                    var blob = new Blob([byteArray], {
                        type: 'application/pdf'
                    });
                    window.navigator.msSaveOrOpenBlob(blob, response.fileName);
                } else {
                    var base64EncodedPDF = response.data;
                    var dataURI = "data:application/pdf;base64," + base64EncodedPDF;
                    window.open(dataURI, "_blank");
                }
            });
        }
    }
}
