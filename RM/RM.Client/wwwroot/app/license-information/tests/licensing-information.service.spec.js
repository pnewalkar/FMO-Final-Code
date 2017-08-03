describe('Licensing Info: Service', function () {
    var rootScope;
    var scope;
    var $q;
    var defrred;
    var mapService
    var referencedataApiService;
    var licensingInfoService;
    var sessionStorage;
    var selectedDeliveryUnit = [{ "id": "B5E7ED0D-C744-4E86-B0C0-0D967F595E93", "area": "BT", "Unit": "" }];
    var layerMockData = {layer:'selected',selectorVisible:true,layerName:'test' };
    var LicensingInfoMockData = [{ "id": "B5E7ED0D-C744-4E86-B0C0-0D967F595E93", "ReferenceDataName": "OrdnanceSurvey_NI_Licensing", "ReferenceDataValue": "This Intellectual Property is based upon Crown Copyright and is reproduced with the permission of Land & Property Services under Delegated Authority from the Controller of Her Majesty’s Stationery Office, © Crown Copyright and database right [year of publication]. Contains public sector information licensed under the terms of the Open Government Licence v3.0" },
        { "id": "F499F99D-044B-423B-91F8-8E1AFA424115", "ReferenceDataName": "GeoPlan_Licensing", "ReferenceDataValue": "GEOPLAN © Geoplan Spatial Intelligence Ltd 2017. Postcodes are copyright of the Post Office 2017" },
     { "id": "4D8AF165-CC70-497A-8917-9506FD490C53", "ReferenceDataName": "OrdnanceSurvey_GB_Licensing", "ReferenceDataValue": "© Crown copyright and database rights 2017 OS 100025125 © Local Government Information House Limited copyright and database rights [year of supply or date of publication][licence number]. This product may contain data created and maintained by Scottish Local Government." },
     { "id": "0DAFEB1D-908A-415B-96D0-9E920D1B746C", "ReferenceDataName": "ThirdParty_GB_Licensing", "ReferenceDataValue": "© Crown copyright and database rights 2017 OS [insert license number]" },
     { "id": "5013AB9D-D8D5-473A-A9D2-C8179C138038", "ReferenceDataName": "ThirdParty_NI_Licensing", "ReferenceDataValue": "This Intellectual Property is based upon Crown Copyright and is reproduced with the permission of Land & Property Services under Delegated Authority from the Controller of Her Majesty’s Stationery Office, © Crown Copyright and database right [year of publication]. Contains public sector information licensed under the terms of the Open Government Licence v3.0" }
    ];
    var getLicensingInfoMockData = [{ "id": "B5E7ED0D-C744-4E86-B0C0-0D967F595E93", "ReferenceDataName": "OrdnanceSurvey_NI_Licensing", "ReferenceDataValue": "This Intellectual Property is based upon Crown Copyright and is reproduced with the permission of Land & Property Services under Delegated Authority from the Controller of Her Majesty’s Stationery Office, © Crown Copyright and database right [year of publication]. Contains public sector information licensed under the terms of the Open Government Licence v3.0" },
        { "id": "F499F99D-044B-423B-91F8-8E1AFA424115", "ReferenceDataName": "GeoPlan_Licensing", "ReferenceDataValue": "GEOPLAN © Geoplan Spatial Intelligence Ltd 2017. Postcodes are copyright of the Post Office 2017" },
     { "id": "4D8AF165-CC70-497A-8917-9506FD490C53", "ReferenceDataName": "OrdnanceSurvey_GB_Licensing", "ReferenceDataValue": "© Crown copyright and database rights 2017 OS 100025125 © Local Government Information House Limited copyright and database rights [year of supply or date of publication][licence number]. This product may contain data created and maintained by Scottish Local Government." },
     { "id": "0DAFEB1D-908A-415B-96D0-9E920D1B746C", "ReferenceDataName": "ThirdParty_GB_Licensing", "ReferenceDataValue": "© Crown copyright and database rights 2017 OS [insert license number]" },
     { "id": "5013AB9D-D8D5-473A-A9D2-C8179C138038", "ReferenceDataName": "ThirdParty_NI_Licensing", "ReferenceDataValue": "This Intellectual Property is based upon Crown Copyright and is reproduced with the permission of Land & Property Services under Delegated Authority from the Controller of Her Majesty’s Stationery Office, © Crown Copyright and database right [year of publication]. Contains public sector information licensed under the terms of the Open Government Licence v3.0" }
    ];

    beforeEach(function () {
        module('licensingInfo');              
        module(function ($provide) {
            $provide.service('licensingInfoService', function ($q) {
                function LicensingInfo(pdfFilename) {
                    var deferred = $q.defer();
                    deferred.resolve(LicensingInfoMockData);
                    return deferred.promise;
                }

                function getLicensingInfo(printMapDTO) {
                    var deferred = $q.defer();
                    deferred.resolve(getLicensingInfoMockData);
                    return deferred.promise;
                }

                return {
                    LicensingInfo: LicensingInfo,
                    getLicensingInfo: getLicensingInfo
                };
            });

            $provide.service('mapService', function () {
                function mapLayers() { return layerMockData; }
                return {
                    mapLayers: mapLayers
                };
            });

            $provide.service('referencedataApiService', function ($q) {
                function getNameValueReferenceData(appGroupName) {
                    var deferred = $q.defer();
                    deferred.resolve(LicensingInfoMockData);
                    return deferred.promise;
                }
                return {
                    getNameValueReferenceData:getNameValueReferenceData
                };

            });

            $provide.service('sessionStorage', function () {
                
                function getItem(data) {
                    return selectedDeliveryUnit;
                }
                function setItem() {
                    return {area:"BT"};
                }
                return {
                    getItem: getItem,
                    setItem: setItem
                }
            });

        });

        inject(function (_$rootScope_, _$q_, _mapService_, _licensingInfoService_, _referencedataApiService_, _sessionStorage_) {
            scope = _$rootScope_.$new();
            rootScope = _$rootScope_;
            $q = _$q_;
            mapService = _mapService_;
            licensingInfoService = _licensingInfoService_;
            referencedataApiService = _referencedataApiService_;
            sessionStorage = _sessionStorage_;
          
        });

    });

    xit('should be return promise response when call LicensingInfo', function (done) {      
        licensingInfoService.LicensingInfo().then(function (response) {
           
            expect(response).toBeDefined();
            expect(response).toBe(LicensingInfoMockData);
            done();
        });
        scope.$digest();
    });

    it('should be initialize LicensingInfo method', function () {
       spyOn(licensingInfoService, 'LicensingInfo');
       licensingInfoService.LicensingInfo();
        expect(licensingInfoService.LicensingInfo).toHaveBeenCalled();
    });

    it('should be selectedLayer return object when mapLayers method called', function () {

        var selectedLayer = null;
        var data = mapService.mapLayers();
        expect(layerMockData).toBe(data);
    });

    it('should be return promise response when call LicensingInfo', function () {

        var Map_License_Information = "dommyInfo";
        referencedataApiService.getNameValueReferenceData(Map_License_Information).then(function (response) {
            var aValue = sessionStorage.getItem('selectedDeliveryUnit');
            var selectedUnitArea = angular.fromJson(aValue);
            expect(selectedUnitArea).toBeDefined();

            expect(response).toBeDefined();
            expect(response).toBe(LicensingInfoMockData);
           
        });
      
        scope.$digest();
    });

    it('should be return promise response when call getLicensingInfo', function (done) {
        licensingInfoService.getLicensingInfo().then(function (response) {
            expect(response).toBeDefined();
            expect(response).toBe(getLicensingInfoMockData);
            done();
        });
        scope.$digest();
    });

});