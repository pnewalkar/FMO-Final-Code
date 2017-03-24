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
angular.module('RMGApp')
.constant("GlobalSettings", GlobalSettings);
