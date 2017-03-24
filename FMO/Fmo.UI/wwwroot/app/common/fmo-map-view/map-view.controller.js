'use strict';

angular.module('mapView')
	.run(['$route', function () { }])
	.controller('MapController', ['$scope', 'mapFactory', '$timeout', 'mapService', 'mapStylesFactory', '$interval', '$http', MapController])

function MapController($scope, mapFactory, $timeout, mapService, mapStylesFactory, $interval, $http) {
    var vm = this;
    vm.showNotification = showNotification;
    var tasks;
    vm.counter = 0;

    var getDeliveryPoints = function () {
        notificationFactory.getDeliveryPoints()
            .then(function (response) {
                vm.tasks = response.data;
                if (vm.tasks.length > 0) {
                    vm.counter = vm.tasks.length;
                    vm.color = "green";
                }
                else {
                    vm.color = "white";
                }

            }, function (error) {
                //$scope.status = 'Unable to load customer data: ' + error.message;
            });
    }


    $interval(getDeliveryPoints, 30000);

    //$scope.$on('changeColor', function (event, data) {
    //    debugger;
    //    var vm = this;
    //    vm.counter = 0;
    //    vm.color = "white";
    //});
    $scope.$on('eventX', function (ev, args) {
        debugger;
        console.log('eventX found on Controller1 $scope');
    });

    //var increaseCounter = function () {
    //    //get the list of tasks from the json file    

    //    $http.get('data/json/tasks.json').then(function (response) {
    //        vm.tasks = response.data.tasksList;
    //        if (vm.tasks.length > 0) {
    //            vm.counter = vm.counter + 1;
    //            vm.color = "green";

    //            //$scope.$broadcast('panelcolorchange', { data: "red" });
    //        }
    //        // sortBy(vm.propertyName);
    //    });

    //}

    //$interval(increaseCounter, 30000);
    $scope.$on('refreshLayers', refreshLayers);

    vm.streetName = "No Street Infomation";

    $scope.$on('updateStreetName', function (event, name, deadhead) {
        $timeout(function () {
            if (deadhead) {
                // vm.streetName = "No Street Infomation";
                // vm.streetName = "No Street Infomation";
            } else {
                vm.streetName = name;
            }
        });
    });

    function showNotification() {
        $scope.$emit('showNotification');
    }

    $scope.$on('updateBlockInfo', function (event, blockObj) {
        $timeout(function () {
            vm.blockObj = blockObj;
        });
    });

    $scope.$on("mapToolChange", function (event, button) {
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
    });

    $scope.$on('roadPanelChange', function (event, show) {
        vm.showRoadPanel = show;
    });

    $scope.$on("deleteSelectedFeature", function (event) {
        deleteSelectedFeature();
    });

    $scope.$on("saveDrawingLayer", function (event) {
        var formatter = new ol.format.GeoJSON();
        console.log("saveDrawingLayer:: Save called!");
        mapService.onSaveButton(formatter.writeFeatures(vm.drawingLayer.layer.getSource().getFeatures()));

    });

    mapService.addSelectionListener(selectFeatures);
    mapService.mapLayers = mapFactory.getAllLayers();
    mapService.mapButtons = ["area", "line", "measure", "select", "delete", "modify"];
    mapService.centerMapOn = centerMapOn;
    mapService.centerMapOnFeature = centerMapOnFeature;
    mapService.clearDrawingLayer = clearDrawingLayer;
    mapService.getFeaturesWithinFeature = getFeaturesWithinFeature;
    mapService.getFeaturesInExtent = getFeaturesInExtent;
    mapService.getVisibleFeatures = getVisibleFeatures;
    mapService.styleLayers = styleLayers;

    vm.activeTool = "";
    vm.toggleActions = toggleActions;
    vm.map = null;
    vm.miniMap = null;
    vm.interactions = {};
    vm.dotStyle = getDotStyle();
    vm.initialiseMiniMap = initialiseMiniMap;
    vm.hideMiniMap = false;
    vm.oncollapse = oncollapse;

    vm.sketch = null;
    vm.measureTooltipElement = null;
    vm.measureTooltip = null;
    vm.featureCount = 0;

    /**
     * Handle pointer move.
     * @param {ol.MapBrowserEvent} evt
     */
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

    initialise();

    function initialise() {
        mapFactory.initialiseMap();

        vm.map = mapFactory.getMap();

        mapService.onMap = function (event, callback) { vm.map.on(event, callback) };
        mapService.offMap = function (event, callback) { vm.map.un(event, callback) };

        var digitalGlobeTiles = new ol.layer.Tile({
            title: 'DigitalGlobe Maps API: Recent Imagery',
            source: new ol.source.XYZ({
                url: 'http://api.tiles.mapbox.com/v4/digitalglobe.nal0g75k/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoiZGlnaXRhbGdsb2JlIiwiYSI6ImNpcGg5dHkzYTAxM290bG1kemJraHU5bmoifQ.CHhq1DFgZPSQQC-DYWpzaQ',
                attribution: "© DigitalGlobe, Inc"
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

        var mockGroupsLayer = new ol.layer.Vector({
            source: mockGroupsVector
        });

        var satelliteSelector = new MapFactory.LayerSelector();
        satelliteSelector.layerName = "Satellite";
        satelliteSelector.layer = digitalGlobeTiles;
        //satelliteSelector.layer = bingMapsSatelliteTiles;
        satelliteSelector.group = "Base Map";
        satelliteSelector.onMiniMap = true;
        satelliteSelector.selectorVisible = false;
        //var satelliteLayer = mapFactory.addLayer(satelliteSelector);

        var roadsSelector = new MapFactory.LayerSelector();
        roadsSelector.layerName = "Roads";
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

        var routesSelector = new MapFactory.LayerSelector();
        routesSelector.layerName = "Routes";
        routesSelector.layerUrl = "data/json/mock_routes.json";
        routesSelector.group = "Routes";
        routesSelector.zIndex = 4;
        routesSelector.selected = true;
        routesSelector.onMiniMap = true;
        routesSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        routesSelector.keys = ["route"];
        mapFactory.createLayerAsync(routesSelector).then(refreshLayers);

        var deliveryPointsSelector = new MapFactory.LayerSelector();
        deliveryPointsSelector.layerName = "Delivery Points";
        deliveryPointsSelector.layerUrl = "data/json/mock_points.json";
        deliveryPointsSelector.group = "";
        deliveryPointsSelector.zIndex = 5;
        deliveryPointsSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        deliveryPointsSelector.keys = ["deliverypoint"]
        mapFactory.createLayerAsync(deliveryPointsSelector).then(refreshLayers);

        var rmgPointsSelector = new MapFactory.LayerSelector();
        rmgPointsSelector.layerName = "RMG Points";
        rmgPointsSelector.layerUrl = "data/json/rmg_points.json";
        rmgPointsSelector.group = "";
        rmgPointsSelector.zIndex = 6;
        rmgPointsSelector.selected = false;
        rmgPointsSelector.onMiniMap = false;
        rmgPointsSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        rmgPointsSelector.keys = ["rmgpoint", "accessLink"];
        mapFactory.createLayerAsync(rmgPointsSelector).then(refreshLayers);

        var groupsSelector = new MapFactory.LayerSelector();
        groupsSelector.layerName = "Groups";
        groupsSelector.layerUrl = "http://localhost:47467/home/getgroupsdata";
        groupsSelector.group = "";
        groupsSelector.zIndex = 7;
        groupsSelector.selected = false;
        groupsSelector.onMiniMap = false;
        groupsSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        groupsSelector.keys = ["deliverypoint", "group"];
        mapFactory.createLayerAsync(groupsSelector).then(refreshLayers);



        var mockWFSLayerSelector = new MapFactory.LayerSelector();
        mockWFSLayerSelector.layerName = "WFS mock";
        mockWFSLayerSelector.layer = mockWFSLayer;
        mockWFSLayerSelector.group = "";
        mockWFSLayerSelector.zIndex = 7;
        mockWFSLayerSelector.selected = false;
        mockWFSLayerSelector.onMiniMap = false;
        mockWFSLayerSelector.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
        mockWFSLayerSelector.keys = ["deliverypoint"];
        mapFactory.addLayer(mockWFSLayerSelector);

        roadsLayer.selected = true;
        vm.map.on('pointermove', vm.pointerMoveHandler);
        refreshLayers();

        vm.showRoadPanel = false;
    }

    function initialiseMiniMap() {
        mapFactory.initialiseMiniMap();
        vm.miniMap = mapFactory.getMiniMap();
    }

    function oncollapse(collapse) {
        if (!collapse)
            $timeout(function () { vm.miniMap.updateSize() }, 10);
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
                mapService.onDeleteButton(feature.getId(), feature.layer);
            });
            vm.interactions.select.getFeatures().clear();
            mapService.setSelections(null, []);
        }

    }

    function getLayerSummary() {
        var summary = "";
        mapService.mapLayers.forEach(function (layer) {
            if (layer.selected && layer.selectorVisible)
                summary += layer.layerName + ", ";
        });
        if (summary.lastIndexOf(", ") == summary.length - 2)
            summary = summary.substring(0, summary.length - 2);
        else
            summary = "No layers selected";

        return summary;
    }

    function setSelectButton() {
        var lastLayer;
        var style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE);
        vm.interactions.select = new ol.interaction.Select({
            condition: ol.events.condition.click,
            toggleCondition: ol.events.condition.never,
            filter: function (a, layer) {
                lastLayer = layer;
                if (mapService.layersForContext.length > 0)
                    return mapService.layersForContext.indexOf(layer.get('name')) != -1;
                return true;
            },
            style: style
        });

        vm.interactions.select.on('select', function (e) {
            vm.interactions.select.getFeatures().clear();
            if (e.selected.length > 0) {
                mapService.setSelections({ featureID: e.selected[0].getId(), layer: lastLayer }, []);
            } else {
                mapService.setSelections(null, []);
            }

        });

        persistSelection();
    }

    function setModifyButton() {
        vm.interactions.select = new ol.interaction.Select({
            condition: ol.events.condition.never
        });
        var collection = new ol.Collection();
        collection.push(mapService.getActiveFeature());
        vm.interactions.modify = new ol.interaction.Modify({
            features: collection
        });

        vm.interactions.modify.on('modifyend', mapService.onModify);

        persistSelection();
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
        vm.interactions.draw.on('drawstart', enableDrawingLayer, this);

    }

    function addFeatureData(evt) {
        evt.feature.set("type", vm.activeTool);
        evt.feature.setId(vm.featureCount++);
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
                // set sketch
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
			    mapService.setSelections(null, []);
			});
        vm.interactions.draw.on('drawend',
			function (evt) {
			    evt.feature.setId(0);
			    $timeout(function () {
			        mapService.setSelections({ featureID: evt.feature.getId(), layer: vm.drawingLayer.layer }, [])
			        mapService.onDrawEnd("group", evt.feature)
			    });
			});
    }

    function setupDP() {
        vm.interactions.draw.on('drawstart',
			function (evt) {
			    removeInteraction("select");
			    clearDrawingLayer(true);
			    mapService.setSelections(null, []);
			});
        vm.interactions.draw.on('drawend',
			function (evt) {
			    evt.feature.setId(0);
			    $timeout(function () {
			        mapService.setSelections({ featureID: evt.feature.getId(), layer: vm.drawingLayer.layer }, [])
			        mapService.onDrawEnd("deliverypoint", evt.feature)
			    });
			});
    }

    function removeCurrentInteractions() {
        for (var key in vm.interactions) {
            vm.map.removeInteraction(vm.interactions[key]);
            delete vm.interactions[key];
        }

    }

    function removeInteraction(key) {
        vm.map.removeInteraction(vm.interactions[key]);
        delete vm.interactions[key];
    }

    function addInteractions() {
        for (var key in vm.interactions) {
            vm.map.addInteraction(vm.interactions[key]);
        }

    }

    function formatLength(line) {
        //assuming non-geodesic calculation for now as working on relatively small areas
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

    function clearDrawingLayer(keepCurrentInteraction) {
        if (vm.measureTooltipElement != null) {
            vm.measureTooltipElement.parentNode.removeChild(vm.measureTooltipElement);
            vm.measureTooltipElement = null;
        }
        vm.lastMeasureFeature = null;
        vm.drawingLayer.layer.getSource().clear();
        if (keepCurrentInteraction != true) {
            removeCurrentInteractions();
        }
    }

    function toggleActions() {
        vm.hideActions = !vm.hideActions;
    }

    function syncMinimapAnimation(event) {
        var vectorContext = event.vectorContext;

        if (vm.latestDrawnFeature) {
            vectorContext.drawFeature(vm.latestDrawnFeature, vm.dotStyle);
        }

        vm.minimap.render();
    }

    function refreshLayers() {
        mapService.mapLayers.forEach(function (layer) {
            layer.layer.setVisible(layer.selected);
        });
        vm.layerSummary = getLayerSummary();
    }

    function styleLayers(activeLayer) {
        mapService.mapLayers.forEach(function (layer) {
            if (layer.layer.setStyle != undefined) {
                if (activeLayer != undefined && activeLayer != layer.layer && layer.layer != vm.drawingLayer.layer) {
                    layer.layer.setStyle(mapStylesFactory.getStyle(mapStylesFactory.styleTypes.INACTIVESTYLE));
                } else {
                    layer.layer.setStyle(mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE));
                }
            }
        })
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

    function centerMapOn(x, y, z) {
        z = z || 15;
        animateTransition(500);
        var transform = ol.proj.transform([x, y], 'EPSG:4326', 'EPSG:3857');
        vm.map.getView().setCenter(transform);
        vm.map.getView().setZoom(z);
    }

    function centerMapOnFeature(feature, zoomScale) {
        zoomScale = zoomScale || 1.2;
        var extent = feature.getGeometry().getExtent().slice(0);
        var xCenter = (extent[0] + extent[2]) / 2;
        var yCenter = (extent[1] + extent[3]) / 2;
        extent[0] = (extent[0] - xCenter) * zoomScale + extent[0];
        extent[1] = (extent[1] - yCenter) * zoomScale + extent[1];
        extent[2] = (extent[2] - xCenter) * zoomScale + extent[2];
        extent[3] = (extent[3] - yCenter) * zoomScale + extent[3];
        animateTransition(500);
        vm.map.getView().fit(extent, vm.map.getSize());
    }

    function animateTransition(duration) {
        var center = vm.map.getView().getCenter();
        var resolution = vm.map.getView().getResolution();
        var pan = ol.animation.pan({
            duration: duration,
            source: center,
            easing: ol.easing.linear
        });
        var zoom = ol.animation.zoom({
            duration: duration,
            resolution: resolution,
            easing: ol.easing.linear
        });
        vm.map.beforeRender(pan, zoom);
    }

    function getFeaturesWithinFeature(layer, srcFeature) {
        //   srcFeature = vm.interactions.select.getFeatures()[0];
        var extent = srcFeature.getGeometry().getExtent();
        var inside = [];
        getFeaturesInExtent(layer, extent).forEach(function (feature) {

            var format = new ol.format.GeoJSON();
            var point = format.writeFeatureObject(feature, {
                featureProjection: 'EPSG:3857',
                dataProjection: 'EPSG:4326'
            });

            var feat = format.writeFeatureObject(srcFeature, {
                featureProjection: 'EPSG:3857',
                dataProjection: 'EPSG:4326'
            });

            if (turf.inside(point, feat)) {
                inside.push(feature);
            }

        });
        return inside;
    }

    function getFeaturesInExtent(layer, extent, filter) {
        var inside = [];
        filter = filter || function () { return true; };
        layer.forEachFeatureInExtent(extent, function (feature) {
            if (filter(feature)) {
                inside.push(feature);
            }
        })
        return inside;
    }

    function getVisibleFeatures(layer, filter) {
        var extent = vm.map.getView().calculateExtent(vm.map.getSize());
        return mapService.getFeaturesInExtent(layer, extent, filter);
    }

    function selectFeatures(features) {
        if (vm.interactions.select == null || vm.interactions.select == undefined) {
            vm.interactions.select = new ol.interaction.Select({
                condition: ol.events.condition.never,
                style: mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE)
            });
            vm.map.addInteraction(vm.interactions.select);
        }
        vm.interactions.select.getFeatures().clear();
        features.forEach(function (feature) {
            vm.interactions.select.getFeatures().push(feature);
        })
    }

    function persistSelection() {
        if (mapService.getActiveFeature() != null && vm.interactions.select != null && vm.interactions.select != undefined) {
            var features = vm.interactions.select.getFeatures();
            features.push(mapService.getActiveFeature());
            mapService.getSecondaryFeatures().forEach(function (feature) {
                features.push(feature);
            })
        }
    }

    function styleLayers() {
        mapService.mapLayers.forEach(function (layer) {
            if (mapService.focusedLayers.indexOf(layer) == -1 && mapService.focusedLayers.length != 0) {
                layer.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.INACTIVESTYLE);
            }
            else {
                layer.style = mapStylesFactory.getStyle(mapStylesFactory.styleTypes.ACTIVESTYLE);
            }
            layer.restyle();
        });

    }
}
