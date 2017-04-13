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
    var customScaleLine = null;

    var layers = [];

    var units = null;
    var dpi = 25.4 / 0.28;
    var mpu = null;

    var definedResolutions = [700.0014000028002, 336.0006720013441, 168.00033600067206, 84.00016800033603, 39.20007840015681, 19.600039200078406, 9.800019600039203, 5.600011200022402, 2.800005600011201, 2.240004480008961, 1.1200022400044805, 0.5600011200022402, 0.2800005600011201, 0.14000028000056006, 0.05600011200022402, 0.02800005600011201];
    var definedScales = [2500000, 1200000, 600000, 300000, 140000, 70000, 35000, 20000, 10000, 8000, 4000, 2000, 1000, 500, 200, 100];

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
        deleteAllFeaturesFromLayer: deleteAllFeaturesFromLayer,
        addFeaturesToMap: addFeaturesToMap,
        definedResolutions: definedResolutions,
        definedScales: definedScales,
        getResolutionFromScale: getResolutionFromScale,
        getScaleFromResolution: getScaleFromResolution
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
            // zoom: 4,
            resolutions: definedResolutions,
            resolution: definedResolutions[11]
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
                getCustomScaleLine()               
            ])
        });
        var external_control = new ol.control.Zoom({
            target: document.getElementById('zoom-control')
        });
        map.addControl(external_control);

        units = map.getView().getProjection().getUnits();
        mpu = ol.proj.METERS_PER_UNIT[units];
    }

    function initialiseMiniMap() {
        if (view == null)
            initialiseMap();

        viewMiniMap = new ol.View({
            projection: 'EPSG:27700',
            center: view.getCenter(),
            zoom: view.getZoom() - 5
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

    function updateMiniMap() {
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
        angular.forEach(layers, function (layer, index) {
            if (layer.layerName === layerName) {
                actualLayer = layer;
            }
        });

        return actualLayer;
    }

    function removeLayer(layerName) {
        angular.forEach(layers, function (layer, index) {
            if (layer.layerName === layerName) {
                map.removeLayer(layer.layer);
                delete layers[index];
            }
        });
    }

    function deleteAllFeaturesFromLayer(layerName) {
        angular.forEach(layers, function (layer, index) {
            if (layer.layerName === layerName) {
                layer.layer.getSource().clear();
            }
        });
    }

    function getLayerDataFromUrl(url) {
        var shapePromise = getShapeAsync(url);
        return shapePromise
            .then(function (result) {
                return createLayerFromFeatures(convertGeoJsonToOl(result.data));
            });
    }

    function convertGeoJsonToOl(featureData, formatOptions) {
        var format = new ol.format.GeoJSON();

        //if (!formatOptions) {
        //    formatOptions = {
        //        featureProjection: map.getView().getProjection()
        //    };
        //}

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
        angular.forEach(layers, function (layer, index) {
            if (layer.layerName === layerName) {
                actualLayer = layer;
            }
        });

        return actualLayer;
    }

    function deleteAllFeaturesFromLayer(layerName) {
        angular.forEach(layers, function (layer, index) {
            if (layer.layerName === layerName) {
                layer.layer.getSource().clear();
            }
        });
    }

    function getLayerDataFromUrl(url) {
        var shapePromise = getShapeAsync(url);
        return shapePromise
            .then(function (result) {
                return createLayerFromFeatures(convertGeoJsonToOl(result.data));
            });
    }

    function convertGeoJsonToOl(featureData, formatOptions) {
        var format = new ol.format.GeoJSON();

        //if (!formatOptions) {
        //    formatOptions = {
        //        featureProjection: map.getView().getProjection()
        //    };
        //}

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

    function getCustomScaleLine() {
        customScaleLine = function (opt_options) {
            var options = opt_options ? opt_options : {};
            var className = options.className !== undefined ? options.className : 'ol-scale-line';
            this.element_ = document.createElement('DIV');
            this.renderedVisible_ = false;
            this.viewState_ = null;
            this.renderedHTML_ = '';
            var render = options.render ? options.render : ol.control.ScaleLine.render;
            ol.control.Control.call(this, {
                element: this.element_,
                render: render,
                target: options.target
            });
        };

        customScaleLine.render = function (mapEvent) {

            var resolution = map.getView().getResolution();
            var scale = Math.round(getScaleFromResolution(resolution));
            if (definedScales.indexOf(scale) > -1) {
                var html = 1 + ': ' + scale;

                if (this.renderedHTML_ != html) {
                    this.element_.innerHTML = html;
                    this.renderedHTML_ = html;
                }

                if (!this.renderedVisible_) {
                    this.element_.style.display = '';
                    this.renderedVisible_ = true;
                }
            }
        };

        ol.inherits(customScaleLine, ol.control.ScaleLine);

        return new customScaleLine({
            render: customScaleLine.render,
            className: 'zoom-scale',
            target: document.getElementById('zoom-scale')
        });
    }

    function getResolutionFromScale(scale) {
        var resolution = scale / (mpu * 39.37 * dpi);
        return resolution;
    }

    function getScaleFromResolution(resolution) {
        var scale = resolution * (mpu * 39.37 * dpi);
        return scale;
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