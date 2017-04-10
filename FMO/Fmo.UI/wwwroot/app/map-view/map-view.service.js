angular.module('mapView')
        .service('mapService', ['mapFactory',
                                'mapStylesFactory','$timeout',
                                 mapService])
function mapService(mapFactory,
                    mapStylesFactory, $timeout) {
    var vm = this;
    vm.map = null;
    vm.miniMap = null;
    vm.activeTool = "";
    vm.focusedLayers = [];
    vm.mapButtons = [ "select", "point","line"];
    vm.interactions = {};
    vm.layersForContext = [];
    vm.activeSelection = null;
    vm.secondarySelections = [];
    vm.selectionListeners = [];
    vm.onDeleteButton = function (featureId, layer) { console.log({ "featureID": featureId, "layer": layer }) };
    vm.onModify = function (feature) { console.log(feature) };
    vm.onDrawEnd = function (buttonName, feature) { console.log(buttonName, feature) };
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
        refreshLayers:refreshLayers
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
        
        var digitalGlobeTiles = new ol.layer.Tile({
            title: 'DigitalGlobe Maps API: Recent Imagery',
            source: new ol.source.XYZ({
                url: GlobalSettings.vectorMapUrl,
                attribution: "� DigitalGlobe, Inc"
            })
        });

        var bingMapsSatelliteTiles = new ol.layer.Tile({
            title: 'Bing maps API',
            source: new ol.source.BingMaps({
                key: 'Arja12vfzFSIIbJWozrlmn3bVgk-G-mKz1pHNcYUtJ1_sJV3mpZIna-ExcYUxA2F',
                imagerySet: 'AerialWithLabels',
                maxZoom: 19
            })
        });

        var osmRoadMapTiles = new ol.layer.Tile({
            source: new ol.source.OSM()
        });

        var bingMapsRoadTiles = new ol.layer.Tile({
            title: 'Bing maps API',
            source: new ol.source.BingMaps({
                key: 'Arja12vfzFSIIbJWozrlmn3bVgk-G-mKz1pHNcYUtJ1_sJV3mpZIna-ExcYUxA2F',
                imagerySet: 'Road',
                maxZoom: 19
            })
        });

        var mockWFSVector = new ol.source.Vector({
            format: new ol.format.GeoJSON(),
            url: function (extent) { return 'http://localhost:47467/home/getdata?bbox=' + extent.join(','); },
            strategy: ol.loadingstrategy.bbox
        });

        var mockWFSLayer = new ol.layer.Vector({
            source: mockWFSVector
        });

        var mockGroupsVector = new ol.source.Vector({
            format: new ol.format.GeoJSON(),
            url: function (extent) { return 'http://localhost:47467/home/getgroupsdata?bbox=' + extent.join(','); },
            strategy: ol.loadingstrategy.bbox
        });

        var mockAccessLinkVector = new ol.source.Vector({
            format: new ol.format.GeoJSON(),
            // url: function (extent) { return 'http://localhost:47467/home/getdata?bbox=' + extent.join(','); },
            url: function (extent) { return 'http://localhost:34583/api/accessLinks/GetAccessLinks?bbox=' + extent.join(','); },
            strategy: ol.loadingstrategy.bbox
        });

        //var mockAccessLinkLayer = new ol.layer.Vector({
        //    source: mockAccessLinkVector
        //});
        var mockGroupsLayer = new ol.layer.Vector({
            source: mockGroupsVector
        });

        var mockAccessLinkLayer = new ol.layer.Vector({
            source: mockAccessLinkVector
        });

        var mockRoadLinkLayer = new ol.layer.Vector({
            source: mockRoadLinkLayer
        });

        var roadsSelector = new MapFactory.LayerSelector();
        roadsSelector.layerName = "Base Layer";
        //roadsSelector.layer = osmRoadMapTiles;
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

      
        var mockWFSLayerSelector = new MapFactory.LayerSelector();
        mockWFSLayerSelector.layerName = "Delivery Points";
        mockWFSLayerSelector.layer = mockWFSLayer;
        mockWFSLayerSelector.group = "";
        mockWFSLayerSelector.zIndex = 8;
        mockWFSLayerSelector.selected = false;
        mockWFSLayerSelector.onMiniMap = false;
        mockWFSLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        mockWFSLayerSelector.keys = ["deliverypoint"];
        mapFactory.addLayer(mockWFSLayerSelector);

        var mockAccessLinksLayerSelector = new MapFactory.LayerSelector();
        mockAccessLinksLayerSelector.layerName = "Access Links";
        mockAccessLinksLayerSelector.layer = mockAccessLinkLayer;
        mockAccessLinksLayerSelector.group = "";
        mockAccessLinksLayerSelector.zIndex = 8;
        mockAccessLinksLayerSelector.selected = false;
        mockAccessLinksLayerSelector.onMiniMap = false;
        mockAccessLinksLayerSelector.selectorVisible = true;
        mockAccessLinksLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        mockAccessLinksLayerSelector.keys = ["accesslink"];
        mapFactory.addLayer(mockAccessLinksLayerSelector);

        var mockRoadsLayerSelector = new MapFactory.LayerSelector();
        mockRoadsLayerSelector.layerName = "Road Links";
        mockRoadsLayerSelector.layer = mockRoadLinkLayer;
        mockRoadsLayerSelector.group = "";
        mockRoadsLayerSelector.zIndex = 8;
        mockRoadsLayerSelector.selected = false;
        mockRoadsLayerSelector.onMiniMap = false;
        mockRoadsLayerSelector.selectorVisible = true;
        mockRoadsLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        mockRoadsLayerSelector.keys = ["roadlink"];
        mapFactory.addLayer(mockRoadsLayerSelector);

        roadsLayer.selected = true;
        vm.map.on('pointermove', vm.pointerMoveHandler);
        refreshLayers();

        vm.showRoadPanel = false;
    }
    function initialiseMiniMap() {
        mapFactory.initialiseMiniMap();
        vm.miniMap = mapFactory.getMiniMap();
    }
    function getMapButtons() {
        return vm.mapButtons;
    }
    function mapLayers()
    {
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
        });
        vm.layerSummary = getLayerSummary();
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
        if (vm.interactions.select) {
            vm.interactions.select.getFeatures().forEach(function (feature) {
                if (feature.get("type") == "measure") {
                    if (vm.measureTooltipElement) {
                        vm.measureTooltipElement.parentNode.removeChild(vm.measureTooltipElement);
                        vm.measureTooltipElement = null;
                    }
                    vm.lastMeasureFeature = null;
                }
                if (vm.drawingLayer.layer.getSource().getFeatures().indexOf(feature) != -1)
                    vm.drawingLayer.layer.getSource().removeFeature(feature);
                vm.onDeleteButton(feature.getId(), feature.layer);
            });
            vm.interactions.select.getFeatures().clear();
            vm.setSelections(null, []);
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
        vm.interactions.draw = new ol.interaction.Draw({
            source: vm.drawingLayer.layer.getSource(),
            type: button.shape,
            style: style
        });
        switch (button.name) {
            case "measure":
                setupMeasure();
                break;
            case "group":
                setupGroup();
                break;
            case "deliverypoint":
                setupDP();
                break;
            default:
                break;
        }
        var name = button.name;
        if (name == "point") {
            vm.interactions.draw.on('drawend', function (evt) {
                evt.feature.set("type", "deliverypoint");

            })
        }
        else {
            vm.interactions.draw.on('drawstart', enableDrawingLayer, this);
        }

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
                vm.setSelections({ featureID: e.selected[0].getId(), layer: lastLayer }, []);
            } else {
                vm.setSelections(null, []);
            }

        });
        persistSelection();
    }
    function setModifyButton() {
        vm.interactions.select = new ol.interaction.Select({
            condition: ol.events.condition.never
        });
        var collection = new ol.Collection();
        collection.push(vm.getActiveFeature());
        vm.interactions.modify = new ol.interaction.Modify({
            features: collection
        });
        vm.interactions.modify.on('modifyend',  onModify());
        persistSelection();
    }
    function persistSelection() {
        if (vm.getActiveFeature() != null && vm.interactions.select != null && vm.interactions.select != undefined) {
            var features = vm.interactions.select.getFeatures();
            features.push(vm.getActiveFeature());
            vm.getSecondaryFeatures().forEach(function (feature) {
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
    function setupMeasure() {
        vm.interactions.draw.on('drawstart',
            function (evt) {
                createMeasureTooltip();
                if (vm.lastMeasureFeature) {
                    mapFactory.getVectorSource().removeFeature(vm.lastMeasureFeature);
                }
                vm.sketch = evt.feature;
            }, this);
        vm.interactions.draw.on('drawend',
            function (evt) {
                vm.measureTooltipElement.className = 'tooltip';
                vm.measureTooltip.setOffset([0, -7]);
                vm.lastMeasureFeature = evt.feature;

                vm.map.addOverlay(vm.measureTooltip);

                vm.sketch = null;

            }, this);
    }
    function setupGroup() {
        vm.interactions.draw.on('drawstart',
			function (evt) {
			    removeInteraction("select");
			    clearDrawingLayer(true);
			    vm.setSelections(null, []);
			});
        vm.interactions.draw.on('drawend',
			function (evt) {
			    evt.feature.setId(0);
			    $timeout(function () {
			       vm.setSelections({ featureID: evt.feature.getId(), layer: vm.drawingLayer.layer }, [])
			        onDrawEnd("group", evt.feature)
			    });
			});
    }
    function setupDP() {
        vm.interactions.draw.on('drawstart',
			function (evt) {
			    removeInteraction("select");
			    clearDrawingLayer(true);
			    vm.setSelections(null, []);
			});
        vm.interactions.draw.on('drawend',
			function (evt) {
			    evt.feature.setId(0);
			    $timeout(function () {
			       vm. setSelections({ featureID: evt.feature.getId(), layer: vm.drawingLayer.layer }, [])
			        onDrawEnd("deliverypoint", evt.feature)
			    });
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
    function createMeasureTooltip() {
        if (vm.measureTooltipElement) {
            vm.measureTooltipElement.parentNode.removeChild(vm.measureTooltipElement);
        }
        vm.measureTooltipElement = document.createElement('div');
        vm.measureTooltipElement.className = 'tooltip tooltip-measure';
        vm.measureTooltip = new ol.Overlay({
            element: vm.measureTooltipElement,
            offset: [0, -15],
            positioning: 'bottom-center'
        });
        vm.map.addOverlay(vm.measureTooltip);
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
    function syncMinimapAnimation(event)
    {
        var vectorContext = event.vectorContext;
        if (vm.latestDrawnFeature) {
            vectorContext.drawFeature(vm.latestDrawnFeature, vm.dotStyle);
        }
        vm.minimap.render();
    }
    function oncollapse(collapse)
    {   if (!collapse)
            $timeout(function () { vm.miniMap.updateSize() }, 10);
    }
    function mapToolChange(button)
    {
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
                default:
                    setDrawButton(button);
            }
            addInteractions();
        }
    }
}
