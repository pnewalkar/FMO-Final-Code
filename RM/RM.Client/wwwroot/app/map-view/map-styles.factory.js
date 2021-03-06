'use strict'

angular.module('mapView')
    .factory('mapStylesFactory',  [
'$q',
'referencedataApiService',
'referenceDataConstants', MapStylesFactory])


function MapStylesFactory(
$q,
referencedataApiService,
referenceDataConstants) {

    var ACTIVESTYLE = 0;
    var INACTIVESTYLE = 1;
    var SELECTEDSTYLE = 2;
    var pointStyles = [];
    var colors = [];
    loadDeliveryPointColour().then(function (response) {
        colors = response;
    });


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

    function loadDeliveryPointColour() {
        var deferred = $q.defer();
        referencedataApiService.getSimpleListsReferenceData(referenceDataConstants.DeliveryPointColor.AppCategoryName).then(function (response) {
            var dpColors = [];
            angular.forEach(response.listItems, function (colors, key) {
                dpColors.push(colors.value);
                deferred.resolve(dpColors);

            });
        });
        return deferred.promise;
    }

  function deliveryPointStyle(feature){
    if(typeof feature=='object'){
    var modValue = hashCode(feature.getProperties().postcode)%colors.length;
    var colour = colors[modValue];

    if(pointStyles[modValue]) {
        return pointStyles[modValue];
    } else {
        var style = new ol.style.Style({
            image: new ol.style.Circle({
            fill: new ol.style.Fill({
                color: colour,
            }),
                stroke: new ol.style.Stroke({
                    color: '#000000',
                    width: 1
                }),
                radius: 5
            })
        });
        pointStyles[modValue] = style;
        return style;
    }
}
    else{
        
      return  new ol.style.Style({
        image: new ol.style.Circle({
            fill: new ol.style.Fill({
                color: '#da202a',
            }),
            stroke: new ol.style.Stroke({
                color: '#000000',
                width: 2
            }),
            radius: 5
        })
    });
    }

}

    var hashCode = function(value) {
        var hash = 0, i, chr;
        if(value){
        if (value.length === 0) return hash;
        for (i = 0; i < value.length; i++) {
        chr   = value.charCodeAt(i);
        hash  = ((hash << 5) - hash) + chr;
        hash |= 0;
            }
        return hash;
        }
        else{
            return colors.length - 1;
        }
        };

    var selectedPointStyle = new ol.style.Style({
        image: new ol.style.RegularShape({
            fill: new ol.style.Fill({
                color: '#fdda24',
            }),
            stroke: new ol.style.Stroke({
                color: '#000000',
                width: 1
            }),
            points: 3,
            radius: 10,
            angle: 0
        })
    });

    var selectedLinkStyle = new ol.style.Style({
        fill: whiteFill,
        stroke: new ol.style.Stroke({
            color: '#62a531',
            width: 2
        })
    });

    var selectedPolygonStyle = new ol.style.Style({
        fill: new ol.style.Fill({
            color: 'rgba(255, 255, 255, 0.5)'
        }),
        stroke: new ol.style.Stroke({
            color: '#ffcc33',
            width: 2
        }),
        image: new ol.style.Circle({
            radius: 7,
            fill: new ol.style.Fill({
                color: '#ffcc33'
            })
        })
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
                return deliveryPointStyle(feature);
            case "polygon":
                return selectedPolygonStyle;
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
            case "linestring":
            case "polygon": 
                return selectedLinkStyle;
            case "roadlink":
                return roadLinkStyle;          
            default:
                return defaultStyle;
        }
    }

}