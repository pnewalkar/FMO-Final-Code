//(function (window) {
//    window.__env = window.__env || {};
//    window.__env.apiUrl = 'http://localhost:34583/api';
//}(this));

var GlobalSettings = {
    apiUrl: '',
    env: 'localhost', // Here set the current environment
    indexUrl: ''
};

/* For Api */

//if (window.location.hostname == "localhost") {
//    GlobalSettings.apiUrl = "http://localhost:34583/api";
//}
//else {
//    GlobalSettings.apiUrl = "";
//}

if (GlobalSettings.env === "localhost") {
    GlobalSettings.actionManagerApiUrl = "http://localhost:43423/api";
    GlobalSettings.referenceDataApiUrl = "http://localhost:26199/api";
    GlobalSettings.accessLinkApiUrl = "http://localhost:50237/api";
    GlobalSettings.deliveryPointApiUrl = "http://localhost:50236/api";
    GlobalSettings.deliveryRouteApiUrl = "http://localhost:50235/api";
    GlobalSettings.networkManagerApiUrl = "http://localhost:50242/api";
    GlobalSettings.postalAddressApiUrl = "http://localhost:43585/api";
    GlobalSettings.specialInstructionApiUrl = "http://localhost:50238/api";
    GlobalSettings.thirdPartyAddressLocationApiUrl = "http://localhost:50234/api";
    GlobalSettings.unitManagerApiUrl = "http://localhost:50239/api";
    GlobalSettings.pdfGeneratorApiUrl = "http://localhost:50241/api";
    GlobalSettings.routeLogApiUrl = "http://localhost:50240/api";
    GlobalSettings.searchManagerApiUrl = "http://localhost:50243/api";
    GlobalSettings.indexUrl = "http://localhost:51978/app/index.html"
}
else if (GlobalSettings.env === "dev") {
    GlobalSettings.apiUrl = "http://10.246.18.250/fmoapi/api"; // Here comes development enviroment url
    GlobalSettings.indexUrl = "http://10.246.18.250/fmoui/app/index.html"
}
else if (GlobalSettings.env === "test") {
    GlobalSettings.apiUrl = "http://10.246.18.217/fmoapi/api"; // Here comes test enviroment url
    GlobalSettings.indexUrl = "http://10.246.18.217/fmoui/app/index.html"
}
else if (GlobalSettings.env === "prod") {
    GlobalSettings.apiUrl = ""; // Here comes production enviroment url
    GlobalSettings.indexUrl = ""
}

GlobalSettings.vectorMapUrl = "http://api.tiles.mapbox.com/v4/digitalglobe.nal0g75k/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiZGlnaXRhbGdsb2JlIiwiYSI6ImNpcGg5dHkzYTAxM290bG1kemJraHU5bmoifQ.CHhq1DFgZPSQQC-DYWpzaQ";
GlobalSettings.bingMapKey = "Arja12vfzFSIIbJWozrlmn3bVgk-G-mKz1pHNcYUtJ1_sJV3mpZIna-ExcYUxA2F";
GlobalSettings.unitBoundaryLayerName = "Unit Boundary";
GlobalSettings.deliveryPointLayerName = "Delivery Points";
GlobalSettings.accessLinkLayerName = "Access Links";
GlobalSettings.roadLinkLayerName = "Roads";
GlobalSettings.deliveryPointDetails = "Details of Delivery Point";


/*delivery-point colors*/
GlobalSettings.dpColor=['#ffff00', '#00ff00',  '#9999ff',  '#ffff99',  '#ff99cc',  '#ff8080',  '#00ccff',  '#008000',  '#ff6600',  '#c0c0c0',  '#808000',  '#ff9900',  '#ccffcc',  '#cc99ff',  '#0000ff',  '#008080',  '#993300',  '#ff0000','#da202a'];

angular.module('RMApp')
.constant("GlobalSettings", GlobalSettings);

//---URL Constants-------------------------------

