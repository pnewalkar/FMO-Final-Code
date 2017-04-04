'use strict';

angular.module('mapView')
.factory('mapFactory', ['$http', 'mapStylesFactory', '$rootScope', MapFactory]);


function MapFactory($http, mapStylesFactory, $rootScope) {
    var map = null;
    var miniMap = null;
    var view = null;
    var vectorLayer = null;
    var vectorSource = null;
    var viewMiniMap = null;

    var layers = [];

    return {
        initialiseMap: initialiseMap,
        getMap: getMap,
        initialiseMiniMap: initialiseMiniMap,
        getShapeAsync: getShapeAsync,
        getMiniMap: getMiniMap,
        getVectorLayer: getVectorLayer,
        getVectorSource: getVectorSource,
        getAllLayers: getAllLayers,
        addLayer: addLayer,
        removeLayer: removeLayer,
        getLayer: getLayer,
        createLayerAsync: createLayerAsync,
        convertGeoJsonToOl: convertGeoJsonToOl,
        deleteAllFeaturesFromLayer:deleteAllFeaturesFromLayer,
        addFeaturesToMap:addFeaturesToMap
    };

    function initialiseMap() {
        //view = new ol.View({
        //    center: ol.proj.fromLonLat([
        //        -0.45419810184716686,
        //        50.83910301753454
        //    ]),
        //    zoom: 17,
        //    minZoom: 5,
        //    maxZoom: 19
        //});

        view = new ol.View({
            projection: 'EPSG:27700',
            center: [400000, 650000],
            zoom: 4
        });


        vectorSource = new ol.source.Vector({
            wrapX: false
        });

        vectorLayer = new ol.layer.Vector({
            source: vectorSource,
            style: mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE),
            renderBuffer: 1000
        });

        map = new ol.Map({
            layers: layers.map(function (a) { return a.layer }),
            target: 'map',
            view: view,
            loadTilesWhileAnimating: true,
            loadTilesWhileInteracting: true,
            controls: ol.control.defaults().extend([
                new ol.control.ScaleLine()
            ])
        });
        var external_control = new ol.control.Zoom({
            target: document.getElementById('zoom-control')
        });
        map.addControl(external_control);
    }

    function initialiseMiniMap() {
        if (view == null)
            initialiseMap();

        viewMiniMap = new ol.View({
            center: view.getCenter(),
            zoom: 12
        });

        miniMap = new ol.Map({
            layers: layers.filter(function (l) { return l.onMiniMap; }).map(function (a) { return a.layer }),
            interactions: [],
            controls: [],
            loadTilesWhileAnimating: true,
            loadTilesWhileInteracting: true,
            target: 'mini-map',
            view: viewMiniMap
        });

        view.on('change:resolution', updateMiniMap);
        view.on('change:center', updateMiniMap);
    }

    function updateMiniMap(){
        viewMiniMap.setCenter(view.getCenter());
        viewMiniMap.setZoom(view.getZoom() - 5);
    }

    function getVectorLayer() {
        return vectorLayer;
    }

    function getVectorSource() {
        return vectorSource;
    }

    function getAllLayers() {
        return layers;
    }

    function getMap() {
        return map;
    }

    function getMiniMap() {
        return miniMap;
    }

    function getLayer(layerName) {
        var actualLayer;
        angular.forEach(layers, function(layer, index) {
            if(layer.layerName === layerName) {
                actualLayer = layer;
            }
        });

        return actualLayer;
    }

    function removeLayer(layerName) {
        angular.forEach(layers, function(layer, index) {
            if(layer.layerName === layerName) {
                map.removeLayer(layer.layer);
                delete layers[index];
            }
        });
    }

    function deleteAllFeaturesFromLayer(layerName) {
        angular.forEach(layers, function(layer, index) {
            if(layer.layerName === layerName) {
                layer.layer.getSource().clear();
            }
        });
    }

    function getLayerDataFromUrl(url){
        var shapePromise = getShapeAsync(url);
        return shapePromise
            .then(function(result){
                return createLayerFromFeatures(convertGeoJsonToOl(result.data));
            });
    }

    function convertGeoJsonToOl(featureData, formatOptions) {
        var format = new ol.format.GeoJSON();

        if(!formatOptions) {
            formatOptions = {
                featureProjection: map.getView().getProjection()
            };
        }

        return format.readFeatures(featureData, formatOptions);
    }

    function createLayerFromFeatures(features) {
        return new ol.layer.Vector({
            source: new ol.source.Vector({
                features: features
            }),
            renderBuffer: 1000
        });
    }

    function getLayer(layerName) {
        var actualLayer;
        angular.forEach(layers, function(layer, index) {
            if(layer.layerName === layerName) {
                actualLayer = layer;
            }
        });

        return actualLayer;
    }

    function deleteAllFeaturesFromLayer(layerName) {
        angular.forEach(layers, function(layer, index) {
            if(layer.layerName === layerName) {
                layer.layer.getSource().clear();
            }
        });
    }

    function getLayerDataFromUrl(url){
        var shapePromise = getShapeAsync(url);
        return shapePromise
            .then(function(result){
                return createLayerFromFeatures(convertGeoJsonToOl(result.data));
            });
    }

    function convertGeoJsonToOl(featureData, formatOptions) {
        var format = new ol.format.GeoJSON();

        if(!formatOptions) {
            formatOptions = {
                featureProjection: map.getView().getProjection()
            };
        }

        return format.readFeatures(featureData, formatOptions);
    }

    function createLayerFromFeatures(features) {
        return new ol.layer.Vector({
            source: new ol.source.Vector({
                features: features
            }),
            renderBuffer: 1000
        });
    }

    function addFeaturesToMap(layerName, layerGroup, features, keys, selectorVisible) {
        var layer = createLayerFromFeatures(features);
        layer.setStyle(mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE));
        var layerSelector = new MapFactory.LayerSelector();
        layerSelector.layerName = layerName;
        layerSelector.layer = layer;
        layerSelector.group = layerGroup;
        layerSelector.selected = true;
        layerSelector.onMiniMap = true;
        layerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        layerSelector.keys = keys;
        layerSelector.selectorVisible = selectorVisible;
        addLayer(layerSelector);
    }

    function createLayerAsync(paramObj) {
        var layerPromise = getLayerDataFromUrl(paramObj.layerUrl);
        return layerPromise.then(function (layer) {
            paramObj.layer = layer;
            return addLayer(paramObj);
        });
    }

    function addLayer(layerObj) {
        layerObj.selected = layerObj.selected ? true : false;
        layerObj.disabled = layerObj.disabled ? true : false;
        layerObj.onMiniMap = layerObj.onMiniMap ? true : false;
        layerObj.keys = layerObj.keys ? layerObj.keys : [];
        layerObj.selectorVisible = layerObj.selectorVisible == undefined ? true : layerObj.selectorVisible;

        layerObj.layer.set('name', layerObj.layerName);
        if (layerObj.layer.setZIndex != undefined)
            layerObj.layer.setZIndex(layerObj.zIndex * 100);

        layerObj.restyle();

        map.addLayer(layerObj.layer);
        if (miniMap && layerObj.onMiniMap)
            miniMap.addLayer(layerObj.layer);
        layers.push(layerObj);
        $rootScope.$broadcast("updateLayerControl");

        return layerObj;
    }

    function getShapeAsync(url) {
        return $http.get(url).success(function (result) {
            return result;
        });
    }
}


var currentZIndex = 0;

MapFactory.LayerSelector = function () {
    this.layerName = "";
    this.layer = null;
    this.layerUrl = null;
    this.group = "";
    this.selected = false;
    this.onMiniMap = false;
    this.zIndex = currentZIndex++;
    this.keys = [];
    this.selectorVisible = true;
    this.style = null;

    this.restyle = function () {
        if (this.style != null && this.layer.setStyle != undefined) {
            this.layer.setStyle(this.style);
        }
    }
}