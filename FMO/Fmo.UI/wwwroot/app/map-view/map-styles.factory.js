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

    function activeStyle(feature) {

        var type = angular.isDefined(feature.get) ? feature.get('type') : feature;
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
            case "deliverypoint":
                return deliveryPointStyle;
            default:
                return defaultStyle;
        }
    }

    function inactiveStyle(feature) {
        var type = angular.isDefined(feature.get) ? feature.get('type') : feature;
        switch (type) {
            case "deliverypoint":
                return inactivePointStyle;
            default:
                return inactiveDefaultStyle;
        }
    }

    function selectedStyle(feature) {
        var type = angular.isDefined(feature.get) ? feature.get('type') : feature;
        switch (type) {
            case "deliverypoint":
                return selectedPointStyle;
            case "accesslink":
                return selectedLinkStyle;
            case "roadlink":
                return roadLinkStyle;
            default:
                return defaultStyle;
        }
    }

}