//----Search Factory-------------//
GlobalSettings.fetchBasicSearchResults = "/searchmanager/basic/";
GlobalSettings.getDeliveryPointByUDPRN = "/DeliveryPointManager/deliverypoint/udprn/{0}";

//----Advance Search Factory-------------//
GlobalSettings.fetchAdvanceSearchResults = "/searchmanager/advance/";

//----Access Link Factory-------------//
GlobalSettings.getAdjustedPathLength = "/accessLinkManager/accessLink/pathlength";
GlobalSettings.createAccessLink = "/accessLinkManager/accessLink/manual/";
GlobalSettings.checkAccessLinkIsValid = "/accessLinkManager/accessLink/valid/";

//------Delivery Point Factory--------//
GlobalSettings.getDeliveryPointsResultSet = "/postaladdressmanager/postaladdress/search/{0}";
GlobalSettings.getAddressByPostCode = "/postaladdressmanager/postaladdress/filter?selectedItem={0}";
GlobalSettings.getAddressLocation = "/thirdpartyaddresslocationmanager/addresslocation/geojson/udprn:";
GlobalSettings.getPostalAddressByGuid = "/postaladdressmanager/postaladdress/filter/addressguid:{0}";
GlobalSettings.createDeliveryPoint = "/DeliveryPointManager/deliverypoint/newdeliverypoint";
GlobalSettings.updateDeliverypoint = "/DeliveryPointManager/deliverypoint/";

//-----Layers Factory---------------//
GlobalSettings.fetchDeliveryPointsByBoundingBox = "/deliverypointmanager/deliverypoints?bbox=";
GlobalSettings.fetchAccessLinksByBoundingBox = "/accesslinkmanager/accessLinks?bbox=";
GlobalSettings.fetchRouteLinksByBoundingBox = "/networkmanager/routelinks?bbox=";

//-----Manage Access Factory---------------//
GlobalSettings.getToken = "/token";

//-----Map view Factory---------------//
GlobalSettings.setAccessLinkByBoundingBox = "/accesslinkmanager/accessLinks?bbox=";
GlobalSettings.GetRouteForDeliveryPoint = "/DeliveryPointManager/deliverypoint/routes/{0}";

//-----Reference Data Factory---------------//
GlobalSettings.getReferenceData = "./reference-data/ReferenceData.js";
GlobalSettings.readJson = "./UI-string.json";

//-----RouteLog Factory---------------//
GlobalSettings.getRouteLogSelectionType = "/RouteLog/RouteLogsSelectionType";
GlobalSettings.getRouteLogStatus = "/RouteLog/RouteLogsStatus";
GlobalSettings.getDeliveryRouteScenario = "/UnitManager/scenario/{0}/{1}/ScenarioName,ID";
GlobalSettings.getDeliveryRoutes = "/DeliveryRouteManager/deliveryroute/{0}/{1}/DisplayText,ID"
GlobalSettings.getRouteDetailsByGUID = "/DeliveryRouteManager/deliveryroute/routedetails/{0}";
GlobalSettings.generateRouteLogSummaryReport = "/RouteLogManager/routelogs/";

//-----Simulation Factory---------------//
GlobalSettings.getRouteLogSimulationStatus = "/RouteSimulation/RouteLogsStatus";
GlobalSettings.getRouteSimulationScenario = "/RouteSimulation/FetchDeliveryScenario?operationStateID={0}&deliveryUnitID={1}&fields=ScenarioName,ID";
GlobalSettings.getRouteSimulationRoutes = "/RouteSimulation/FetchDeliveryRoute?operationStateID={0}&deliveryScenarioID={1}&fields=DisplayText,ID"
//-----Unit Selector Factory---------------//
GlobalSettings.getDeliveryUnit = "/UnitManager/Unit";

//-----Pdf Generator Factory---------------//
GlobalSettings.getPdfreport = "/PDFGenerator/PDFReports/";