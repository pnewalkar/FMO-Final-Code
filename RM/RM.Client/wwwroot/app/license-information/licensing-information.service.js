angular.module('licensingInfo')
        .service('licensingInfoService', licensingInfoService);
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
            if (selectedUnitArea.area === GlobalSettings.BT) {
                if (selectedLayer.layerName !== GlobalSettings.baseLayerName) {

                    result = response.filter(function (e) {
                        return (e.name == GlobalSettings.OrdnanceSurvey_NI_Licensing);
                    });

                } else {

                    result = response.filter(function (e) {
                        return (e.name == GlobalSettings.ThirdParty_NI_Licensing);
                    });
                }

            } else {
                if (selectedLayer.layerName !== GlobalSettings.baseLayerName) {
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
            angular.forEach(result, function (value, key) {
                if (value != "")
                    result2 = result2 + '<p> ©' + value + '</p>';
            });

            $rootScope.$emit('LicensingInfoText', { displayText: result2 });
        });       
    }
};
