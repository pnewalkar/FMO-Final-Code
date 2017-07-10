angular.module('licensingInfo')
        .service('licensingInfoService', licensingInfoService);
licensingInfoService.$inject = ['$q', 'mapService', 'referencedataApiService', '$rootScope', 'licensingInformationAccessorService'];
function licensingInfoService($q, mapService, referencedataApiService, $rootScope, licensingInformationAccessorService) {

    return {
        LicensingInfo: LicensingInfo,
        getLicensingInfo: getLicensingInfo,
        getLicensingText: getLicensingText
    };
    function LicensingInfo() {
        var selectedLayer = null;
        mapService.mapLayers().forEach(function (layer) {
            if (layer.selected == true && layer.selectorVisible == true)
                selectedLayer = layer;
        })
        var list = mapService.mapLayers();
        var deferred = $q.defer();
        referencedataApiService.getNameValueReferenceData(GlobalSettings.Map_License_Information).then(function (response) {
            var result = null;
            var aValue = sessionStorage.getItem('selectedDeliveryUnit');
            var selectedUnitArea = angular.fromJson(aValue);
            if (selectedLayer != null) {
                if (selectedUnitArea.area === GlobalSettings.BT) {
                    if (selectedLayer.layerName !== GlobalSettings.baseLayerName && selectedLayer.layerName !== GlobalSettings.accessLinkLayerName && selectedLayer.layerName !== GlobalSettings.unitBoundaryLayerName) {

                        result = response.filter(function (e) {
                            return (e.name == GlobalSettings.OrdnanceSurvey_NI_Licensing);
                        });

                    } else {
                        if (selectedLayer.layerName === GlobalSettings.baseLayerName) {
                            result = response.filter(function (e) {
                                return (e.name == GlobalSettings.ThirdParty_NI_Licensing);
                            });
                        }
                    }

                } else {
                    if (selectedLayer.layerName !== GlobalSettings.baseLayerName && selectedLayer.layerName !== GlobalSettings.accessLinkLayerName && selectedLayer.layerName !== GlobalSettings.unitBoundaryLayerName) {
                        var result = response.filter(function (e) {
                            return (e.name == GlobalSettings.OrdnanceSurvey_GB_Licensing);
                        });
                    }
                    else {
                        if (selectedLayer.layerName === GlobalSettings.baseLayerName)
                            {
                        result = response.filter(function (e) {
                            return (e.name == GlobalSettings.ThirdParty_GB_Licensing);
                        });
                        }
                    }
                }
                licensingInformationAccessorService.setLicensingInformation(result);
                deferred.resolve(result);
            }
        });
        return deferred.promise;

    }
    function getLicensingInfo(selectedUnitArea) {
        LicensingInfo().then(function (response) {
            var result = response[0].value.split('©');
            var result2 = '';
            angular.forEach(result, function (value, key) {
                if (value != "")
                    result2 = result2 + '<p> ©' + value + '</p>';
            });

            $rootScope.$emit('LicensingInfoText', { displayText: result2 });
        });
    }

    function getLicensingText(selectedLayer) {
        LicensingInfo().then(function (response) {

            if (response!=null){
                mapService.baseLayerLicensing();        
            }
        });
    }
};
