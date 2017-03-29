angular.module('mapView')
        .service('mapService', mapService)

function mapService() {
	var vm = this;

	vm.mapLayers = [];
	vm.focusedLayers = [];
	vm.mapButtons = {};
	vm.layersForContext = [];
	vm.activeSelection = null;
	vm.secondarySelections = [];

    vm.selectedSearchResult = function (result) {console.log(result);};
    vm.onNoSearchResult = function(){console.log("No results found!");};
    vm.centerMapOn = function (x, y, z) { console.log({ "x": x, "y": y, "z": z }); };
    vm.centerMapOnFeature = function (feature) { };
	vm.clearDrawingLayer = function(){};
	vm.onSaveButton = function(drawingGeoJson){console.log(drawingGeoJson)};
	vm.onDeleteButton = function(featureId, layer){console.log({"featureID":featureId,"layer":layer})};
	vm.onModify = function(feature){console.log(feature)};
	vm.getFeaturesWithinFeature = function (layer, srcFeature) { };
	vm.getFeaturesInExtent = function (layer, extent, filter) { };
	vm.getVisibleFeatures = function (layer, filter) { };
	vm.onDrawEnd = function (buttonName, feature){console.log(buttonName, feature)};
	vm.getLayer = getLayer;
	vm.addSelectionListener = addSelectionListener;
	vm.removeSelectionListener = removeSelectionListener;
	vm.setSelections = setSelections;
	vm.getActiveSelection = getActiveSelection;
	vm.getSecondarySelections = getSecondarySelections;
	vm.getActiveFeature = getActiveFeature;
	vm.getSecondaryFeatures = getSecondaryFeatures;
	vm.selectionListeners = [];
	vm.getFeaturesWithinFeature = function(){};
	vm.callSelectionListeners = callSelectionListeners;
	vm.refreshLayers = refreshLayers;
	vm.styleLayers = function () { };
	vm.onMap = function (event, callback) { };
	vm.offMap = function (event, callback) { };

	function getLayer(layerName){
		var returnVal = null;
		vm.mapLayers.forEach(function(layer){
			if (layer.layerName == layerName)
				returnVal = layer;
		})
		return returnVal;
	}

	function getActiveSelection() {
		return vm.activeSelection;
	}

	function getActiveFeature(){
		if (vm.activeSelection == null)
			return null;
		return vm.activeSelection.layer.getSource().getFeatureById(vm.activeSelection.featureID);
	}

	function getSecondarySelections(){
		return vm.secondarySelections;
	}

	function getSecondaryFeatures(){
		var secondaryFeatures = [];
		vm.secondarySelections.forEach(function(selection){
			secondaryFeatures.push(selection.layer.getSource().getFeatureById(selection.featureID));
		});
		return secondaryFeatures;
	}

	function setSelections(active, secondary) {
		vm.activeSelection = active;
		vm.secondarySelections = secondary;
		callSelectionListeners();
	}

	function addSelectionListener(callback){
		vm.selectionListeners.push(callback)
	}

	function removeSelectionListener(callback){
    var newListeners = [];
		vm.selectionListeners.forEach(function(listener){
      if (callback != listener){
        newListeners.push(listener);
      }
    })
    vm.selectionListeners = newListeners;
	}

	function callSelectionListeners(){
		var selectedFeatures = [];
		if (vm.activeSelection != null) {
            selectedFeatures.push(getActiveFeature());
        }
        vm.secondarySelections.forEach(function(selection){
				selectedFeatures.push(selection.layer.getSource().getFeatureById(selection.featureID));
			});

		vm.selectionListeners.forEach(function(callback){
			callback(selectedFeatures);
		})
	}

	function refreshLayers() {
	    vm.mapLayers.forEach(function (layer) {
	        layer.layer.setVisible(layer.selected);
	    });
	    vm.layerSummary = getLayerSummary();
	}
}
