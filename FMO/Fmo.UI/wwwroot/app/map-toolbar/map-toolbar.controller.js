'use strict';
angular.module('mapToolbar')
    .controller('MapToolbarController', ['mapToolbarService', '$scope', MapToolbarController])

function MapToolbarController(mapToolbarService, $scope) {

    var vm = this;
    vm.setSelectedButton = setSelectedButton;
    vm.deselectButton = deselectButton;
    vm.showButton = showButton();
    vm.selectedButton = null;
    vm.mapButtons = mapToolbarService.getMapButtons();

    function setSelectedButton(button) {
        var shape = getShapeForButton(button);
        var emit = mapToolbarService.setSelectedButton(button, vm.selectedButton);
        if (emit) {
            $scope.$emit('mapToolChange', { "name": button, "shape": shape, "enabled": true });
        }

    }

    function deselectButton(button) {
        var shape = getShapeForButton(button);
        var emitseelct = mapToolbarService.deselectButton(button);
        if (emitseelct == true) {
            $scope.$emit('mapToolChange', { "name": "select", "shape": shape, "enabled": true });
        }
        else {
            $scope.$emit('mapToolChange', { "name": button, "shape": shape, "enabled": false });
        }

    }

    function getShapeForButton(button) {
        return mapToolbarService.getShapeForButton(button)
    }

    function showButton(button) {
        return mapToolbarService.showButton(button);
    }

}