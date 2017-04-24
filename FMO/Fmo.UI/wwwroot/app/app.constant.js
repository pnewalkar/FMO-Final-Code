//(function (window) {
//    window.__env = window.__env || {};
//    window.__env.apiUrl = 'http://localhost:34583/api';
//}(this));

var GlobalSettings = {
    apiUrl: '',
    env: 'localhost', // Here set the current environment
    indexUrl:'http://10.246.18.250/fmoui/app/index.html'
};

/* For Api */
//if (window.location.hostname == "localhost") {
//    GlobalSettings.apiUrl = "http://localhost:34583/api";
//}
//else {
//    GlobalSettings.apiUrl = "";
//}

if (GlobalSettings.env == "localhost") {
    GlobalSettings.apiUrl = "http://10.246.18.250/fmoapi";
}
else if (GlobalSettings.env == "dev") {
    GlobalSettings.apiUrl = ""; // Here comes development enviroment url
}
else if (GlobalSettings.env == "test") {
    GlobalSettings.apiUrl = ""; // Here comes test enviroment url
}
else if (GlobalSettings.env == "prod") {
    GlobalSettings.apiUrl = ""; // Here comes production enviroment url
}

GlobalSettings.vectorMapUrl = "http://api.tiles.mapbox.com/v4/digitalglobe.nal0g75k/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiZGlnaXRhbGdsb2JlIiwiYSI6ImNpcGg5dHkzYTAxM290bG1kemJraHU5bmoifQ.CHhq1DFgZPSQQC-DYWpzaQ";
GlobalSettings.unitBoundaryLayerName = "Unit Boundary";
GlobalSettings.deliveryPointLayerName = "Delivery Points";
GlobalSettings.accessLinkLayerName = "Access Links";
GlobalSettings.roadLinkLayerName = "Roads";

angular.module('FMOApp')
.constant("GlobalSettings", GlobalSettings);
