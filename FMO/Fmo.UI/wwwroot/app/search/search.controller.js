'use strict';
angular.module('search')
   .controller('SearchController', SearchController);

function SearchController(searchApiService, $scope, $state, mapFactory, mapStylesFactory, advanceSearchService, $mdDialog, $stateParams) {
    var self = this;

    self.resultSet = resultSet;
    self.presEnter = presEnter;
    self.OnChangeItem = OnChangeItem;
    self.advanceSearch = advanceSearch;
    self.openModalPopup = openModalPopup;

    function querySearch(query) {
        searchApiService.basicSearch(query).then(function (response) {
            self.resultscount = response.data.searchCounts;
            self.results = response.data.searchResultItems
            $state.go('searchDetails', { selectedItem: self.results });
        });
    }

    function resultSet(query) {
        if (query.length >= 3) {
            querySearch(query);
        }
        else {
            self.results = {};
        }
    }

    function presEnter(searchText) {
        if (searchText.length > 3) {
            if (self.results.length === 1) {
                OnChangeItem(self.results);
            }
        }
        else {
            self.results = [{ displayText: "At least three characters must be input for a Search", type: "Warning" }];
        }
    }

    function OnChangeItem(selectedItem) {
        if (selectedItem.type === "DeliveryPoint") {
            mapFactory.getShapeAsync('http://localhost:34583/api/deliveryPoints/GetDeliveryPointByUDPRN?udprn=' + selectedItem.udprn)
                .then(function (response) {
                    var data = response.data;

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
            });
            $state.go('searchDetails', { selectedItem: selectedItem });
        }
    }

    function advanceSearch(query) {
        debugger;
        $stateParams.data = query;
        var state = $stateParams;
        var advaceSearchTemplate = advanceSearchService.advanceSearch(query);
        self.openModalPopup(advaceSearchTemplate);
      //  vm.openModalPopup("Test");
    }

    function openModalPopup(modalSetting) {
        //var state = $stateParams;
        //var setting = routeLogService.routeLog(selectedUnit);
        //debugger;
        var popupSetting = modalSetting;
        $mdDialog.show(popupSetting);
      
    };

    
}
