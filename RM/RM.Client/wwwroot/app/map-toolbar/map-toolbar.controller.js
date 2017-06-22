'use strict';
angular
    .module('mapToolbar')
    .controller('MapToolbarController', MapToolbarController)

MapToolbarController.$inject = [
       'mapToolbarService',
       '$scope',
       '$rootScope'
       
];

function MapToolbarController(
    mapToolbarService,
    $scope,
    $rootScope
    ) {

    var vm = this;
    vm.setSelectedButton = setSelectedButton;
    vm.showButton = showButton();
    vm.selectedButton = null;
    vm.mapButtons = mapToolbarService.getMapButtons();
    vm.autoSelect = autoSelect();
    vm.state = $rootScope.state;

    function setSelectedButton(button) {
        var shape = getShapeForButton(button);
        var isActive = mapToolbarService.setSelectedButton(button, vm.selectedButton);
        if (isActive) {
            vm.selectedButton = button;
            $scope.$emit('mapToolChange', { "name": button, "shape": shape, "enabled": true });
        }
        else {
            vm.selectedButton = 'select';
            $scope.$emit('mapToolChange', { "name": "select", "shape": shape, "enabled": true });
        }

    }


    function getShapeForButton(button) {
        return mapToolbarService.getShapeForButton(button)
    }

    function showButton(button) {
        return mapToolbarService.showButton(button);
    }

    function autoSelect() {
        mapToolbarService.autoSelect();
        var shape = 'undefined';
        vm.selectedButton = 'select';
        $scope.$emit('mapToolChange', { "name": "select", "shape": shape, "enabled": true });
    }          
}