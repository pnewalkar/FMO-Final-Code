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
    GlobalSettings.mapManagerApiUrl = "http://localhost:1386/api";
    GlobalSettings.indexUrl = "http://localhost:51978/app/index.html"
}
else if (GlobalSettings.env === "dev") {
    GlobalSettings.actionManagerApiUrl = "http://172.18.5.7/ActionManager/api";
    GlobalSettings.referenceDataApiUrl = "http://172.18.5.7/ReferenceData/api";
    GlobalSettings.accessLinkApiUrl = "http://172.18.5.7/AccessLink/api";
    GlobalSettings.deliveryPointApiUrl = "http://172.18.5.7/DeliveryPoint/api";
    GlobalSettings.deliveryRouteApiUrl = "http://172.18.5.7/DeliveryRoute/api";
    GlobalSettings.networkManagerApiUrl = "http://172.18.5.7/NetworkManager/api";
    GlobalSettings.postalAddressApiUrl = "http://172.18.5.7/PostalAddress/api";
    GlobalSettings.specialInstructionApiUrl = "http://172.18.5.7/SpecialInstruction/api";
    GlobalSettings.thirdPartyAddressLocationApiUrl = "http://172.18.5.7/ThirdPartyAddressLocation/api";
    GlobalSettings.unitManagerApiUrl = "http://172.18.5.7/UnitManager/api";
    GlobalSettings.pdfGeneratorApiUrl = "http://172.18.5.7/PDFGenerator/api";
    GlobalSettings.routeLogApiUrl = "http://172.18.5.7/RouteLog/api";
    GlobalSettings.searchManagerApiUrl = "http://172.18.5.7/SearchManager/api";
    GlobalSettings.mapManagerApiUrl = "http://172.18.5.7/MapManager/api";
    GlobalSettings.indexUrl = "http://172.18.4.4/app/index.html";
}
else if (GlobalSettings.env === "test") {
    GlobalSettings.actionManagerApiUrl = "http://172.18.5.12/ActionManager/api";
    GlobalSettings.referenceDataApiUrl = "http://172.18.5.12/ReferenceData/api";
    GlobalSettings.accessLinkApiUrl = "http://172.18.5.12/AccessLink/api";
    GlobalSettings.deliveryPointApiUrl = "http://172.18.5.12/DeliveryPoint/api";
    GlobalSettings.deliveryRouteApiUrl = "http://172.18.5.12/DeliveryRoute/api";
    GlobalSettings.networkManagerApiUrl = "http://172.18.5.12/NetworkManager/api";
    GlobalSettings.postalAddressApiUrl = "http://172.18.5.12/PostalAddress/api";
    GlobalSettings.specialInstructionApiUrl = "http://172.18.5.12/SpecialInstruction/api";
    GlobalSettings.thirdPartyAddressLocationApiUrl = "http://172.18.5.12/ThirdPartyAddressLocation/api";
    GlobalSettings.unitManagerApiUrl = "http://172.18.5.12/UnitManager/api";
    GlobalSettings.pdfGeneratorApiUrl = "http://172.18.5.12/PDFGenerator/api";
    GlobalSettings.routeLogApiUrl = "http://172.18.5.12/RouteLog/api";
    GlobalSettings.searchManagerApiUrl = "http://172.18.5.12/SearchManager/api";
    GlobalSettings.mapManagerApiUrl = "http://172.18.5.12/MapManager/api";
    GlobalSettings.indexUrl = "http://172.18.4.8/app/index.html";
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
GlobalSettings.baseLayerName = "Base Layer";
GlobalSettings.drawingLayerName = "Drawing";
GlobalSettings.deliveryPointDetails = "Details of Delivery Point";
GlobalSettings.single = "Single";
GlobalSettings.range = "Range";
GlobalSettings.subBuilding = "Sub building";
GlobalSettings.numberInName = "Number in Name";
GlobalSettings.defaultRangeOption = "Odds";


angular.module('RMApp')
.constant("GlobalSettings", GlobalSettings);

//---URL Constants-------------------------------

//----Search Factory-------------//
GlobalSettings.fetchBasicSearchResults = "/searchmanager/basic/";
GlobalSettings.getDeliveryPointById = "/DeliveryPointManager/deliverypoint/Guid/{0}";

//----Advance Search Factory-------------//
GlobalSettings.fetchAdvanceSearchResults = "/searchmanager/advance/";

//----Access Link Factory-------------//
GlobalSettings.getAdjustedPathLength = "/accessLinkManager/accessLink/pathlength";
GlobalSettings.createAccessLink = "/accessLinkManager/accessLink/manual/";
GlobalSettings.checkAccessLinkIsValid = "/accessLinkManager/accessLink/valid/";

//------Delivery Point Factory--------//
GlobalSettings.getDeliveryPointsResultSet = "/UnitManager/postaladdress/search/{0}";
GlobalSettings.getAddressByPostCode = "/UnitManager/postaladdress/filter?selectedItem={0}";
GlobalSettings.getAddressLocation = "/thirdpartyaddresslocationmanager/addresslocation/geojson/udprn:";
GlobalSettings.getPostalAddressByGuid = "/postaladdressmanager/postaladdress/filter/addressguid:{0}";
GlobalSettings.createDeliveryPoint = "/DeliveryPointManager/deliverypoint/newdeliverypoint";
GlobalSettings.updateDeliverypointForRange = "/DeliveryPointManager/deliverypoint/range";
GlobalSettings.createDeliveryPointsRange = "/DeliveryPointManager/deliverypoint/newdeliverypoint/range";
GlobalSettings.validateDeliveryPoints = "/DeliveryPointManager/deliverypoint/newdeliverypoint/range/check";
GlobalSettings.createDeliveryPointsRange = "/DeliveryPointManager/deliverypoint/newdeliverypoint/range";

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
GlobalSettings.getDeliveryRoutes = "/DeliveryRouteManager/deliveryroute/{0}/DisplayText,ID,RouteName,RouteNumber"
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

//-----Map manager Factory---------------//
GlobalSettings.generateReportWithMap = "/MapManager/MapImage/";
GlobalSettings.generatePdf = "/MapManager/MapPDF/";

//--- Unit Area ---///
GlobalSettings.BT = "BT";

//--- Licensing Information-----//
GlobalSettings.Map_License_Information = "Map_License_Information";
GlobalSettings.OrdnanceSurvey_GB_Licensing = "OrdnanceSurvey_GB_Licensing";
GlobalSettings.ThirdParty_GB_Licensing = "ThirdParty_GB_Licensing";
GlobalSettings.OrdnanceSurvey_NI_Licensing = "OrdnanceSurvey_NI_Licensing";
GlobalSettings.ThirdParty_NI_Licensing = "ThirdParty_NI_Licensing";
GlobalSettings.GeoPlan_Licensing = "GeoPlan_Licensing";