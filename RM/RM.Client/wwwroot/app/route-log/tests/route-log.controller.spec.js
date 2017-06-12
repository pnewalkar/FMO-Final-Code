describe('Route-Log: Controller', function () {

    var mockService;
    var controller;
    var items;
    var vm;
    var $q;
    var defrred;
    var $httpBackend;
    var $scope;

    //Set to Mock JsonData
    var loadSelectionTypeMockData = [{
        "RouteselectionTypeObj": [
                              { "referenceDataName": null, "referenceDataValue": "Single", "dataDescription": "Single", "displayText": null, "dataParentId": null, "id": "ffeb0dbc-d12b-e711-8735-28d244aef9ed", "referenceDataCategory_GUID": "5c64cd73-d12b-e711-8735-28d244aef9ed", "dataParent_GUID": null },
                              { "referenceDataName": null, "referenceDataValue": "Multiple", "dataDescription": "Multiple", "displayText": null, "dataParentId": null, "id": "01ae2cc6-d12b-e711-8735-28d244aef9ed", "referenceDataCategory_GUID": "5c64cd73-d12b-e711-8735-28d244aef9ed", "dataParent_GUID": null }],
        "selectedRouteSelectionObj": { "referenceDataName": null, "referenceDataValue": "Single", "dataDescription": "Single", "displayText": null, "dataParentId": null, "id": "ffeb0dbc-d12b-e711-8735-28d244aef9ed", "referenceDataCategory_GUID": "5c64cd73-d12b-e711-8735-28d244aef9ed", "dataParent_GUID": null }
    }];

    var loadRouteLogStatusMockData = [
   										{ "id": "9c1e56d7-5397-4984-9cf0-cd9ee7093c88", "name": null, "value": "Live", "displayText": null, "description": "Live" },
   										{ "id": "bee6048d-79b3-49a4-ad26-e4f5b988b7ab", "name": null, "value": "Not Live", "displayText": null, "description": "Not Live" }
    ];

    var deliveryRouteChangeMockData = { "id": "33bc42ed-42bd-4456-a3f6-33a10efc8bd3", "externalId": null, "routeName": "1102 UPPER HIGH STREET        ", "routeNumber": "102       ", "operationalStatus_Id": 0, "routeMethodType_Id": 0, "travelOutTransportType_Id": null, "travelInTransportType_Id": null, "travelOutTimeMin": null, "travelInTimeMin": null, "spanTimeMin": null, "deliveryScenario_Id": null, "deliveryRouteBarcode": null, "displayText": "(102)1102 UPPER HIGH STREET        ", "methodReferenceGuid": "c168f46e-561b-e711-9f8c-28d244aef9ed", "method": "High Capacity Trolley", "deliveryOffice": null, "aliases": 0, "blocks": 2, "scenarioName": "Worthing Delivery Office - Baseline weekday", "dPs": 524, "businessDPs": 8, "residentialDPs": 516, "travelOutTransportType_GUID": "c168f46e-561b-e711-9f8c-28d244aef9ed", "travelInTransportType_GUID": "c168f46e-561b-e711-9f8c-28d244aef9ed", "accelarationIn": "High Capacity Trolley", "accelarationOut": "High Capacity Trolley", "pairedRoute": "", "totaltime": "0:07 mins" };

    var scenarioChangeMockData = { "isDeliveryRouteDisabled": false, "isShowMultiSelectionRoute": false };

    var loadScenarioMockData = [{ "scenarioName": "Worthing Delivery Office - Baseline weekday", "id": "b51aa229-c984-4ca6-9c12-510187b81050" }];

    var loadDeliveryRouteMockData = [{ "deliveryRoute": [{ "displayText": "(415)1415 BROADWATER ST EAST       ", "id": "a5065aec-a4ba-4f94-8c0a-0045d1131966" }, { "displayText": "(313)1313 SALVINGTON ROAD          ", "id": "bbe72234-3b1b-442e-8e75-00824e229360" }, { "displayText": "(431)1431 LINKS ROAD               ", "id": "b13d545d-2de7-4e62-8dad-00ec2b7ff8b8" }, { "displayText": "(302)1302 STONEHURST ROAD          ", "id": "61474827-7844-4969-bdca-03b2e0ab8108" }, { "displayText": "(430)1430 NEPCOTE LANE             ", "id": "102359d0-7558-40ae-975b-058f69d36c0c" }, { "displayText": "(F007)FIRMS 7                       ", "id": "cfcab05f-cdec-475e-87be-0c4b0bc881b2" }, { "displayText": "(311)1311 CLEVELAND ROAD           ", "id": "822c4706-94d8-40ef-82e2-0ca453af77ec" }, { "displayText": "(413)1413 BROADWATER ST WEST       ", "id": "7243ed5a-6b87-4e1f-a275-0cf956e59120" }, { "displayText": "(61B)GATWICK MAIL CENTRE DLVY 061  ", "id": "22a125a5-a3b6-4e5e-bdb3-11c420b6d5e3" }, { "displayText": "(505)FIRMS 5                       ", "id": "f683bf82-ffca-40ee-9cf6-1c49f6499a96" }, { "displayText": "(323)1323 NEW ROAD                 ", "id": "4cd23e81-34f6-4260-bf49-1cb25794591f" }, { "displayText": "(326)1326 SILVERBIRCH              ", "id": "405ad50e-d11c-4ccb-bf03-1fde41586bbe" }, { "displayText": "(416)1416 SHANDON ROAD             ", "id": "a9038f07-bd28-4bf9-a3f4-268ec14318d9" }, { "displayText": "(322)1322 VANCOUVER ROAD           ", "id": "3b10d88b-66b5-4ea9-af64-27ee002c2ab5" }, { "displayText": "(910)WORTHING SOUTHERN WATER BOX 41", "id": "98b52b66-e3b2-47c2-b574-2a0b8754b96f" }, { "displayText": "(318)1318 SALVINGTON HILL          ", "id": "17b510e6-a843-4194-aa66-2b48cd96f555" }, { "displayText": "(408)1408 OFFINGTON AVE            ", "id": "8efa25e0-0eab-4600-a3aa-2cee65255a53" }, { "displayText": "(128)1128 MILL ROAD                ", "id": "ba3ee301-428d-4b8c-b4e0-2d2098f64661" }, { "displayText": "(140B)NIGHT BULK 140 B              ", "id": "364ce838-8440-460c-9036-3200cb662896" }, { "displayText": "(114)1114 MARINE PARADE            ", "id": "c9006332-ec6c-48fc-82fa-32d5ce944b4c" }, { "displayText": "(102)1102 UPPER HIGH STREET        ", "id": "33bc42ed-42bd-4456-a3f6-33a10efc8bd3" }, { "displayText": "(409)1409 WARREN ROAD              ", "id": "b23330a5-682a-4593-9679-36b8d3dcdbb0" }, { "displayText": "(120)1120 VICTORIA ROAD            ", "id": "073d6187-58ba-4b14-b682-377abee09ecc" }, { "displayText": "(404)1404 BULKINGTON AVE           ", "id": "d70632c4-a345-4a28-936f-3a4cc52412de" }, { "displayText": "(118)1118 DOWNVIEW ROAD            ", "id": "cc619be3-4dfc-4de2-bf2b-3af279c02151" }, { "displayText": "(129)1129 GEORGE V AVENUE          ", "id": "6305d1e3-65c6-43fa-88a6-3b70b3ccab45" }, { "displayText": "(321)1321 COLUMBIA DRIVE           ", "id": "a7aa77a7-1083-4559-bddf-3ce2e9ed6d8b" }, { "displayText": "(121)1121 WORDSWORTH ROAD          ", "id": "ca6c6506-dbda-4d3e-8444-490cff35a7c0" }, { "displayText": "(417)1417 SOMPTING ROAD            ", "id": "2f53b6b5-0ff3-4868-9d8e-49a81bb61d87" }, { "displayText": "(301)1301 LITTLEHAMPTON ROAD       ", "id": "15a3f8ac-ca12-4cf1-add9-4bc88a028417" }, { "displayText": "(305)1305 TERRINGS AVENUE          ", "id": "f18980f2-4e3d-4d7c-8963-4ccac5e66075" }, { "displayText": "(425)1425 FINDON ROAD              ", "id": "14abcb57-1e19-4db8-a1bd-50e18c38ff75" }, { "displayText": "(F004)FIRMS 4                       ", "id": "60d8246a-e99c-4b8f-9a44-522d0a30c85e" }, { "displayText": "(913)WORTHING BOX 4762             ", "id": "ecc4748d-8dbb-4f36-8620-52a49e295e79" }, { "displayText": "(926)WORTHING BOX 4016             ", "id": "27c5b7e8-3c39-4ee1-8c6f-5bb635b4e0fc" }, { "displayText": "(315)1315 BURNHAM ROAD             ", "id": "7fa1e8af-7afe-483c-bb86-5ccbee9f9d45" }, { "displayText": "(124)1124 BELSIZE ROAD             ", "id": "893966d4-5539-47c3-98cd-5deb081791ce" }, { "displayText": "(317)1317 HAYLING RISE             ", "id": "f7b1ae87-5002-4063-8c36-5f701b54673a" }, { "displayText": "(309)1309 STONE LANE               ", "id": "9e66845b-ead3-423a-ab0c-60b21abbec01" }, { "displayText": "(325)1325 IVYDORE AVE              ", "id": "5d54c55b-d0db-4032-b922-617e01a3b143" }, { "displayText": "(429)1429 BEECH ROAD               ", "id": "73716822-a023-4389-b1f0-638def42475f" }, { "displayText": "(402)1402 SOUTH STREET             ", "id": "1e0ff5a0-4806-4efa-a6bb-64e8b72a88f5" }, { "displayText": "(503)FIRMS 3                       ", "id": "4facbe41-862b-4429-9793-64fffdffdb0e" }, { "displayText": "(428)1428 BOST HILL                ", "id": "2e10fee4-7503-4715-a95d-651c3c392fbe" }, { "displayText": "(125)1125 GRAND AVENUE             ", "id": "cdb89f0f-bc03-4c93-8f0d-6592b5f8ae8f" }, { "displayText": "(502)FIRMS 2                       ", "id": "78dc97e5-e273-427b-ad7c-65d4d7b91ec3" }, { "displayText": "(105)1105 FARNCOMBE ROAD           ", "id": "2bc7425f-934d-4ad0-a70f-6723e54c4f40" }, { "displayText": "(312)1312 EXMOOR DRIVE             ", "id": "51041d48-bfb5-4410-9c30-6a869cc84ac1" }, { "displayText": "(412)1412 NORTHCOURT ROAD          ", "id": "c9e69d5b-823a-4aae-a896-6c56aea812ff" }, { "displayText": "(110)1110 MEADOW ROAD              ", "id": "d617d115-6db8-4aae-a863-6c766a97709f" }, { "displayText": "(304)1304 CANTERBURY ROAD          ", "id": "9e46cb02-f6ad-4f4c-977f-747af4d6902d" }, { "displayText": "(113)1113 MONTAGUE STREET          ", "id": "e6c8ea85-16c1-4634-906f-7660a4f3abeb" }, { "displayText": "(126)1126 BRUCE AVE                ", "id": "422ff19d-9727-46b8-9720-76da3eb9fa83" }, { "displayText": "(103)1103 WESTBROOKE               ", "id": "129751dc-665e-40b0-8941-777568fec2ce" }, { "displayText": "(328)1328 SAXONS PLAIN             ", "id": "823b57cb-6440-408f-9867-79d7158cd045" }, { "displayText": "(419)1419 DOMINION ROAD            ", "id": "cf092902-edf2-49d9-80f1-7fa06172b1e9" }, { "displayText": "(411)1411 BECKET ROAD              ", "id": "088613cd-b1df-41b5-9cfa-805ad24977a3" }, { "displayText": "(426)1426 THE HEIGHTS              ", "id": "c4769804-2f60-4152-a8ab-82390430b8db" }, { "displayText": "(307)1307 THE BOULEVARD            ", "id": "411885f8-d432-4c80-9b27-844b412c236c" }, { "displayText": "(115)1115 WEST PARADE              ", "id": "3a42a143-5e94-4c6d-82ca-8a43d9e3daa1" }, { "displayText": "(107)1107 CHURCH WALK              ", "id": "0eecfbec-23f9-4996-b618-8a5d08387095" }, { "displayText": "(501)FIRMS 1                       ", "id": "cb037e73-dc82-40cc-bff3-8cf79b08c896" }, { "displayText": "(420)1420 CONGREVE ROAD            ", "id": "8f169b5d-ab3a-46fa-aa82-8eaa874447b6" }, { "displayText": "(104)1104 CHESSWOOD ROAD           ", "id": "ecaa146d-f862-4c20-99d8-8f65d85afd9e" }, { "displayText": "(316)1316 CASTLE GORING            ", "id": "bf27e0d1-f809-4453-8a08-961c9feac896" }, { "displayText": "(327)1327 CARISBROOKE DRIVE        ", "id": "1e995783-3d7a-4aee-977b-96e52da4a612" }, { "displayText": "(427)1427 CENTRAL AVE              ", "id": "9ea13efd-f974-4715-bfc9-97db01764eaf" }, { "displayText": "(127)1127 LANSDOWNE ROAD           ", "id": "8ec00e08-e571-4dcb-b185-9895095142be" }, { "displayText": "(122)1122 HEENE ROAD               ", "id": "96ee5366-2b15-4c9b-a57a-99235403be69" }, { "displayText": "(424)1424 VALE AVE                 ", "id": "5dad434f-9588-4a2f-9fea-a1a6593a0359" }, { "displayText": "(401)1401 PAVILION ROAD            ", "id": "56a9d36b-c22e-43fe-a008-a5df8e929c36" }, { "displayText": "(303)1303 ST ANDREWS               ", "id": "c0789ea5-9812-439c-8033-a6047ddc6678" }, { "displayText": "(108)1108 BRIGHTON ROAD            ", "id": "3e136e6c-b685-4279-bf96-a79aeee68405" }, { "displayText": "(320)1320 PATCHING                 ", "id": "73f592d3-c27c-4cae-b88c-a81862a5cd77" }, { "displayText": "(915)WORTHING BOX 4575             ", "id": "40e19829-dae7-4c68-bfd7-abc3e6845b3d" }, { "displayText": "(418)1418 CLARENDON ROAD           ", "id": "3161f530-58b4-428c-9cbb-ae515a977cf6" }, { "displayText": "(925)WORTHING BOX 4323             ", "id": "4cc4d126-793d-41e1-8a26-b133afcbab7e" }, { "displayText": "(405)1405 WISTON AVENUE            ", "id": "360f950a-d535-4009-8950-b20d883b14c3" }, { "displayText": "(403)1403 LAVINGTON ROAD           ", "id": "b10c4eb0-c864-4696-967e-b76d7142b7ca" }, { "displayText": "(F006)FIRMS 6                       ", "id": "f91769ee-d515-4a58-ab85-b958921d7a68" }, { "displayText": "(406)1406 LOXWOOD AVE              ", "id": "01b5c33b-860a-4cf5-b483-b9ba3653d2da" }, { "displayText": "(332)1332 FARADAY CLOSE            ", "id": "0ddb4e05-4c57-49e7-aaca-ba57c6bd708e" }, { "displayText": "(116)1116 ROWLANDS ROAD            ", "id": "b69a1d4e-c519-4c04-88cc-befdeab4b433" }, { "displayText": "(106)1106 LYNDHURST ROAD           ", "id": "f341972a-8d3c-4381-ade8-c03df85ab83a" }, { "displayText": "(314)1314 DURRINGTON LANE          ", "id": "4802bdf2-4cc1-4d26-b88a-c364bed9c6a1" }, { "displayText": "(130)1130 WALLACE AVE              ", "id": "6ca50f90-9639-4e17-b346-cb6db731ba9b" }, { "displayText": "(919)WORTHING BOX 4740             ", "id": "01e8243e-6b95-48bf-b83e-ccf0b1580d81" }, { "displayText": "(422)1422 KING EDWARD AVE          ", "id": "6abc893c-70c2-4618-bdd0-cee64fb077e8" }, { "displayText": "(421)1421 SACKVILLE ROAD           ", "id": "9d55e2f7-db77-4c70-a4f2-d1dd342e5332" }, { "displayText": "(319)1319 MILL LANE                ", "id": "517616de-8a53-47cc-be46-d4334ae1d7c1" }, { "displayText": "(912)WORTHING BOX 4014             ", "id": "ed587357-d3f5-47b7-a05e-d94959474e78" }, { "displayText": "(PHG)WORTHING PO BOX 1000          ", "id": "2b5d49e8-b248-4cec-98c3-dba74332d906" }, { "displayText": "(101)1101 NEWLAND ROAD             ", "id": "d8053df2-1aad-40c8-bbf6-de0ae783e772" }, { "displayText": "(306)1306 RINGMER ROAD             ", "id": "ef0daee5-004a-4940-9959-df97b7735e1f" }, { "displayText": "(308)1308 PRINCESS AVENUE          ", "id": "53625b6d-9548-4204-a121-e00b4ea2531a" }, { "displayText": "(123)1123 TARRING ROAD             ", "id": "2f105397-9781-429a-834d-e26852ac60e7" }, { "displayText": "(111)1111 BROUGHAM ROAD            ", "id": "04a796c2-1222-4cec-baf4-e30d6b809cbf" }, { "displayText": "(119)1119 SHELLEY ROAD             ", "id": "34990ad6-b8db-4716-9909-e3b2782da5ae" }, { "displayText": "(310)1310 THE PLANTATION           ", "id": "03502464-4548-44cb-9e3e-ea47e856c228" }, { "displayText": "(117)1117 BATH ROAD                ", "id": "4bc0163e-1aeb-43a1-a077-f33d52d0a47e" }, { "displayText": "(407)1407 POULTERS LANE            ", "id": "0cce2409-abee-48dc-a7f4-f51cce15344e" }, { "displayText": "(109)1109 HAM ROAD                 ", "id": "09fe48cc-21fa-4a5c-a670-f8343cf2c8e2" }, { "displayText": "(324)1324 ADUR AVENUE              ", "id": "93cd1ac0-f221-441a-aed7-fd2fe0fa5f8c" }, { "displayText": "(410)1410 FIRST AVE                ", "id": "12037830-5dfc-41ed-8367-fde157382067" }], "multiSelectiondeliveryRoute": null }];

    //Load our module and inject with dependencies provider
    beforeEach(function () {

        angular.mock.module('routeLog'); //load module

        //inject with mockdata routeLogService
        angular.mock.module(function ($provide) {
            $provide.factory('routeLogService', function ($q) {
                function loadSelectionType() {
                    deferred = $q.defer();
                    deferred.resolve(loadSelectionTypeMockData);
                    return deferred.promise;
                }
                function loadRouteLogStatus() {
                    deferred = $q.defer();
                    deferred.resolve(loadRouteLogStatusMockData);
                    return deferred.promise;
                }
                function closeWindow() { return [] }
                function loadScenario() {
                    deferred = $q.defer();
                    deferred.resolve(loadScenarioMockData);
                    return deferred.promise;
                }
                function loadDeliveryRoute() {
                    deferred = $q.defer();
                    deferred.resolve(loadDeliveryRouteMockData);
                    return deferred.promise;
                }

                function deliveryRouteChange() {
                    deferred = $q.defer();
                    deferred.resolve(deliveryRouteChangeMockData);
                    return deferred.promise;
                }

                function scenarioChange() {
                    return scenarioChangeMockData;
                }

                return {
                    loadSelectionType: loadSelectionType,
                    loadRouteLogStatus: loadRouteLogStatus,
                    closeWindow: closeWindow,
                    loadScenario: loadScenario,
                    loadDeliveryRoute: loadDeliveryRoute,
                    deliveryRouteChange: deliveryRouteChange,
                    scenarioChange: scenarioChange
                };
            });
        });

        //Inejct with mockdata routeLogAPIService
        angular.mock.module(function ($provide) {
            $provide.factory('routeLogAPIService', function ($q) {
                function getSelectionType() {
                    deferred = $q.defer();
                    deferred.resolve(['Single', 'Multiple']);
                    return deferred.promise;
                }
                function getStatus() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function getScenario() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function getRoutes() {
                    deferred = $q.defer();
                    return deferred.promise;
                }
                function getRouteDetailsByGUID() {
                    deferred = $q.defer();
                    //deferred.resolve();
                    return deferred.promise;
                }
                return {
                    getSelectionType: getSelectionType,
                    getStatus: getStatus,
                    getScenario: getScenario,
                    getRoutes: getRoutes,
                    getRouteDetailsByGUID: getRouteDetailsByGUID
                };
            });
        });

        //Inejct with mockdata routeLogAPIService
        angular.mock.module(function ($provide) {
            $provide.value('items', { externalId: "0", unitName: "Worthing  Office", unitAddressUDPRN: 2333402, area: "BN", id: "b51aa229-c984-4ca6-9c12-510187b81050" });

        });

        //get Instance of controller with inject properties
        angular.mock.inject(function (_$httpBackend_, $controller, routeLogService, _$rootScope_, items) {
            $rootScope = _$rootScope_;
            vm = $rootScope.$new();
            mockService = routeLogService;
            $httpBackend = _$httpBackend_;

            controller = $controller('RouteLogController', {
                routeLogService: mockService,
                items: items,
                $scope: vm

            })
        });

        spyOn($rootScope, '$broadcast').and.callThrough();
        spyOn($rootScope, '$on').and.callThrough();
        $rootScope.$broadcast.and.stub()
    });


    it('should be set md-text class is selected', function () {
        expect(controller.selectClass).toContain("routeSearch md-text");
    });

    it('Should be initialize selectionType and routelogstatus', function () {
        spyOn(controller, 'loadSelectionType').and.callThrough();
        spyOn(controller, 'loadRouteLogStatus').and.callThrough();

        controller.initialize();

        expect(controller.loadSelectionType).toHaveBeenCalled();
        expect(controller.loadRouteLogStatus).toHaveBeenCalled();
    });


    it('Should close the dialog-window', function () {
        spyOn(mockService, 'closeWindow').and.callThrough();
        controller.closeWindow();
        expect(mockService.closeWindow).toHaveBeenCalled();
    });

    it('should be clear search term', function () {

        spyOn(controller, 'clearSearchTerm').and.callThrough();
        var term = controller.clearSearchTerm();

        expect(controller.clearSearchTerm).toHaveBeenCalled();
        expect(controller.searchTerm).toBe('');
    });

    it('should be selected delivery unit is a object', function () {
        expect(controller.selectedDeliveryUnitObj).toEqual(jasmine.any(Object));
    });

    it('Should clear fields on changing selection type', function () {

        var typeChange = controller.selectionTypeChange();
        expect(controller.selectedRouteStatusObj).toBe(null);
        expect(controller.selectedRouteScenario).toBe(null);
        expect(controller.selectedRoute).toBe(null);
        expect(controller.isSelectionType).toBe(false);
        expect(controller.isRouteScenarioDisabled).toBe(true);
        expect(controller.isDeliveryRouteDisabled).toBe(true);
        expect(controller.isShowMultiSelectionRoute).toBe(false);

        //expect for clearDeliveyroute() method once done after call
        expect(controller.deliveryRoute).toBe(null);
        expect(controller.routeDetails).toBe(false);
    });

    it('should be select a route status and clear the delivery route ', function () {

        spyOn(controller, 'selectedRouteStatus').and.callThrough();

        //its need to appy rootscope update
        $rootScope.$apply();

        controller.selectedRouteStatus();


        expect(controller.isRouteScenarioDisabled).toBe(false);
        expect(controller.deliveryRoute).toBe(null);
        expect(controller.routeDetails).toBe(false);

    });

    it('Should be return object loadSelectionType method', function () {

        spyOn(mockService, 'loadSelectionType').and.callFake(function () {
            return responceData;
        });

        //Apply to scope update
        $rootScope.$apply();

        //check Single object also
        expect(controller.RouteselectionTypeObj).toBe(loadSelectionTypeMockData[0].RouteselectionTypeObj);
        expect(controller.selectedRouteSelectionObj.referenceDataValue).toBe(loadSelectionTypeMockData[0].selectedRouteSelectionObj.referenceDataValue);
        expect(controller.RouteselectionTypeObj.length).toBe(loadSelectionTypeMockData[0].RouteselectionTypeObj.length);
    });

    it('Should be return route log status after check loadScenario responce', function () {

        spyOn(mockService, 'loadRouteLogStatus').and.callFake(function () {
            return responceData;
        });

        //Apply to scope update
        $rootScope.$apply();

        //check Single object also
        expect(controller.RouteStatusObj).toBe(loadRouteLogStatusMockData);
        expect(controller.selectedRouteStatusObj).toBe(loadRouteLogStatusMockData[0]);

        //spyOn(controller,'loadScenario');
        //controller.loadScenario();
        //expect(controller.loadScenario).toHaveBeenCalled();
    });


    it('should be return object when loadScenario called and responce length > 0', function () {

        $rootScope.$apply();

        //Argument hardcode id
        var arg1 = "9c1e56d7-5397-4984-9cf0-cd9ee7093c88";
        var arg2 = "b51aa229-c984-4ca6-9c12-510187b81050";

        spyOn(mockService, 'loadScenario').and.callFake(function (arg1, arg2) {
            return loadScenarioMockData;
        });

        expect(controller.RouteScenario).toBeDefined();
        expect(controller.RouteScenario).toEqual(loadScenarioMockData);

    });

    it('should be return undefined when loadScenario called and responce blank', function () {

        spyOn(mockService, 'loadScenario').and.callFake(function () {
            return '';
        });

        expect(controller.RouteScenario).toBeUndefined();

    });

    xit('should be delivery route disabled when scenario changed', function () {

        spyOn(mockService, 'scenarioChange').and.callFake(function () {
            return scenarioChangeMockData;
        });

        $rootScope.$apply();

        controller.scenarioChange();
        /*
   		*if i remvoe id for scenarioChange inside loadDeliveryRoute then its work fine
   		*/
        //console.log(controller.selectedRouteStatusObj);
        expect(controller.isDeliveryRouteDisabled).toBe(scenarioChangeMockData.isDeliveryRouteDisabled);
        expect(controller.isShowMultiSelectionRoute).toBe(scenarioChangeMockData.isShowMultiSelectionRoute);

    });

    xit('should be cal load delivery route and clear delivery route when scenario changed', function () {

        spyOn(mockService, 'scenarioChange').and.callFake(function (single) {
            return scenarioChangeMockData;
        });

        $rootScope.$apply();

        controller.scenarioChange();

        //spy through cal load Delivery route and clear delivery route
        spyOn(controller, 'loadDeliveryRoute');
        controller.loadDeliveryRoute();
        expect(controller.loadDeliveryRoute).toHaveBeenCalled();

        spyOn(controller, 'clearDeliveyroute').toHaveBeenCalled();
        controller.clearDeliveyroute();
        expect(controller.clearDeliveyroute).toHaveBeenCalled();
    });


    xit('should be return promise response when called loadDeliveryRoute method', function () {

        //var selectedRouteSelectionObj = "Single";
        spyOn(mockService, 'loadDeliveryRoute').and.callFake(function (operationStateID, deliveryScenarioID) {
            return loadDeliveryRouteMockData;
        });

        $rootScope.$apply();

        console.log(controller.multiSelectiondeliveryRoute);



    });

    it('Should be delivery route change method return promise result', function () {

        spyOn(mockService, 'deliveryRouteChange').and.callFake(function () {

            return deliveryRouteChangeMockData;
        });


        //Apply to scope update
        $rootScope.$apply();

        //controller.deliveryRouteChange();
        console.log(controller.routeDetails);
    });

});
