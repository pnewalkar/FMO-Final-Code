'use strict';
angular.module('mapKey')
    .controller('MapKeyController', ['$scope', 'mapStylesFactory', 'mapService', MapKeyController]);

function MapKeyController($scope, mapStylesFactory, mapService) {

    var vm = this;

    vm.showKey = showKey;

    var activeStyle = mapStylesFactory.getStyle();

    function showKey(id) {
        if (id == "") {
            return true;
        }
        var keys = mapService.mapLayers
            .filter(function (l) { return l.selected; })
            .map(function (l) { return l.keys; });

        var distinct = [];

        for (var i = 0; i < keys.length; i++) {
            for (var j = 0; j < keys[i].length; j++) {
                if (distinct.indexOf(keys[i][j]) == -1) {
                    distinct.push(keys[i][j]);
                }
            }
        }

        return distinct.indexOf(id) != -1;
    }

    vm.pointTypes = [
        {
            "text": "Delivery Point",
            "id": "deliverypoint",
            "style": activeStyle("deliverypoint")
        }
    ];

    vm.lineTypes = [
        {
            "text": "Access Link",
            "id": "accessLink",
            "style": activeStyle("accesslink")
        },
        {
            "text": "Route",
            "id": "route",
            "style": activeStyle("route")
        }
    ];
};