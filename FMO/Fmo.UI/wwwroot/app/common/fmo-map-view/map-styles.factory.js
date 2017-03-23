'use strict'

angular.module('mapView')
.factory('mapStylesFactory', MapStylesFactory)

function MapStylesFactory(){
	var ACTIVESTYLE = 0;
	var INACTIVESTYLE = 1;
    var SELECTEDSTYLE = 2;

	var blockStyles = {};
	var currentColor = 0;

    var whiteFill = new ol.style.Fill({
        color: 'rgba(255,255,255,0.4)'
    });

	return {
		styleTypes: {ACTIVESTYLE: ACTIVESTYLE, INACTIVESTYLE: INACTIVESTYLE, SELECTEDSTYLE: SELECTEDSTYLE},
		getStyle: getStyle
	}

	function getStyle(styleType){
		switch (styleType){
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

	function blockStyle(feature) {
		var colours = ["green", "purple", "blue", "brown", "orange", "red"];
		var colour = colours[currentColor];
		if(currentColor === colours.length) {
			currentColor = 0;
		}

		var blockStyleOl = new ol.style.Style({
			stroke: new ol.style.Stroke({
				color: colour,
				width: 3
			})
		});

		var block = feature.get('block') != undefined ? feature.get('block') : feature;
		if(block.deadhead) {
			blockStyleOl = new ol.style.Style({
				stroke: new ol.style.Stroke({
					color: "black",
					width: 3
				})
			});
		}

		if(blockStyles[block.id]) {
			blockStyleOl = blockStyles[block.id];
		} else {
			blockStyles[block.id] = blockStyleOl;
			currentColor++;
		}

		return blockStyleOl;
	}

	function activeStyle(feature) {
		var fill = new ol.style.Fill({
			color: 'rgba(255,255,255,0.4)'
		});
		var stroke = new ol.style.Stroke({
			color: '#3399CC',
			width: 1.25
		});
		var type = feature.get != undefined ? feature.get('type') : feature;
		switch(type) {
			case "splitroute":
				return new ol.style.Style({
					stroke: new ol.style.Stroke({
						color: "grey",
						width: 3
					})
				})
			case "group":
				return getGroupStyle(feature,fill,stroke,false, feature.get('group'));
			case "measure":
				return new ol.style.Style({
					fill: whiteFill,
					stroke: new ol.style.Stroke({
						color: 'rgba(0, 0, 0, 0.5)',
						lineDash: [10, 10],
						width: 2
					})});
			case "deliverypoint":
				return new ol.style.Style ({
					text: new ol.style.Text({
						text: '\uf041',
						font: 'normal 18px FontAwesome',
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
			case "collectionpoint":
				return new ol.style.Style ({
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
			case "rmgpoint":
				return new ol.style.Style ({
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
            case "routesim":
                return blockStyle(feature);
            case "routesim-point":
                return new ol.style.Style({
                    text: new ol.style.Text({
                        text: '\uf0d1',
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
			default:
				return new ol.style.Style({
					image: new ol.style.Circle({
						fill: fill,
						stroke: stroke,
						radius: 5
					}),
					fill: fill,
					stroke: stroke
				});
        }
	}

	function inactiveStyle(feature){

		var stroke = new ol.style.Stroke({
			color: '#a5a5a5',
			width: 1.25
		});
		var type = feature.get != undefined ? feature.get('type') : feature;
		switch(type){
			case "measure":
				return new ol.style.Style({
					fill: whiteFill,
					image: new ol.style.Circle({
						fill: fill,
						stroke: stroke,
						radius: 5
					}),
					stroke: new ol.style.Stroke({
						color: 'rgba(0, 0, 0, 0.5)',
						lineDash: [10, 10],
						width: 2
					})});
			case "collectionpoint":
			case "deliverypoint":
            case "rmgpoint":
                return new ol.style.Style ({
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
			case "routesim":
				return blockStyle(feature);
			case "routesim-point":
				return new ol.style.Style({
					text: new ol.style.Text({
						text: '\uf0d1',
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
			default:
				return new ol.style.Style({
					image: new ol.style.Circle({
						fill: whiteFill,
						stroke: stroke,
						radius: 5
					}),
					fill: whiteFill,
					stroke: stroke
				});
		}
	}

    function selectedStyle(feature){
        var fill = new ol.style.Fill({
            color: 'rgba(255,255,255,0.4)'
        });
        var stroke = new ol.style.Stroke({
            color: '#3399CC',
            width: 1.25
        });
        var type = feature.get != undefined ? feature.get('type') : feature;
        switch(type){
            case "group":
                return getGroupStyle(feature,fill,stroke,true);
            case "measure":
                return new ol.style.Style({
                    fill: whiteFill,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(0, 0, 0, 0.5)',
                        lineDash: [10, 10],
                        width: 2
                    })});
			case "collectionpoint":
            case "deliverypoint":
            case "rmgpoint":
                return new ol.style.Style ({
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
            default:
                return new ol.style.Style({
                    image: new ol.style.Circle({
                        fill: fill,
                        stroke: stroke,
                        radius: 5
                    }),
                    fill: fill,
                    stroke: stroke
                });
        }
    }

    function getGroupStyle(feature,fill,stroke,selected, simGroup)
	{
	    var width = 2;
	    if(selected)
        {
            width = 4;
        }
		var groupType = feature.get != undefined ? feature.get('groupType') : feature;
		if(simGroup) {
			var point = feature.get != undefined ? feature.get('point') : feature;
			groupType = point.type;
		}
		switch(groupType) {
			case "Conversion":
				return new ol.style.Style({
                    fill: whiteFill,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(255, 0, 0, 0.5)',
                        width: width
                    })
				});
            case "Tenement Block":
                return new ol.style.Style({
                    fill: whiteFill,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(46, 114, 23, 1)',
                        width: width
                    })
                });
			case "Complex":
                return new ol.style.Style({
                    fill: whiteFill,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(47, 25, 245, 1)',
                        width: width
                    })
                });
			case "Skyscraper":
                return new ol.style.Style({
                    fill: whiteFill,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(245, 165, 25, 1)',
                        width: width
                    })
                });
			case "Service Point":
                return new ol.style.Style({
                    fill: whiteFill,
                    stroke: new ol.style.Stroke({
                        color: 'rgba(245, 25, 223, 1)',
                        width: width
                    })
                });
			default:
				return new ol.style.Style({
					fill: fill,
					stroke: stroke
				});
		}
    }
}
