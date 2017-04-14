'use strict';
angular.module('advanceSearch')
    .controller('advanceSearchController', ['$scope', 'searchApiService', 'mapFactory', '$state', '$mdDialog','advanceSearchService','$stateParams', AdvanceSearchController]);

function AdvanceSearchController($scope, searchApiService, mapFactory, $state, $mdDialog, advanceSearchService, $stateParams) {
    var vm = this;
    
    vm.chkRoute = true;
    vm.chkPostCode = true;
    vm.chkDeliveryPoint = true;
    vm.chkStreetNetwork = true;
    vm.queryAdvanceSearch = queryAdvanceSearch;
    vm.selectAll = selectAll;
    vm.deSelectAll = deSelectAll;
    var results = [];
    vm.searchText = $stateParams.data;
    vm.query;
    vm.routeval = [];
    var searchCount = [];
    vm.searchCount = searchCount;
    vm.OnChangeItem = OnChangeItem;
    vm.advanceSearch = advanceSearch;
    vm.openAdvanceSearchPopup = openAdvanceSearchPopup;
    var searchItem;
    debugger;
    function queryAdvanceSearch(query) {
       
        debugger;
        console.log(query);
        searchApiService.advanceSearch(query).then(function (response) {

            vm.results = response.data;
            vm.searchCount = vm.results.searchCounts;

            vm.results.searchCounts.splice(-1, 1)
            // vm.searchCount.remove('seven');
            console.log(vm.results.searchCounts);
            vm.searchItem = results.searchResultItems;
            vm.arrRoutes = [];

            var arrDeliverypoints = [];
            var arrPostCodes = [];
            var arrStreetNames = [];
            var arrDeliveryRoutes = [];


            for (var i = 0; i < vm.results.searchResultItems.length; i++) {
                var route = vm.results.searchResultItems[i];
                var obj;
                if (route.type == 'DeliveryPoint') {
                    obj = { 'displayText': route.displayText, 'UDPRN': route.udprn,'type':"DeliveryPoint" }
                    arrDeliverypoints.push(obj);
                }
                else if (route.type == 'Postcode') {
                    obj = { 'displayText': route.displayText }
                    arrPostCodes.push(obj);
                }
                else if (route.type == 'StreetNetwork') {
                    obj = { 'displayText': route.displayText }
                    arrStreetNames.push(obj);
                }
                else if (route.type == 'Route') {
                    obj = { 'displayText': route.displayText }
                    arrDeliveryRoutes.push(obj);
                }
            }
            var deliveryPointObj = { 'type': 'DeliveryPoint', 'name': arrDeliverypoints };
            var postCodeObj = { 'type': 'PostCode', 'name': arrPostCodes };
            var streetnameObj = { 'type': 'StreetNetwork', 'name': arrStreetNames };
            var deliveryRouteobj = { 'type': 'Route', 'name': arrDeliveryRoutes };


            if (arrDeliverypoints.length > 0) {
                vm.arrRoutes.push(deliveryPointObj);
            }
            if (arrPostCodes.length > 0) {
                vm.arrRoutes.push(postCodeObj);
            }
            if (arrStreetNames.length > 0) {
                vm.arrRoutes.push(streetnameObj);
            }
            if (arrDeliveryRoutes.length > 0) {
                vm.arrRoutes.push(deliveryRouteobj);
            }

        });
    }

    function selectAll() {
        vm.chkRoute = true;
        vm.chkPostCode = true;
        vm.chkDeliveryPoint = true;
        vm.chkStreetNetwork = true;
    }

    function deSelectAll() {
        vm.chkRoute = false;
        vm.chkPostCode = false;
        vm.chkDeliveryPoint = false;
        vm.chkStreetNetwork = false;
    }
    $scope.routes = [
                       {

                           "type": "Route1", "open": false,
                           "name": [
                             { "type_name": "Route 1" },
                             { "type_name": "Route 2" }]

                       },
                       {

                           "type": "Route2", "open": false,
                           "name": [
                             { "type_name": "Route 10" },
                             { "type_name": "Route 20" },
                             { "type_name": "Route 30" }]

                       }
                       ,
                       {

                           "type": "Route3", "open": false,
                           "name": [
                             { "type_name": "Route 100" },
                             { "type_name": "Route 200" },
                             { "type_name": "Route 300" },
                             { "type_name": "Route 400" }]

                       },
                        {

                            "type": "Route4", "open": false,
                            "name": [
                              { "type_name": "Route 100" },
                              { "type_name": "Route 200" },
                              { "type_name": "Route 300" },
                              { "type_name": "Route 400" }]

                        }
    ]

    $scope.toggleList = function (state) {
        debugger;
        $scope.routes.forEach(function (e) {
            e.show = state;
        });
    }

    function OnChangeItem(selectedItem) {
        debugger;

        if (selectedItem.type === "DeliveryPoint") {
           
            $state.go('searchDetails', { selectedItem: selectedItem});
         //   $mdDialog.cancel(selectedItem.displaytext);
            //mapFactory.getShapeAsync('http://localhost:34583/api/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + selectedItem.UDPRN)
            //    .then(function (response) {
            //      //  var data = response.data;
            //        debugger;
                   // $state.go('searchDetails', { selectedItem: selectedItem.displaytext });

                    //var vectorSource = new ol.source.Vector({});
                    //var map = new ol.Map({
                    //    layers: [

                    //        new ol.layer.Vector({
                    //            source: vectorSource
                    //        })
                    //    ],
                    //    target: 'map',
                    //    view: new ol.View({
                    //        center: [-11000000, 4600000],
                    //        zoom: 4
                    //    })
                    //});
                    //var thing = new ol.geom.Polygon(ol.proj.transform([-16, -22], 'EPSG:4326', 'EPSG:3857'),
                    //                                ol.proj.transform([-44, -55], 'EPSG:4326', 'EPSG:3857'),
                    //                                ol.proj.transform([-88, 75], 'EPSG:4326', 'EPSG:3857'));

                    //var featurething = new ol.Feature({
                    //    name: "Thing",
                    //    geometry: thing
                    //});

                    //vectorSource.addFeature(featurething);
               // });
            //debugger;
          //  $state.go('searchDetails', { selectedItem: selectedItem.displaytext });
           // $mdDialog.close(selectedItem.displaytext);
        }
        $mdDialog.cancel();
    }

    function advanceSearch() {
        var advaceSearchTemplate = advanceSearchService.advanceSearch();
        vm.openAdvanceSearchPopup(advaceSearchTemplate);
        ///vm.openModalPopup("Test");
    }

    function openAdvanceSearchPopup(modalSetting) {
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting)
    };
}