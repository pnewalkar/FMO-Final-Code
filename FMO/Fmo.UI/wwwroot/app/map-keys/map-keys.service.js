angular.module('mapKey')
        .service('mapKeyService', ['mapStylesFactory','mapService', mapToolbarService])

function mapToolbarService(mapStylesFactory,mapService) {
   
    return{
        showKey:showKey,
        initialize:initialize      
    }

    function showKey(id) {
        if (id == "") {
            return true;
        }
        var keys = mapService.mapLayers()
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

    function initialize() {
        var activeStyle = mapStylesFactory.getStyle();

         var pointTypes = [
            {
                "text": "Selected",
                "id": "",
                "style": mapStylesFactory.getStyle(mapStylesFactory.styleTypes.SELECTEDSTYLE)("deliverypoint")
            }, {
                "text": "Delivery Point",
                "id": "deliverypoint",
                "style": activeStyle("deliverypoint")
            },
             {
                 "text": "Access Link",
                 "id": "accesslink",
                 "style": activeStyle("accesslink")
             },
            {
                "text": "Road",
                "id": "roadlink",
                "style": activeStyle("roadlink")
            }
        ];
        return pointTypes;
      
    }
  
}