angular.module('mapView')
        .factory('mapService', mapService);

mapService.$inject = ['$http',
                     'mapFactory',
                     'mapStylesFactory',
                     '$timeout',
                     'GlobalSettings',
                     'coordinatesService',
                     'roadLinkGuidService',
                     'accessLinkCoordinatesService',
                     'intersectionPointService',
                     'accessLinkAPIService',
                     'guidService',
                     '$document',
                     '$state',
                     '$stateParams',
                     '$rootScope',
                     'layersAPIService',
                     'CommonConstants'
];

function mapService($http,
                    mapFactory,
                    mapStylesFactory,
                    $timeout,
                    GlobalSettings,
                    coordinatesService,
                    roadLinkGuidService,
                    accessLinkCoordinatesService,
                    intersectionPointService,
                    accessLinkAPIService,
                    guidService,
                    $document,
                    $state,
                    $stateParams,
                    $rootScope,
                    layersAPIService,
                    CommonConstants
                   ) {
    var vm = this;
    vm.map = null;
    $rootScope.state = true;
    vm.miniMap = null;
    vm.activeTool = "";
    vm.focusedLayers = [];
    vm.mapButtons = ["select", "point", "line", "accesslink", "area", "modify", "delete"];
    vm.interactions = {
    };
    vm.layersForContext = [];
    vm.activeSelection = null;
    vm.secondarySelections = [];
    vm.routeName = "";
    vm.opacity = null;
    vm.selectionListeners = [];
    vm.features = null;
    vm.selectedDP = null;
    vm.selectedLayer = null;
    vm.isSelected = false;
    vm.layerName = "";
    vm.onDeleteButton = function (featureId, layer) { };
    vm.onModify = function (feature) { };
    vm.onDrawEnd = function (buttonName, feature) { };
    vm.pointerMoveHandler = function (evt) {
        if (evt.dragging || vm.activeTool != 'measure') {
            return;
        }

        var tooltipCoord = evt.coordinate;

        if (vm.sketch) {
            var output;
            var geom = (vm.sketch.getGeometry());
            if (geom instanceof ol.geom.LineString) {
                output = formatLength((geom));
                tooltipCoord = geom.getLastCoordinate();
            }
            vm.measureTooltipElement.innerHTML = output;
            vm.measureTooltip.setPosition(tooltipCoord);
        }
    };
    return {
        initialise: initialise,
        initialiseMiniMap: initialiseMiniMap,
        getMapButtons: getMapButtons,
        mapLayers: mapLayers,
        getDotStyle: getDotStyle,
        deleteSelectedFeature: deleteSelectedFeature,
        getLayer: getLayer,
        addSelectionListener: addSelectionListener,
        removeSelectionListener: removeSelectionListener,
        getActiveSelection: getActiveSelection,
        getSecondarySelections: getSecondarySelections,
        addInteractions: addInteractions,
        setDrawButton: setDrawButton,
        removeCurrentInteractions: removeCurrentInteractions,
        syncMinimapAnimation: syncMinimapAnimation,
        oncollapse: oncollapse,
        mapToolChange: mapToolChange,
        refreshLayers: refreshLayers,
        getActiveFeature: getActiveFeature,
        setSelections: setSelections,
        getfeature: getfeature,
        selectFeatures: selectFeatures,
        getSecondaryFeatures: getSecondaryFeatures,
        setSelectedObjectsVisibility: setSelectedObjectsVisibility,
        removeInteraction: removeInteraction,
        deleteAccessLinkFeature: deleteAccessLinkFeature,
        showDeliveryPointDetails: showDeliveryPointDetails,
        clearDrawingLayer: clearDrawingLayer,
        setSize: setSize,
        composeMap: composeMap,
        getResolution: getResolution,
        setOriginalSize: setOriginalSize,
        LicenceInfo: LicenceInfo,
        baseLayerLicensing: baseLayerLicensing,
        GetPolygonTransparency: GetPolygonTransparency
    }

    function LicenceInfo(displayText) {
        return mapFactory.LicenceInfo(displayText);
    }

    function GetPolygonTransparency() {
        mapFactory.GetPolygonTransparency().then(function (response) {
            vm.opacity = parseFloat(response[0].value);
        });
    }

    function initialise() {
        proj4.defs('EPSG:27700', '+proj=tmerc +lat_0=49 +lon_0=-2 +k=0.9996012717 ' +
       '+x_0=400000 +y_0=-100000 +ellps=airy ' +
       '+towgs84=446.448,-125.157,542.06,0.15,0.247,0.842,-20.489 ' +
       '+units=m +no_defs');

        var proj27700 = ol.proj.get('EPSG:27700');
        proj27700.setExtent([0, 0, 700000, 1300000]);

        mapFactory.initialiseMap();
        vm.map = mapFactory.getMap();
        vm.originalSize = vm.map.getSize();
        vm.GetPolygonTransparency();

        var digitalGlobeTiles = new ol.layer.Tile({
            title: 'DigitalGlobe Maps API: Recent Imagery',
            source: new ol.source.XYZ({
                url: GlobalSettings.vectorMapUrl,
                attribution: "© DigitalGlobe, Inc"
            })
        });

        var bingMapsRoadTiles = new ol.layer.Tile({
            title: 'Bing maps API',
            source: new ol.source.BingMaps({
                key: GlobalSettings.bingMapKey,
                imagerySet: 'Road',
                maxZoom: 19
            })
        });


        var loadFeatures = function (layerSource, response) {
            var features = new ol.format.GeoJSON({
                defaultDataProjection: 'EPSG:27700'
            }).readFeatures(response);
            layerSource.addFeatures(features);
        };

        var deliveryPointsVector = new ol.source.Vector({
            format: new ol.format.GeoJSON({
                defaultDataProjection: 'EPSG:27700'
            }),
            strategy: ol.loadingstrategy.bbox,
            loader: function (extent) {
                var authData = angular.fromJson(sessionStorage.getItem('authorizationData'));
                layersAPIService.fetchDeliveryPoints(extent, authData).then(function (response) {
                    var layerName = GlobalSettings.deliveryPointLayerName;
                    mapFactory.LicenceInfo("", layerName, deliveryPointsVector);
                });
            }
        });

        var deliveryPointsLayer = new ol.layer.Vector({
            source: deliveryPointsVector,
            minResolution: 0,
            maxResolution: 2.1002842005684017
        });

        var mockGroupsVector = new ol.source.Vector({
            format: new ol.format.GeoJSON(),
            url: function (extent) {
                return 'http://localhost:47467/home/getgroupsdata?bbox=' + extent.join(',');
            },
            strategy: ol.loadingstrategy.bbox
        });

        var accessLinkVector = new ol.source.Vector({
            format: new ol.format.GeoJSON({
                defaultDataProjection: 'EPSG:27700'
            }),
            strategy: ol.loadingstrategy.bbox,
            loader: function (extent) {
                var authData = angular.fromJson(sessionStorage.getItem('authorizationData'));
                layersAPIService.fetchAccessLinks(extent, authData).then(function (response) {
                    var layerName = GlobalSettings.accessLinkLayerName;
                    mapFactory.LicenceInfo("", layerName, accessLinkVector);
                });
            }
        });

        var roadLinkVector = new ol.source.Vector({
            format: new ol.format.GeoJSON({
                defaultDataProjection: 'EPSG:27700'
            }),
            strategy: ol.loadingstrategy.bbox,
            loader: function (extent) {
                var authData = angular.fromJson(sessionStorage.getItem('authorizationData'));
                layersAPIService.fetchRouteLinks(extent, authData).then(function (response) {
                    var layerName = GlobalSettings.roadLinkLayerName;
                    mapFactory.LicenceInfo("", layerName, roadLinkVector);
                });
            }
        });


        var unitBoundaryVector = new ol.source.Vector({
            format: new ol.format.GeoJSON({
                defaultDataProjection: 'EPSG:27700'

            }),
            loader: function (extent) {
                var layerName = GlobalSettings.unitBoundaryLayerName;
                mapFactory.LicenceInfo("", layerName, unitBoundaryVector);
            }
        });

        var mockGroupsLayer = new ol.layer.Vector({
            source: mockGroupsVector
        });

        var accessLinkLayer = new ol.layer.Vector({
            source: accessLinkVector,
            minResolution: 0,
            maxResolution: 2.1002842005684017
        });

        var roadLinkLayer = new ol.layer.Vector({
            source: roadLinkVector,
            minResolution: 0,
            maxResolution: 3.9202878405756825
        });

        var unitBoundaryLayer = new ol.layer.Vector({
            source: unitBoundaryVector
        });

        var roadsSelector = new MapFactory.LayerSelector();
        roadsSelector.layerName = "Base Layer";
        roadsSelector.layer = bingMapsRoadTiles;
        roadsSelector.group = "Base Map";
        roadsSelector.selected = true;
        roadsSelector.onMiniMap = true;
        var roadsLayer = mapFactory.addLayer(roadsSelector);

        var drawingLayerSelector = new MapFactory.LayerSelector();
        drawingLayerSelector.layerName = "Drawing";
        drawingLayerSelector.layer = mapFactory.getVectorLayer();
        drawingLayerSelector.group = "";
        drawingLayerSelector.selected = true;
        drawingLayerSelector.selectorVisible = false;
        vm.drawingLayer = mapFactory.addLayer(drawingLayerSelector);

        var deliveryPointsLayerSelector = new MapFactory.LayerSelector();
        deliveryPointsLayerSelector.layerName = GlobalSettings.deliveryPointLayerName;
        deliveryPointsLayerSelector.layer = deliveryPointsLayer;
        deliveryPointsLayerSelector.group = "";
        deliveryPointsLayerSelector.zIndex = 8;
        deliveryPointsLayerSelector.selected = false;
        deliveryPointsLayerSelector.onMiniMap = false;
        deliveryPointsLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        deliveryPointsLayerSelector.keys = ["deliverypoint"];
        mapFactory.addLayer(deliveryPointsLayerSelector);

        var accessLinksLayerSelector = new MapFactory.LayerSelector();
        accessLinksLayerSelector.layerName = GlobalSettings.accessLinkLayerName,
                    accessLinksLayerSelector.layer = accessLinkLayer;
        accessLinksLayerSelector.group = "";
        accessLinksLayerSelector.zIndex = 7;
        accessLinksLayerSelector.selected = false;
        accessLinksLayerSelector.onMiniMap = false;
        accessLinksLayerSelector.selectorVisible = true;
        accessLinksLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        accessLinksLayerSelector.keys = ["accesslink"];
        mapFactory.addLayer(accessLinksLayerSelector);

        var roadLinkLayerSelector = new MapFactory.LayerSelector();
        roadLinkLayerSelector.layerName = GlobalSettings.roadLinkLayerName;
        roadLinkLayerSelector.layer = roadLinkLayer;
        roadLinkLayerSelector.group = "";
        roadLinkLayerSelector.zIndex = 6;
        roadLinkLayerSelector.selected = false;
        roadLinkLayerSelector.onMiniMap = false;
        roadLinkLayerSelector.selectorVisible = true;
        roadLinkLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        roadLinkLayerSelector.keys = ["roadlink"];
        mapFactory.addLayer(roadLinkLayerSelector);

        var unitBoundaryLayerSelector = new MapFactory.LayerSelector();
        unitBoundaryLayerSelector.layerName = GlobalSettings.unitBoundaryLayerName;
        unitBoundaryLayerSelector.layer = unitBoundaryLayer;
        unitBoundaryLayerSelector.group = "";
        unitBoundaryLayerSelector.zIndex = 1;
        unitBoundaryLayerSelector.selected = false;
        unitBoundaryLayerSelector.onMiniMap = false;
        unitBoundaryLayerSelector.selectorVisible = true;
        unitBoundaryLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        unitBoundaryLayerSelector.keys = ["unitBoundary"];
        mapFactory.addLayer(unitBoundaryLayerSelector);

        roadsLayer.selected = true;
        vm.map.on('pointermove', vm.pointerMoveHandler);
        refreshLayers();

        vm.showRoadPanel = false;
    }

    function baseLayerLicensing() {
        mapFactory.LicenceInfo("Base Layers", "Base Layer", null);
    }
    function initialiseMiniMap() {
        mapFactory.initialiseMiniMap();
        vm.miniMap = mapFactory.getMiniMap();
    }
    function getMapButtons() {
        return vm.mapButtons;
    }
    function mapLayers() {
        return mapFactory.getAllLayers();
    }
    function getLayer(layerName) {
        var returnVal = null;
        mapLayers().forEach(function (layer) {
            if (layer.layerName == layerName)
                returnVal = layer;
        })
        return returnVal;
    }
    function getActiveSelection() {
        return vm.activeSelection;
    }
    function getActiveFeature() {
        if (vm.activeSelection == null)
            return null;
        return vm.activeSelection.layer.getSource().getFeatureById(vm.activeSelection.featureID);
    }
    function getSecondarySelections() {
        return vm.secondarySelections;
    }
    function getSecondaryFeatures() {
        var secondaryFeatures = [];
        vm.secondarySelections.forEach(function (selection) {
            secondaryFeatures.push(selection.layer.getSource().getFeatureById(selection.featureID));
        });
        return secondaryFeatures;
    }
    function setSelections(active, secondary) {
        vm.activeSelection = active;
        vm.secondarySelections = secondary;
        callSelectionListeners();
    }
    function addSelectionListener(callback) {
        vm.selectionListeners.push(callback)
    }
    function removeSelectionListener(callback) {
        var newListeners = [];
        vm.selectionListeners.forEach(function (listener) {
            if (callback != listener) {
                newListeners.push(listener);
            }
        })
        vm.selectionListeners = newListeners;
    }
    function callSelectionListeners() {
        var selectedFeatures = [];
        if (vm.activeSelection != null) {
            selectedFeatures.push(getActiveFeature());
        }
        vm.secondarySelections.forEach(function (selection) {
            selectedFeatures.push(selection.layer.getSource().getFeatureById(selection.featureID));
        });

        vm.selectionListeners.forEach(function (callback) {
            callback(selectedFeatures);
        })
    }
    function refreshLayers() { 
        mapLayers().forEach(function (layer) {
            layer.layer.setVisible(layer.selected);
            layer.layer.changed();          
        });        
        vm.layerSummary = getLayerSummary();
    }
    function deleteAccessLinkFeature(feature) {
        if (vm.drawingLayer.layer.getSource().getFeatures().indexOf(feature) != -1)
            vm.drawingLayer.layer.getSource().removeFeature(feature);
    }
    function getLayerSummary() {
        var summary = "";
        mapLayers().forEach(function (layer) {
            if (layer.selected && layer.selectorVisible)
                summary += layer.layerName + ", ";
        });
        if (summary.lastIndexOf(", ") == summary.length - 2)
            summary = summary.substring(0, summary.length - 2);
        else
            summary = "No layers selected";

        return summary;
    }
    function deleteSelectedFeature() {
        if (vm.layerName == "Drawing") {
            var collection = new ol.Collection();
            collection.push(getActiveFeature());
            collection.forEach(function (feature) {
                if (vm.drawingLayer.layer.getSource().getFeatures().indexOf(feature) != -1)
                    vm.drawingLayer.layer.getSource().removeFeature(feature);
                vm.onDeleteButton(feature.getId(), feature.layer);
            });
            setSelections(null, []);
            $rootScope.$emit('selectedObjectChange', { "isSelected": false });
        }
    }
    function addInteractions() {
        for (var key in vm.interactions) {
            vm.map.addInteraction(vm.interactions[key]);
        }
    }
    function removeCurrentInteractions() {
        for (var key in vm.interactions) {
            vm.map.removeInteraction(vm.interactions[key]);
            delete vm.interactions[key];
        }
    }
    function setDrawButton(button) {
        var style = null;
        style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE)(button.name);
        if (button.name == "area") {
            //style.opacity = 0.4;
            style.opacity = vm.opacity;
        }
        vm.interactions.draw = setDrawInteraction(button, style);
        switch (button.name) {
            case "deliverypoint":
                setupDP();
                break;
            case "accesslink":
                var roadLinklayer = mapFactory.getLayer('Roads');
                snapOnFeature(roadLinklayer);
                setupAccessLink();
            default:
                break;
        }
        var name = button.name;
        if (name == "point") {
            vm.interactions.draw.on('drawend', function (evt) {
                evt.feature.set("type", "deliverypoint");
                evt.feature.setId(createGuid())
            })
        }

        else if (name == "accesslink") {
            vm.interactions.draw.on('drawend', function (evt) {
                evt.feature.set("type", "accesslink");
            })
        }

        else if (name == "line") {
            
            vm.interactions.draw.on('drawstart', enableDrawingLayer, this);
            vm.interactions.draw.on('drawend', function (evt) {
                evt.feature.set("type", "linestring");
                evt.feature.setId(createGuid())
            })
        }

        else if (name == "area") {
            vm.interactions.draw.on('drawstart', enableDrawingLayer, this);
            vm.interactions.draw.on('drawend', function (evt) {
                evt.feature.setId(createGuid());
                evt.feature.set("type", "polygon");
            })
        }
        else {
            vm.interactions.draw.on('drawstart', enableDrawingLayer, this);
        }
    }

    function setDrawInteraction(button, style) {
        var draw = null;

        switch (button.name) {
            case "accesslink":
                var dpCoordinates = coordinatesService.getCordinates();

                function drawCondition(e) {
                    if (dpCoordinates === '') {
                        return false;
                    }
                    else {
                        return true;
                    }
                }
                draw = new ol.interaction.Draw({
                    source: vm.drawingLayer.layer.getSource(),
                    type: button.shape,
                    style: style,
                    condition: drawCondition,
                    geometryFunction: setAccessLinkCoordinates,
                    finishCondition: finishCondition
                });
                break;
            default:
                draw = new ol.interaction.Draw({
                    source: vm.drawingLayer.layer.getSource(),
                    type: button.shape,
                    style: style,
                    condition: ol.events.condition.primaryAction
                });
                break;
        }
        return draw;
    }
    function setAccessLinkCoordinates(coordinates, geometry) {
        if (!geometry) {
            geometry = new ol.geom.LineString(null);
        }

        var dpCoordinates = coordinatesService.getCordinates();
        if (dpCoordinates && dpCoordinates !== '') {
            coordinates[0] = dpCoordinates;
        }
        geometry.setCoordinates(coordinates);
        return geometry;
    }
    function setupAccessLink() {
        var startLineCoordinate = [];
        var endLineCoordinate = [];
        if (vm.selectedDP && vm.selectedLayer) {
            setSelections({
                featureID: vm.selectedDP.getId(), layer: vm.selectedLayer
            }, []);
        }

        vm.interactions.draw.on('drawstart',
			function (evt) {
			    //removeInteraction('select');
			    clearDrawingLayer(true);
			    // setSelections(null, []);
			    $rootScope.$broadcast('disablePrintMap', {
			        disable: true
			    });
			});
        vm.interactions.draw.on('drawend',
			function (evt) {
			    evt.feature.set("type", "accesslink");
			    var coordinates = evt.feature.getGeometry().getCoordinates();
			    accessLinkCoordinatesService.setCordinates(coordinates);
			    $rootScope.state = false;
			    $stateParams.accessLinkFeature = evt.feature;
			    var layer = mapFactory.getLayer('Drawing');
			    vm.map.getInteractions().forEach(function (interaction) {
			        if (interaction instanceof ol.interaction.Select) {
			            interaction.getFeatures().clear();
			        }
			    });

			    $rootScope.$broadcast('redirectTo', {
			        feature: evt.feature,
			        contextTitle: CommonConstants.AccessLinkActionName
			    });
			    //$state.go("accessLink", { accessLinkFeature: evt.feature }, {
			    //    reload: 'accessLink'
			    //});
			});
    }
    function finishCondition(e) {
        var intersectionPoint = e.coordinate;

        var featurePresent = vm.map.forEachFeatureAtPixel(e.pixel, function (feature, layer) {
            var dpCoordinates = coordinatesService.getCordinates();
            if ((dpCoordinates && dpCoordinates != '') && (layer && layer.get("name") === GlobalSettings.roadLinkLayerName)) {
                roadLinkGuidService.setRoadLinkGuid(feature.getId());
                intersectionPointService.setIntersectionPoint(intersectionPoint);
                return true;
            }
            else {
                return false;
            }
        });
        return featurePresent;
    }
    function snapOnFeature(vector) {
        vm.interactions.snap = new ol.interaction.Snap({
            source: vector.layer.getSource(),
            pixelTolerance: 20
        });
    }
    function setSelectButton() {
        var lastLayer;
        var style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE);
        vm.interactions.select = new ol.interaction.Select({
            condition: ol.events.condition.click,
            toggleCondition: ol.events.condition.never,
            filter: function (a, layer) {
                lastLayer = layer;
                if (vm.layersForContext.length > 0)
                    return vm.layersForContext.indexOf(layer.get('name')) != -1;
                return true;
            },
            style: style
        });
        vm.interactions.select.on('select', function (e) {
            vm.interactions.select.getFeatures().clear();
            if (e.selected.length > 0) {
                vm.isSelected = true;               
                    $rootScope.$emit('selectedObjectChange', { "isSelected": vm.isSelected });
                setSelections({
                    featureID: e.selected[0].getId(), layer: lastLayer
                }, []);
                vm.layerName = lastLayer.getProperties().name;
                var deliveryPointDetails = e.selected[0].getProperties();
                coordinatesService.setCordinates(deliveryPointDetails.geometry.flatCoordinates);
                $rootScope.state = false;
                guidService.setGuid(deliveryPointDetails.deliveryPointId);
                showDeliveryPointDetails(deliveryPointDetails);
                vm.selectedDP = e.selected[0];
                vm.selectedLayer = lastLayer;
            } else {
                vm.isSelected = false;
                vm.layerName = "";
                $rootScope.$emit('selectedObjectChange', { "isSelected": vm.isSelected });
                setSelections(null, []);
                var deliveryPointDetails = null;
                showDeliveryPointDetails(deliveryPointDetails);
            }
        });
        persistSelection();
    }
    function setModifyButton() {       
            vm.interactions.select = new ol.interaction.Select({
                condition: ol.events.condition.never
            });
            var collection = new ol.Collection();
            collection.push(getActiveFeature());
            if (vm.layerName == "Drawing") {
            vm.interactions.modify = new ol.interaction.Modify({
                features: collection
            });
            

            vm.map.on('singleclick', function (evt) {
                if (vm.activeTool === "modify") {
                    getActiveFeature().getGeometry().setCoordinates(evt.coordinate);
                }
            });

            vm.interactions.modify.on('modifyend', vm.onModify);

            persistSelection();
        }
    }
    function persistSelection() {
        if (getActiveFeature() != null && vm.interactions.select != null && angular.isDefined(vm.interactions.select)) {
            var features = vm.interactions.select.getFeatures();
            features.push(getActiveFeature());
            getSecondaryFeatures().forEach(function (feature) {
                features.push(feature);
            })
        }
    }
    function enableDrawingLayer() {
        if (!vm.drawingLayer.selected) {
            vm.drawingLayer.selected = true;
            $timeout(function () {
                refreshLayers();
            });
        }
    }
    function setupDP() {
        vm.interactions.draw.on('drawstart',
                         function (evt) {
                             removeInteraction("select");
                             clearDrawingLayer(true);
                             setSelections(null, []);
                         });
        vm.interactions.draw.on('drawend',
			function (evt) {
			    evt.feature.set("type", "deliverypoint");
			    var coordinates = evt.feature.getGeometry().getCoordinates();
			    coordinatesService.setCordinates(coordinates);
			    $rootScope.state = false;
			});
    }
    function clearDrawingLayer(keepCurrentInteraction) {
        if (vm.measureTooltipElement != null) {
            vm.measureTooltipElement.parentNode.removeChild(vm.measureTooltipElement);
            vm.measureTooltipElement = null;
        }
        vm.lastMeasureFeature = null;
        vm.drawingLayer.layer.getSource().clear();
        if (keepCurrentInteraction != true) {
            mapFactory.removeCurrentInteractions();
        }
    }
    function removeInteraction(key) {
        vm.map.removeInteraction(vm.interactions[key]);
        delete vm.interactions[key];
    }
    function formatLength(line) {
        var length = Math.round(line.getLength() * 100) / 100;
        var output;
        if (length > 100) {
            output = (Math.round(length / 1000 * 100) / 100) +
                ' ' + 'km';
        } else {
            output = (Math.round(length * 100) / 100) +
                ' ' + 'm';
        }
        return output;
    }
    function getDotStyle() {
        return new ol.style.Style({
            image: new ol.style.Circle({
                radius: 10,
                fill: null,
                stroke: new ol.style.Stroke({
                    color: 'red',
                    width: 12
                })
            })
        });
    }
    function syncMinimapAnimation(event) {
        var vectorContext = event.vectorContext;
        if (vm.latestDrawnFeature) {
            vectorContext.drawFeature(vm.latestDrawnFeature, vm.dotStyle);
        }
        vm.minimap.render();
    }
    function oncollapse(collapse) {
        if (!collapse)
            $timeout(function () {
                vm.miniMap.updateSize()
            }, 10);
    }
    function mapToolChange(button) {
        vm.activeTool = button.name;
        removeCurrentInteractions();
        if (button.enabled) {
            switch (button.name) {
                case "select":
                    setSelectButton();
                    break;
                case "modify":
                    setModifyButton();
                    break;
                case "delete":
                    deleteSelectedFeature();
                    break;
                default:
                    setDrawButton(button);
            }
            addInteractions();
        }
    }

    function selectFeatures() {
        if (vm.interactions.select == null || angular.isUndefined(vm.interactions.select)) {
            vm.interactions.select = new ol.interaction.Select({
                condition: ol.events.condition.never,
                style: mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE)
            });
            vm.map.addInteraction(vm.interactions.select);
        }
        vm.interactions.select.getFeatures().clear();
        vm.features.forEach(function (feature) {
            vm.interactions.select.getFeatures().push(feature);
        })
    }

    function getfeature(feature) {
        vm.features = feature;
    }

    function setSelectedObjectsVisibility(selectedLayer) {
        if (vm.interactions.select) {
            vm.interactions.select.getFeatures().forEach(function (feature) {
                vm.featuredType = feature.get("type");
                if (vm.featuredType === "deliverypoint") {
                    vm.featuredType = "Delivery Points"
                }
                if (vm.featuredType === "accesslink") {
                    vm.featuredType = "Access Links"
                }
                if (vm.featuredType === "roadlink") {
                    vm.featuredType = "Roads"
                }
                if (vm.featuredType === selectedLayer) {
                    setSelections(null, []);
                }
            });
        }
    }

    function showDeliveryPointDetails(deliveryPointDetails) {
        if (deliveryPointDetails != null) {
            deliveryPointDetails.routeName = null;
            mapFactory.GetRouteForDeliveryPoint(deliveryPointDetails.deliveryPointId)
                  .then(function (response) {
                      if (response != null) {
                          if (response[0].key == CommonConstants.RouteName) {
                              deliveryPointDetails.routeName = [response[0].value];
                              if (response[1].key == CommonConstants.DpUse) {
                                  deliveryPointDetails.dpUse = response[1].value;
                              }
                          }
                          else if (response[0].key == CommonConstants.DpUse) {
                              deliveryPointDetails.dpUse = response[0].value;
                          }
                      }
                      $state.go('deliveryPointDetails', {
                          selectedDeliveryPoint: deliveryPointDetails
                      }, { reload: true });
                  });
        }
        else {
            $state.go('deliveryPointDetails', {
                selectedDeliveryPoint: deliveryPointDetails
            }, { reload: true });
        }
    }

    function setSize(width, height) {
        vm.map.setSize([width, height]);
    }

    function setOriginalSize() {
        vm.map.setSize(vm.originalSize);
    }

    function composeMap() {
        vm.map.once('postcompose', function (event) {
            writeScaletoCanvas(event);
        });
        vm.map.renderSync();
    }

    function writeScaletoCanvas(e) {
        var ctx = e.context;
        var canvas = e.context.canvas;
        //get the Scaleline div container the style-width property
        var olscale = document.getElementsByClassName('ol-scale-line-inner')[0];
        //Scaleline thicknes
        var line1 = 6;
        //Offset from the left
        var x_offset = 10;
        //offset from the bottom
        var y_offset = 30;
        var fontsize1 = 15;
        var font1 = fontsize1 + 'px Arial';
        // how big should the scale be (original css-width multiplied)
        var multiplier = 2;
        var scalewidth = parseInt(olscale.style.width, 10) * multiplier;
        var scale = olscale.innerHTML;
        var scalenumber = parseInt(scale, 10);
        var scaleunit = scale.match(/[Aa-zZ]{1,}/g);

        var calculatedScale = setScaleUnit(scalenumber, scaleunit);
        //Scale Text
        ctx.beginPath();
        ctx.textAlign = "left";
        ctx.strokeStyle = "#ffffff";
        ctx.fillStyle = "#000000";
        ctx.lineWidth = 5;
        ctx.font = font1;
        ctx.strokeText([calculatedScale], x_offset + fontsize1 / 2, canvas.height - y_offset - fontsize1 / 2);
        ctx.fillText([calculatedScale], x_offset + fontsize1 / 2, canvas.height - y_offset - fontsize1 / 2);

        //Scale Dimensions
        var xzero = scalewidth + x_offset;
        var yzero = canvas.height - y_offset;
        var xfirst = x_offset + scalewidth * 1 / 4;
        var xsecond = xfirst + scalewidth * 1 / 4;
        var xthird = xsecond + scalewidth * 1 / 4;
        var xfourth = xthird + scalewidth * 1 / 4;

        // Stroke
        ctx.beginPath();
        ctx.lineWidth = line1 + 2;
        ctx.strokeStyle = "#000000";
        ctx.fillStyle = "#ffffff";
        ctx.moveTo(x_offset, yzero);
        ctx.lineTo(xzero + 1, yzero);
        ctx.stroke();

        //sections black/white
        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#000000";
        ctx.moveTo(x_offset, yzero);
        ctx.lineTo(xfirst, yzero);
        ctx.stroke();

        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#FFFFFF";
        ctx.moveTo(xfirst, yzero);
        ctx.lineTo(xsecond, yzero);
        ctx.stroke();

        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#000000";
        ctx.moveTo(xsecond, yzero);
        ctx.lineTo(xthird, yzero);
        ctx.stroke();

        ctx.beginPath();
        ctx.lineWidth = line1;
        ctx.strokeStyle = "#FFFFFF";
        ctx.moveTo(xthird, yzero);
        ctx.lineTo(xfourth, yzero);
        ctx.stroke();

        $rootScope.canvas = canvas;
    }

    function setScaleUnit(scalenumber, scaleunit) {
        if (scaleunit == 'km') {
            var scale = scalenumber * 0.621371;
            if (scale < 1) {
                scale = scalenumber * 1000
                return Math.round(scale) + ' Metre';
            }
            else {
                return Math.round(scale) + ' Mile';
            }
        }
        else if (scaleunit == 'm') {
            var scale = scalenumber * 0.000621371;
            if (scale < 1) {
                return scalenumber + ' Metre';
            }
            else {
                return Math.round(scale) + ' Mile';
            }
        }
    }

    function getResolution() {
        return vm.map.getView().getResolution();
    }
    function createGuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}