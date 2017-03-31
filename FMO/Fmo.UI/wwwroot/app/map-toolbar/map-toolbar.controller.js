'use strict';
angular.module('mapToolbar')
    .controller('MapToolbarController', mapToolbarController);

    function mapToolbarController ($scope, $rootScope, $timeout, mapService) {

        var vm  = this;

        vm.mapService = mapService;
        vm.setSelectedButton = setSelectedButton;
		vm.deselectButton = deselectButton;
		vm.showButton = showButton;
		vm.selectedButton = null;
		vm.autoSelect = autoSelect;

		function setSelectedButton(button) {
			switch (button){
				case 'delete':
					$scope.$emit('deleteSelectedFeature');
					return;
				case 'save':
					$scope.$emit('saveDrawingLayer');
					return;
			}

			var shape = getShapeForButton(button);

			if (button == vm.selectedButton && mapService.mapButtons.length != 1) {
			    deselectButton(button);
			}
			else if (button != vm.selectedButton) {
			    vm.selectedButton = button;
			    $scope.$emit('mapToolChange', { "name": button, "shape": shape, "enabled": true });
			}
        }

		function deselectButton(button) {
		    var shape = getShapeForButton(button);
			if (button == vm.selectedButton) {
				if (mapService.mapButtons.indexOf('select') >= 0){
					vm.selectedButton = 'select';
					$scope.$emit('mapToolChange', {"name": "select", "shape": shape, "enabled": true});
				} else {
					vm.selectedButton = null;
					$scope.$emit('mapToolChange', {"name": button, "shape": shape, "enabled": false});
				}
			}
		}

		function getShapeForButton(button) {
		    switch (button){		        
		        case 'point':
		            return 'point';		       
		        case 'line':
		        case 'measure':
		            return 'LineString';
		        default:
		            return undefined;

		    }
		}

		function showButton(button) {
			return mapService.mapButtons.indexOf(button) != -1;
		}

		function autoSelect() {
		    if (mapService.mapButtons.length == 1) {
		        setSelectedButton(mapService.mapButtons[0]);
		    }
		}
    }
