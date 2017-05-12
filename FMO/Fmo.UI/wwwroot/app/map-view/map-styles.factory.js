'use strict'

angular.module('mapView')
    .factory('mapStylesFactory', MapStylesFactory)

function MapStylesFactory() {
    var ACTIVESTYLE = 0;
    var INACTIVESTYLE = 1;
    var SELECTEDSTYLE = 2;

    var blockStyles = {};
    var currentColor = 0;

    var whiteFill = new ol.style.Fill({
        color: 'rgba(255,255,255,0.4)'
    });

    var fill = new ol.style.Fill({
        color: 'rgba(255,255,255,0.4)'
    });
    var stroke = new ol.style.Stroke({
        color: '#3399CC',
        width: 1.25
    });

    var inactivestroke = new ol.style.Stroke({
        color: '#a5a5a5',
        width: 1.25
    });

    var pathLinkStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: "purple",
            width: 3
        })
    });

    var connectingLinkStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: "aqua",
            width: 3
        })
    });

    var roadLinkStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgb(16, 115, 5)',
            width: 2
        })
    });

    var accessLinkStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: '#000',
            width: 2
        })
    });

    var splitRouteStyle = new ol.style.Style({
        stroke: new ol.style.Stroke({
            color: "grey",
            width: 3
        })
    });

    var measureStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(0, 0, 0, 0.5)',
            lineDash: [10, 10],
            width: 2
        })
    });

    var deliveryPointStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf041',
            font: 'normal 16px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: '#da202a',
            }),
            stroke: new ol.style.Stroke({
                color: '#000',
                width: 2
            })
        })
    });

    var collectionPointStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf041',
            font: 'normal 18px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: '#0892cb',
            }),
            stroke: new ol.style.Stroke({
                color: '#000',
                width: 2
            })
        })
    });

    var rmgPointStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf041',
            font: 'normal 18px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: '#cb912b',
            }),
            stroke: new ol.style.Stroke({
                color: '#000',
                width: 2
            })
        })
    });

    var routeSimPointStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf1b9',
            font: 'normal 35px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: 'rgb(218,32,42)', // RMG Red
            }),
            stroke: new ol.style.Stroke({
                color: 'black',
                width: 2
            })
        })
    });

    var routeSimPersonStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf007',
            font: 'normal 35px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: 'rgb(218,32,42)', // RMG Red
            }),
            stroke: new ol.style.Stroke({
                color: 'black',
                width: 2
            })
        })
    });

    var inactiveMeasureStyle = new ol.style.Style({
        fill: whiteFill,
        image: new ol.style.Circle({
            fill: fill,
            stroke: inactivestroke,
            radius: 5
        }),
        stroke: new ol.style.Stroke({
            color: 'rgba(0, 0, 0, 0.5)',
            lineDash: [10, 10],
            width: 2
        })
    });

    var inactivePointStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf041',
            font: 'normal 18px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: '#666666',
            }),
            stroke: new ol.style.Stroke({
                color: '#c4c4c4',
                width: 2
            })
        })
    });

    var inactiveRoutesimPoint = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf1b9',
            font: 'normal 35px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: 'rgb(218,32,42)', // RMG Red
            }),
            stroke: new ol.style.Stroke({
                color: 'black',
                width: 2
            })
        })
    });

    var inactiveRoutesimPerson = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf007',
            font: 'normal 35px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: 'rgb(218,32,42)', // RMG Red
            }),
            stroke: new ol.style.Stroke({
                color: 'black',
                width: 2
            })
        })
    });

    var selectedPointStyle = new ol.style.Style({
        text: new ol.style.Text({
            text: '\uf041',
            font: 'normal 18px FontAwesome',
            textBaseline: 'Bottom',
            fill: new ol.style.Fill({
                color: '#62a531',
            }),
            stroke: new ol.style.Stroke({
                color: '#000',
                width: 2
            })
        })
    });

    var selectedLinkStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: '#62a531',
            width: 2
        })
    });


    var inactiveDefaultStyle = new ol.style.Style({
        image: new ol.style.Circle({
            fill: whiteFill,
            stroke: inactivestroke,
            radius: 5
        }),
        fill: whiteFill,
        stroke: inactivestroke
    });

    var defaultStyle = new ol.style.Style({
        image: new ol.style.Circle({
            fill: fill,
            stroke: stroke,
            radius: 5
        }),
        fill: fill,
        stroke: stroke
    });

    var conversionStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(255, 0, 0, 0.5)',
            width: 2
        })
    });

    var tenementStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(46, 114, 23, 1)',
            width: 2
        })
    });

    var complexStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(47, 25, 245, 1)',
            width: 2
        })
    });

    var skyscraperStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(245, 165, 25, 1)',
            width: 2
        })
    });

    var servicePointStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(245, 25, 223, 1)',
            width: 2
        })
    });

    var selectedConversionStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(255, 0, 0, 0.5)',
            width: 4
        })
    });

    var selectedTenementStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(46, 114, 23, 1)',
            width: 4
        })
    });

    var selectedComplexStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(47, 25, 245, 1)',
            width: 4
        })
    });

    var selectedSkyscraperStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(245, 165, 25, 1)',
            width: 4
        })
    });

    var selectedServicePointStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: 'rgba(245, 25, 223, 1)',
            width: 4
        })
    });

    return {
        styleTypes: { ACTIVESTYLE: ACTIVESTYLE, INACTIVESTYLE: INACTIVESTYLE, SELECTEDSTYLE: SELECTEDSTYLE },
        getStyle: getStyle
    }

    function getStyle(styleType) {
        switch (styleType) {
            case ACTIVESTYLE:
                return activeStyle;
            case INACTIVESTYLE:
                return inactiveStyle;
            case SELECTEDSTYLE:
                return selectedStyle;
            default:
                return activeStyle;
        }
    }

    function getRouteMaintenanceStyle(feature) {
        var colours = ["green", "maroon", "blue", "black", "orange", "red", "purple", "deeppink", "darkslategray"];
        var colour = colours[feature.getId() % colours.length];

        var blockStyleOl = new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: colour,
                width: 3
            })
        });

        var blockId = feature.get != undefined  ? feature.getId() : feature;
        if (blockStyles[blockId]) {
            blockStyleOl = blockStyles[blockId];
        } else {
            blockStyles[blockId] = blockStyleOl;
        }
        return blockStyleOl;
    }

    function blockStyle(feature) {
        var colours = ["green", "purple", "blue", "brown", "orange", "red"];
        var colour = colours[currentColor];
        if (currentColor === colours.length) {
            currentColor = 0;
        }

        var blockStyleOl = new ol.style.Style({
            stroke: new ol.style.Stroke({
                color: colour,
                width: 3
            })
        });

        var block = feature.get('block') != undefined  ? feature.get('block') : feature;
        if (block.deadhead) {
            blockStyleOl = new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: "black",
                    width: 3
                })
            });
        }

        if (blockStyles[block.id]) {
            blockStyleOl = blockStyles[block.id];
        } else {
            blockStyles[block.id] = blockStyleOl;
            currentColor++;
        }

        return blockStyleOl;
    }

    function activeStyle(feature) {

        var type = feature.get != undefined  ? feature.get('type') : feature;
        switch (type) {
            case "pathlink":
                return pathLinkStyle;
            case "connectinglink":
                return connectingLinkStyle;
            case "roadlink":
                return roadLinkStyle;
            case "accesslink":
                return accessLinkStyle;
            case "splitroute":
                return splitRouteStyle;
            case "group":
                if (feature.get != undefined ) {
                    return getGroupStyle(feature, false, feature.get('group'));
                }
                return null;
            case "measure":
                return measureStyle;
            case "deliverypoint":
                return deliveryPointStyle;
            case "collectionpoint":
                return collectionPointStyle;
            case "rmgpoint":
                return rmgPointStyle;
            case "routesim":
                return blockStyle(feature);
            case "routesim-point":
                return routeSimPointStyle;
            case "routeMaintenanceLine":
                return getRouteMaintenanceStyle(feature);
            case "routesim-person":
                return routeSimPersonStyle;
            default:
                return defaultStyle;
        }
    }

    function inactiveStyle(feature) {


        var type = feature.get != undefined  ? feature.get('type') : feature;
        switch (type) {
            case "measure":
                return inactiveMeasureStyle;
            case "collectionpoint":
            case "deliverypoint":
            case "rmgpoint":
                return inactivePointStyle;
            case "routesim":
                return blockStyle(feature);
            case "routesim-point":
                return inactiveRoutesimPoint;
            case "routesim-person":
                return inactiveRoutesimPerson;
            default:
                return inactiveDefaultStyle;
        }
    }

    function selectedStyle(feature) {
        var type = feature.get != undefined  ? feature.get('type') : feature;
        switch (type) {
            case "group":
                return getGroupStyle(feature, true);
            case "measure":
                return measureStyle
            case "collectionpoint":
            case "deliverypoint":
            case "rmgpoint":
                return selectedPointStyle;
            case "accesslink":
                return selectedLinkStyle;
            case "roadlink":
                return roadLinkStyle;
            default:
                return defaultStyle;
        }
    }

    function getGroupStyle(feature, selected, simGroup) {


        var groupType = feature.get != undefined  ? feature.get('groupType') : feature;
        if (simGroup) {
            var point = feature.get != undefined  ? feature.get('point') : feature;
            groupType = point.type;
        }
        if (selected) {
            switch (groupType) {
                case "Conversion":
                    return selectedConversionStyle;
                case "Tenement Block":
                    return selectedTenementStyle;
                case "Complex":
                    return selectedComplexStyle;
                case "Skyscraper":
                    return selectedSkyscraperStyle
                case "Service Point":
                    return selectedServicePointStyle;
                default:
                    return defaultStyle;
            }
        }
        else {
            switch (groupType) {
                case "Conversion":
                    return conversionStyle;
                case "Tenement Block":
                    return tenementStyle;
                case "Complex":
                    return complexStyle;
                case "Skyscraper":
                    return skyscraperStyle
                case "Service Point":
                    return servicePointStyle;
                default:
                    return defaultStyle;
            }
        }
    }
}
