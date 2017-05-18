'use strict';
angular
    .module('mapKey')
    .controller('MapKeyController', MapKeyController);

MapKeyController.$inject = [
    'mapKeyService'
];

function MapKeyController(
    mapKeyService) {

    var vm = this;
    vm.showKey = showKey;

    vm.pointTypes= mapKeyService.initialize();

    function showKey(id) {
        return mapKeyService.showKey(id);        
    }
    
};