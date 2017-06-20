angular.module('licensingInfo')
        .factory('licensingInfoService', licensingInfoService);
licensingInfoService.$inject = ['$q','mapService', 'referencedataApiService', '$rootScope'];
function licensingInfoService($q,mapService, referencedataApiService, $rootScope) {

    return {
        LicensingInfo:LicensingInfo,
        getLicensingInfo: getLicensingInfo
    };
    function LicensingInfo() {
        var selectedLayer = null;
        mapService.mapLayers().forEach(function (layer) {
            if (layer.selected == true && layer.selectorVisible == true)
                selectedLayer = layer;
        })

        var deferred = $q.defer();
        referencedataApiService.getNameValueReferenceData(GlobalSettings.Map_License_Information).then(function (response) {
            var result = null;
            var aValue = sessionStorage.getItem('selectedDeliveryUnit');
            var selectedUnitArea = angular.fromJson(aValue);
            if (selectedUnitArea.area === "BT") {
                if (selectedLayer.layerName !== "Base Layer") {

                    result = response.filter(function (e) {
                        return (e.name == GlobalSettings.OrdnanceSurvey_NI_Licensing);
                    });

                } else {

                    result = response.filter(function (e) {
                        return (e.name == GlobalSettings.ThirdParty_NI_Licensing);
                    });
                }

            } else {
                if (selectedLayer.layerName !== "Base Layer") {
                    var result = response.filter(function (e) {
                        return (e.name == GlobalSettings.OrdnanceSurvey_GB_Licensing);
                    });
                }
                else {
                    result = response.filter(function (e) {
                        return (e.name == GlobalSettings.ThirdParty_GB_Licensing);
                    });
                }
            }
            deferred.resolve(result);
        });
        return deferred.promise;

    }
    function getLicensingInfo(selectedUnitArea) {
        LicensingInfo().then(function (response) {
            debugger;
            var result = response[0].value.split('©');

            var result2 = '';

            for (var i = 0; i < result.length; i++) {

                if (result[i] != "")
                    result2 = result2 + '<p> ©' + result[i] + '</p>';
            }
            $rootScope.$emit('LicensingInfoText', { displayText: result2 });

        });       
    }
};
