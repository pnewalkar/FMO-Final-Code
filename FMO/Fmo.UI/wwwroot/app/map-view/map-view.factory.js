'use strict';

angular.module('mapView')
.factory('mapFactory',MapFactory);

MapFactory.$inject = ['$http', 'mapStylesFactory', '$rootScope', 'GlobalSettings','$document'];
function MapFactory($http, mapStylesFactory, $rootScope, GlobalSettings, $document) {
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

    var defaultResolutions = [700.0014000028002, 336.0006720013441, 168.00033600067206, 84.00016800033603, 39.20007840015681, 19.600039200078406, 9.800019600039203, 5.600011200022402, 2.800005600011201, 2.240004480008961, 1.1200022400044805, 0.5600011200022402, 0.2800005600011201, 0.14000028000056006, 0.05600011200022402, 0.02800005600011201];
    var definedScales = [2500000, 1200000, 600000, 300000, 140000, 70000, 35000, 20000, 10000, 8000, 4000, 2000, 1000, 500, 200, 100];
    var zoomLimitReached = false;
    var defaultZoomScale = 2000;

    var availableResolutionForCurrentExtent = [];
    var maxScale = null;
    var BNGProjection = 'EPSG:27700';

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
        defaultResolutions: defaultResolutions,
        definedScales: definedScales,
        getResolutionFromScale: getResolutionFromScale,
        getScaleFromResolution: getScaleFromResolution,
        setUnitBoundaries: setUnitBoundaries,
        setDeliveryPoint: setDeliveryPoint,
        setAccessLink : setAccessLink,
        setMapScale: setMapScale,
        locateDeliveryPoint: locateDeliveryPoint
    };

    function initialiseMap() {
        availableResolutionForCurrentExtent = defaultResolutions;
        view = new ol.View({
            projection: BNGProjection,
            center: [400000, 650000],
            resolutions: defaultResolutions,
            resolution: defaultResolutions[11]
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
            controls: []
        });

        map.addControl(getCustomScaleLine());

        var external_control = new ol.control.Zoom({
            target: $document[0].getElementById('zoom-control')
        });
        map.addControl(external_control);

        units = map.getView().getProjection().getUnits();
        mpu = ol.proj.METERS_PER_UNIT[units];
    }

    function initialiseMiniMap() {
        if (view == null)
            initialiseMap();

        viewMiniMap = new ol.View({
            projection: BNGProjection,
            center: view.getCenter(),
            zoom: view.getZoom() - 2
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
        viewMiniMap.setZoom(view.getZoom() - 2);
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
        layerObj.selectorVisible = layerObj.selectorVisible == angular.isUndefined(undefined)  ? true : layerObj.selectorVisible;

        layerObj.layer.set('name', layerObj.layerName);
        if (angular.isDefined(layerObj.layer.setZIndex))
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
    function setZoomButtonStatus(buttonList, status) {
        for (var i = 0; i < buttonList.length; i++) {
            buttonList[i].disabled = status;

            if (status) {
                buttonList[i].style.opacity = "0.65";
                buttonList[i].style.cursor = "not-allowed";
            }
            else {
                buttonList[i].style.opacity = "1";
                buttonList[i].style.cursor = "pointer";
            }
        }
    }

    function setMapScale(scale) {
        var resolution = getResolutionFromScale(scale)
        map.getView().setResolution(resolution);
        $rootScope.$broadcast('zommLevelchanged', { zoomLimitReached: false, currentScale: scale, maximumScale: maxScale });
    }

    function getCustomScaleLine() {
        customScaleLine = function (opt_options) {
            var options = opt_options ? opt_options : {};
            var className = options.className !== angular.isUndefined(undefined)  ? options.className : 'ol-scale-line';
            this.element_ = $document[0].createElement('DIV');
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
            zoomLimitReached = false;
            var resolution = map.getView().getResolution();
            var scale = Math.round(getScaleFromResolution(resolution));
            var index = definedScales.indexOf(scale);
            var maxScaleIndex = definedScales.indexOf(maxScale);
            if (index > -1) {
                var zoomInButtons = $document[0].getElementsByClassName("ol-zoom-in");
                var zoomOutButtons = $document[0].getElementsByClassName("ol-zoom-out");

                if (index == definedScales.length - 1) {
                    setZoomButtonStatus(zoomInButtons, true);

                    zoomLimitReached = true;
                }
                else {
                    setZoomButtonStatus(zoomInButtons, false);
                }

                if (index == 0 || index == maxScaleIndex) {
                    setZoomButtonStatus(zoomOutButtons, true);

                    zoomLimitReached = true;
                }
                else {
                    setZoomButtonStatus(zoomOutButtons, false);
                }

                $rootScope.$apply($rootScope.$broadcast('zommLevelchanged', { zoomLimitReached: zoomLimitReached, currentScale: scale, maximumScale: maxScale }));
            }
        };

        ol.inherits(customScaleLine, ol.control.ScaleLine);

        return new customScaleLine({
            render: customScaleLine.render,
            className: 'zoom-scale',
            target: $document[0].getElementById('zoom-scale')
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

    function setUnitBoundaries(bbox, center, unitBoundaryGeoJSONData) {
        view = new ol.View({
            projection: BNGProjection,
            center: [400000, 650000],
            resolutions: defaultResolutions,
            resolution: defaultResolutions[11]
        });

        map.setView(view);

        map.getView().fit(bbox, map.getSize());

        var maxResolution = map.getView().getResolution();

        availableResolutionForCurrentExtent = [];
        for (var i = 0; i < defaultResolutions.length; i++) {
            if (maxResolution > defaultResolutions[i]) {
                availableResolutionForCurrentExtent.push(defaultResolutions[i]);
            }
        }

        maxScale = Math.round(getScaleFromResolution(availableResolutionForCurrentExtent[0]));
        view = new ol.View({
            projection: BNGProjection,
            center: center,
            extent: bbox,
            resolutions: availableResolutionForCurrentExtent,
            resolution: getResolutionFromScale(defaultZoomScale)
        });

        view.on('change:resolution', updateMiniMap);
        view.on('change:center', updateMiniMap);

        map.setView(view);

        var unitBoundaryLayer = getLayer(GlobalSettings.unitBoundaryLayerName);

        unitBoundaryLayer.layer.getSource().clear();

        unitBoundaryLayer.layer.getSource().addFeatures((new ol.format.GeoJSON({ defaultDataProjection: BNGProjection })).readFeatures(unitBoundaryGeoJSONData));
    }

    function setDeliveryPoint(long, lat) {
        debugger;
        map.getView().setCenter([long, lat]);
        map.getView().setResolution(0.5600011200022402);
        var deliveryPointsLayer = getLayer(GlobalSettings.deliveryPointLayerName);
        deliveryPointsLayer.layer.getSource().clear();
        deliveryPointsLayer.selected = true;
        deliveryPointsLayer.layer.setVisible(true)
        var authData = angular.fromJson(sessionStorage.getItem('authorizationData'));
        $http({
            method: 'GET',
            url: GlobalSettings.apiUrl + '/deliveryPoints/GetDeliveryPoints?boundaryBox=' + map.getView().calculateExtent(map.getSize()).join(','),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': 'Bearer ' + authData.token
            }
        }).success(function (response) {          
        
            var features = new ol.format.GeoJSON({ defaultDataProjection: BNGProjection }).readFeatures(response);
            deliveryPointsLayer.layer.getSource().addFeatures(features);
           
            var style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE);

            var select = new ol.interaction.Select({ style: style });
            
            map.addInteraction(select);

            var selectedFeatures = select.getFeatures();

            var featureToSelect;
            angular.forEach(features, function (feature, index) {              
                var featureLatitude = feature.values_.geometry.getCoordinates()[1];
                var featureLongitude = feature.values_.geometry.getCoordinates()[0];

                if (featureLatitude ===lat && featureLongitude===long) {
                    featureToSelect = feature;
                }
            });      

            if (featureToSelect)
                selectedFeatures.push(featureToSelect);
        });
      
    }

    function locateDeliveryPoint(long, lat) {
        map.getView().setCenter([long, lat]);
        map.getView().setResolution(0.5600011200022402);

        var point_feature = new ol.Feature({});

        var point_geom = new ol.geom.Point([long, lat]);
        point_feature.setGeometry(point_geom);

        var vector_layer = new ol.layer.Vector({
            source: new ol.source.Vector({
                features: [point_feature]
            })
        });
        vector_layer.setStyle(mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE)("deliverypoint"));
        map.addLayer(vector_layer);
    }

    function setAccessLink() {
        debugger;
        
        var accessLinkLayer = getLayer(GlobalSettings.accessLinkLayerName);
        accessLinkLayer.layer.getSource().clear();
        accessLinkLayer.selected = true;
        accessLinkLayer.layer.setVisible(true)
        var authData = angular.fromJson(sessionStorage.getItem('authorizationData'));
        $http({
            method: 'GET',
            url: GlobalSettings.apiUrl + '/accessLink/GetAccessLinks?boundaryBox=' + map.getView().calculateExtent(map.getSize()).join(','),
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'Authorization': 'Bearer ' + authData.token
            }
        }).success(function (response) {
            debugger;
            var features = new ol.format.GeoJSON({ defaultDataProjection: BNGProjection }).readFeatures(response);
            accessLinkLayer.layer.getSource().addFeatures(features);

            var style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE);

            var select = new ol.interaction.Select({ style: style });

            map.addInteraction(select);

            var selectedFeatures = select.getFeatures();
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
        if (this.style != null && angular.isDefined(this.layer.setStyle)) {
            this.layer.setStyle(this.style);
        }
    }
}