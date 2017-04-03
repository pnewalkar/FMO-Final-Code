'use strict';
angular.module('mapKey')
    .controller('MapKeyController', ['$scope', 'mapStylesFactory', 'mapService', MapKeyController]);

function MapKeyController($scope, mapStylesFactory, mapService) {

    var vm = this;

    vm.showKey = showKey;

    var activeStyle = mapStylesFactory.getStyle();
    var spoofStyleObj = function (groupType) {
        return {
            "get": function (key) { return this[key]; },
            "type": "group",
            "groupType": groupType
        }
    }

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
        //{
        //    "text": "Selected",
        //    "id": "",
        //    "style": mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE)("deliverypoint")
        //},
        {
            "text": "Delivery Point",
            "id": "deliverypoint",
            "style": activeStyle("deliverypoint")
        }
        //,
        //{
        //    "text": "Collection Point",
        //    "id": "collectionpoint",
        //    "style": activeStyle("collectionpoint")
        //},
        //{
        //    "text": "RMG Point",
        //    "id": "rmgpoint",
        //    "style": activeStyle("rmgpoint")
        //},
        //{
        //    "text": "Van/Person",
        //    "id": "routesim-point",
        //    "style": activeStyle("routesim-point")
        //}
    ];

    vm.areaTypes = [
        //{
        //    "text": "Tenement Block",
        //    "id": "group",
        //    "style": activeStyle(new spoofStyleObj("Tenement Block"))
        //},
        //{
        //    "text": "Conversion",
        //    "id": "group",
        //    "style": activeStyle(new spoofStyleObj("Conversion"))
        //},
        //{
        //    "text": "Complex",
        //    "id": "group",
        //    "style": activeStyle(new spoofStyleObj("Complex"))
        //},
        //{
        //    "text": "Skyscraper",
        //    "id": "group",
        //    "style": activeStyle(new spoofStyleObj("Skyscraper"))
        //},
        //{
        //    "text": "Service Point",
        //    "id": "group",
        //    "style": activeStyle(new spoofStyleObj("Service Point"))
        //}
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
       // ,
       //{
       //    "text": "Path Link",
       //    "id": "pathlink",
       //    "style": activeStyle("pathlink")
       //}
    ];
};