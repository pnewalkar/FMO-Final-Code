//(function (window) {
//    window.__env = window.__env || {};
//    window.__env.apiUrl = 'http://localhost:34583/api';
//}(this));

var GlobalSettings = {
    apiUrl: ''
};

/* For Api */
if (window.location.hostname == "localhost") {
    GlobalSettings.apiUrl = "http://localhost:34583/api";
}
else {
    GlobalSettings.apiUrl = "";
}

GlobalSettings.vectorMapUrl = "http://api.tiles.mapbox.com/v4/digitalglobe.nal0g75k/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiZGlnaXRhbGdsb2JlIiwiYSI6ImNpcGg5dHkzYTAxM290bG1kemJraHU5bmoifQ.CHhq1DFgZPSQQC-DYWpzaQ";
GlobalSettings.unitBoundaryLayerName = "Unit Boundary";
GlobalSettings.deliveryPointLayerName = "Delivery Points";
GlobalSettings.accessLinkLayerName = "Access Links";
GlobalSettings.roadLinkLayerName = "Roads";

angular.module('FMOApp')
.constant("GlobalSettings", GlobalSettings);
